using Backpack.Application.Service;
using Backpack.Domain.Configuration;
using Backpack.Domain.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Application.Extension;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection service, AppSettings settings)
    {
        return service
            .AddSingleton<IMediator, Mediator>()
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            )
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            )
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            )
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo(typeof(INotificationHandler<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            )
            .Scan(x => x
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(c => c.AssignableTo(typeof(IPipelineBehavior<,>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            )
        ;
    }
}
