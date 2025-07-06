using Backpack.Application.Extension;
using Backpack.Domain.Configuration;
using Backpack.Infrastructure.Extension;
using Backpack.Persistence;
using Backpack.Persistence.Extension;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Feature.Core;
using Backpack.Shared.Extension;
using Backpack.Shared.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Hosting;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Backpack.Core;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IHostBuilder _hostBuilder;
    private IHost _host;

    public App()
    {
        _hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                IHostEnvironment env = context.HostingEnvironment;

                config
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .AddJsonFile($"appsettings.output.json", optional: false, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .AddUserSecrets<App>();
            })
            .UseNLog()
            .ConfigureServices((context, services) =>
            {
                AppSettings settings = context.Configuration.As<AppSettings>(true, true);

                GlobalDiagnosticsContext.Set("logFilePath", PathResolver.GetLogFilePath(settings.Environment));
                GlobalDiagnosticsContext.Set("logArchivesPath", PathResolver.GetLogArchivesPath(settings.Environment));
                GlobalDiagnosticsContext.Set("nlogFilePath", PathResolver.GetNLogFilePath(settings.Environment));

                services
                    .AddSingleton(settings)

                    .AddApplication(settings)
                    .AddPresentation(settings)
                    .AddInfrastructure(settings)
                    .AddPersistence(settings)
                ;
            });

        _host = _hostBuilder.Build();

    }

    private void SetCultureInfo(CultureInfo culture)
    {
        // Set the culture for all threads (including default for new threads)
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        // Optionally set for the current thread too
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.IetfLanguageTag))
        );
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        SetCultureInfo(CultureInfo.CurrentCulture);

        _host.Start();

        AppSettings appSettings = _host.Services.GetRequiredService<AppSettings>();

        // Database
        ApplicationDbContext dbContext = _host.Services.GetRequiredService<ApplicationDbContext>();
        IEnumerable<string> pendingMigrations = dbContext.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            MessageBox.Show("The database structure has changed. Applying the latest version.", "Applying migrations", MessageBoxButton.OK, MessageBoxImage.Information);
            dbContext.Database.Migrate();
        }

        // Entry point
        MainVM mainVM = _host.Services.GetRequiredService<MainVM>();
        Main main = new() { DataContext = mainVM };
        main.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            _host.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
        }

        base.OnExit(e);
    }

    private void OnGlobalException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        ILogger<App> logger = _host.Services.GetRequiredService<ILogger<App>>();
        logger.LogError(message: "An unexpected error occured", exception: e.Exception);
        MessageBox.Show(e.Exception.Message, "An unexpected error occured", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }
}
