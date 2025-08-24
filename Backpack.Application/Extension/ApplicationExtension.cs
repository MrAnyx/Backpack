using Backpack.Application.Service;
using Backpack.Domain.Configuration;
using Backpack.Domain.Contract.Mediator;
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
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(c => c.AssignableTo(typeof(INotificationHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IPipelineMiddleware<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IStreamPipelineBehavior<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IStreamCommandHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IStreamQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            )
        ;
    }
}
