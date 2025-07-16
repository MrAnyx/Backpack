using Backpack.Domain.Configuration;
using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Contract.Repository;
using Backpack.Persistence.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Persistence.Extension;

public static class PersistenceExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection service, AppSettings settings)
    {
        return service
            .AddDbContext<ApplicationDbContext>(options => options.Configure(settings.Environment), ServiceLifetime.Singleton)
            .AddSingleton<IUnitOfWork, UnitOfWork>()
            .AddSingleton<IMigration, Migration>()
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
                .AsImplementedInterfaces().AsSelf()
                .WithSingletonLifetime()
            )
        ;
    }
}
