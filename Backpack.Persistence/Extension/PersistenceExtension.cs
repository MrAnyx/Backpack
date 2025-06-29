using Backpack.Domain.Model.Configuration;
using Backpack.Domain.Persistence.Contract;
using Backpack.Persistence.Repository;
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
