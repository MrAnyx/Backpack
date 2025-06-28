using Backpack.Domain.Contract;
using Backpack.Domain.Model.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Persistence.Extension;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection service, AppSettings settings)
    {
        return service
            .AddDbContext<ApplicationDbContext>(options => options.Configure(settings.Environment))
            .AddSingleton<IUnitOfWork, UnitOfWork>();
    }
}
