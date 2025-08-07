using Backpack.Domain.Configuration;
using Backpack.Domain.Contract;
using Backpack.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, AppSettings settings)
    {
        return service
            .AddSingleton<IUserPreference, UserPreference>()
            .AddSingleton<ITranslationManager, TranslationManager>();
    }
}
