using Backpack.Domain.Configuration;
using Backpack.Domain.Contract;
using Backpack.Presentation.Feature.Core;
using Backpack.Presentation.Model;
using Backpack.Presentation.Service;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Presentation.Extension;

public static class PresentationExtension
{
    public static IServiceCollection AddPresentation(this IServiceCollection service, AppSettings settings)
    {
        return service
            .AddSingleton<MainVM>()
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo<FeatureViewModel>())
                .As<FeatureViewModel>().As<ViewModel>().AsSelf()
                .WithSingletonLifetime()
            )
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo<DialogViewModel>())
                .As<DialogViewModel>().As<ViewModel>().AsSelf()
                .WithTransientLifetime()
            )
            .AddSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>()
            .AddSingleton<IStatusBarMessageService, StatusBarMessageService>()
            .AddSingleton<StatusBarMessageStore>()
        ;
    }
}
