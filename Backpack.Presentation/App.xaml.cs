using Backpack.Presentation.Feature.Core;
using Backpack.Presentation.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Windows;

namespace Backpack.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
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
            .UseSerilog((context, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.WithProperty("logPath", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            })
            .ConfigureServices((context, services) =>
            {
                services
                    .AddSingleton<MainVM>()
                    .Scan(x => x
                        .FromDependencyContext(DependencyContext.Default!)
                        .AddClasses(c => c.AssignableTo<ViewModel>())
                        .As<ViewModel>()
                        .AsSelf()
                        .WithSingletonLifetime()
                    );
            });
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _host = _hostBuilder.Build();

        var mainVM = _host.Services.GetRequiredService<MainVM>();

        var mainWindow = new Main
        {
            DataContext = mainVM
        };

        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
