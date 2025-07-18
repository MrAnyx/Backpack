using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Enum;
using Backpack.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Backpack.Application.Service;

public class Mediator(IServiceProvider _provider) : IMediator
{
    public Task<Result> SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var context = new RequestContext();
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        var methodName = nameof(ICommandHandler<ICommand>.HandleAsync);
        var method = handlerType.GetMethod(methodName)!;

        return InvokePipelineAsync<ICommand, Result>(command, handlerType, method, context, cancellationToken);
    }

    public Task<Result<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var context = new RequestContext();

        var queryType = command.GetType();
        var resultType = typeof(TResult);
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(queryType, resultType);

        var methodName = nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync);
        var method = handlerType.GetMethod(methodName)!;

        return InvokePipelineAsync<ICommand<TResult>, Result<TResult>>(command, handlerType, method, context, cancellationToken);
    }

    public Task<Result<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var context = new RequestContext();

        var queryType = query.GetType();
        var resultType = typeof(TResult);
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, resultType);

        var methodName = nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync);
        var method = handlerType.GetMethod(methodName)!;

        return InvokePipelineAsync<IQuery<TResult>, Result<TResult>>(query, handlerType, method, context, cancellationToken);
    }

    public async Task PublishAsync<TNotification>(INotification notification, eNotificationProcessingType processingType = eNotificationProcessingType.AwaitWhenAll, CancellationToken cancellationToken = default)
    {
        var context = new RequestContext();
        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
        var handleAsyncMethod = handlerType.GetMethod("HandleAsync");

        var handlers = _provider.GetServices(handlerType);

        if (processingType == eNotificationProcessingType.ForeachAwait)
        {
            foreach (var handler in handlers)
            {
                await (Task)handleAsyncMethod!.Invoke(handler, [notification, context, cancellationToken])!;
            }
        }
        else if (processingType == eNotificationProcessingType.AwaitWhenAll)
        {
            var tasks = handlers.Select(handler =>
                (Task)handleAsyncMethod!.Invoke(handler, [notification, context, cancellationToken])!);

            await Task.WhenAll(tasks);
        }
    }

    private Task<TResult> InvokePipelineAsync<TRequest, TResult>(TRequest request, Type handlerType, MethodInfo handlerMethod, RequestContext context, CancellationToken cancellationToken)
        where TRequest : IRequest<TResult>
        where TResult : Result
    {
        var handler = _provider.GetRequiredService(handlerType);

        var handlerDelegate = () =>
        {
            var resultTask = (Task<TResult>)handlerMethod.Invoke(handler, [request, context, cancellationToken])!;
            return resultTask;
        };

        var pipeline = BuildPipeline(request, context, handlerDelegate, cancellationToken);
        return pipeline();
    }

    private Func<Task<TResult>> BuildPipeline<TRequest, TResult>(TRequest request, RequestContext context, Func<Task<TResult>> handlerInvocation, CancellationToken cancellationToken)
        where TRequest : IRequest<TResult>
        where TResult : Result
    {
        var behaviors = _provider
            .GetServices<IPipelineBehavior<TRequest, TResult>>()
            .Where(b => b.IsEnabled)
            .OrderBy(b => b.Order)
            .ToList();

        var pipeline = handlerInvocation;

        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            pipeline = () => behavior.HandleAsync(request, context, next, cancellationToken);
        }

        return pipeline;
    }
}