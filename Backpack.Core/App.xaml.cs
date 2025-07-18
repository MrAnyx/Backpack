using Backpack.Application.Extension;
using Backpack.Domain.Configuration;
using Backpack.Infrastructure.Extension;
using Backpack.Persistence.Extension;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Feature.Core;
using Backpack.Shared.Extension;
using Backpack.Shared.Helper;
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
    private readonly IHostBuilder _hostBuilder;
    private readonly IHost _host;
    private readonly ILogger<App> _logger;

    private readonly MainVM _mainVM;

    public App()
    {
        SetCultureInfo(CultureInfo.CurrentCulture);

        _hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;

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
                var settings = context.Configuration.As<AppSettings>(true, true);

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

        _logger = _host.Services.GetRequiredService<ILogger<App>>();

        _mainVM = _host.Services.GetRequiredService<MainVM>();
    }

    private static void SetCultureInfo(CultureInfo culture)
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

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        await _mainVM.OnStartupAsync();

        var main = new Main() { DataContext = _mainVM };
        main.Show();

        await _mainVM.OnActivatedAsync();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _mainVM.OnDeactivatedAsync();

        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }

    private void OnGlobalException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        _logger.LogError(e.Exception, "Unexpected Error");
        MessageBox.Show(e.Exception.Message, "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }
}
