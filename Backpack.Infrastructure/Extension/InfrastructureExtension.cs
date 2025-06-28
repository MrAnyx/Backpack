using Backpack.Domain.Model.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, AppSettings settings)
    {
        return service;
    }
}
