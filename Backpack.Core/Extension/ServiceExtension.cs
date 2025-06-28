using Backpack.Shared.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Core.Extension;

public static class ServiceExtension
{
    public static T AddAppSettings<T>(this IServiceCollection services, IConfiguration configuration) where T : class
    {
        var settings = configuration.As<T>(true, true);

        services.AddSingleton(typeof(T), settings);

        return settings;
    }
}
