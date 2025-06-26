using Backpack.Application.Extension;
using Backpack.Infrastructure.Extension;
using Backpack.Presentation.Extension;
using Backpack.Presentation.Feature.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Windows;

namespace Backpack.Core;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IHostBuilder _hostBuilder;
    private IHost? _host;

    public App()
    {
        _hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;

                config
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .AddJsonFile($"appsettings.output.json", optional: false, reloadOnChange: false);
            })
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services);
            })
            .ConfigureServices((context, services) =>
            {
                services
                    .AddApplication()
                    .AddPresentation()
                    .AddInfrastructure();
            });
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host = _hostBuilder.Build();
        _host.Start();

        var mainVM = _host.Services.GetRequiredService<MainVM>();
        var mainWindow = new Main { DataContext = mainVM };
        mainWindow.Show();
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
}
