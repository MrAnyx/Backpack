using Backpack.Domain.Model.Configuration;
using Backpack.Presentation.Feature.Core;
using Backpack.Presentation.Model;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace Backpack.Presentation.Extension;

public static class PresentationExtension
{
    public static IServiceCollection AddPresentation(this IServiceCollection service, AppSettings settings)
    {
        return service
            .AddSingleton<MainVM>()
            .Scan(x => x
                .FromDependencyContext(DependencyContext.Default!)
                .AddClasses(c => c.AssignableTo<FeatureViewModel>())
                    .As<FeatureViewModel>().As<ViewModel>().AsSelf()
                    .WithSingletonLifetime()
            )
            .AddSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>()
        ;
    }
}
