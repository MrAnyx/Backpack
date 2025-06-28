using Backpack.Domain.Model.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Application.Extension;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection service, AppSettings settings)
    {
        return service;
    }
}
