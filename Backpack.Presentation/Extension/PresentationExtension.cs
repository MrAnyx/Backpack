using Backpack.Presentation;
using Backpack.Presentation.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Backpack.Domain.Configuration;
using Backpack.Presentation.Feature.Core;
using Backpack.Presentation.Model;

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
        ;
    }
}
