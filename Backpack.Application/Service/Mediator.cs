using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Enum;
using Backpack.Domain.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Backpack.Application.Service;

public class Mediator(IServiceProvider _provider) : IMediator
{
    public Task<Result> SendAsync(ICommand command, CancellationToken cancellationToken = default)
        => SendInternalAsync(command, typeof(ICommandHandler<>), nameof(ICommandHandler<ICommand>.HandleAsync), cancellationToken);

    public Task<Result<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        => SendInternalAsync(command, typeof(ICommandHandler<,>), nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync), cancellationToken);

    public Task<Result<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => SendInternalAsync(query, typeof(IQueryHandler<,>), nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync), cancellationToken);

    public async Task PublishAsync<TNotification>(INotification notification, eNotificationProcessingType processingType = eNotificationProcessingType.AwaitWhenAll, CancellationToken cancellationToken = default)
    {
        var context = new RequestContext();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handleMethod = handlerType.GetMethod(nameof(INotificationHandler<INotification>.HandleAsync))!;
        var handlers = _provider.GetServices(handlerType);

        var tasks = handlers.Select(handler =>
            (Task)handleMethod.Invoke(handler, [notification, context, cancellationToken])!);

        if (processingType == eNotificationProcessingType.ForeachAwait)
        {
            foreach (var task in tasks)
                await task;
        }
        else if (processingType == eNotificationProcessingType.AwaitWhenAll)
        {
            await Task.WhenAll(tasks);
        }
        else
        {
            throw new NotSupportedException($"Notification processing type {processingType} is not supported");
        }
    }

    public IAsyncEnumerable<Result<TResult>> StreamQueryAsync<TResult>(IStreamQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        var context = new RequestContext();
        var requestType = query.GetType();
        var handlerType = typeof(IStreamQueryHandler<,>).MakeGenericType(requestType, typeof(TResult));
        var handler = _provider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IStreamQueryHandler<IStreamQuery<TResult>, TResult>.HandleAsync))!;

        Func<CancellationToken, IAsyncEnumerable<Result<TResult>>> handlerDelegate =
            ct => (IAsyncEnumerable<Result<TResult>>)method.Invoke(handler, [query, context, ct])!;

        return BuildStreamPipeline((dynamic)query, context, handlerDelegate, cancellationToken)(cancellationToken);
    }

    public IAsyncEnumerable<Result<TResult>> StreamCommandAsync<TResult>(IStreamCommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var context = new RequestContext();
        var requestType = command.GetType();
        var handlerType = typeof(IStreamCommandHandler<,>).MakeGenericType(requestType, typeof(TResult));
        var handler = _provider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod(nameof(IStreamCommandHandler<IStreamCommand<TResult>, TResult>.HandleAsync))!;

        Func<CancellationToken, IAsyncEnumerable<Result<TResult>>> handlerDelegate =
            ct => (IAsyncEnumerable<Result<TResult>>)method.Invoke(handler, [command, context, ct])!;

        return BuildStreamPipeline((dynamic)command, context, handlerDelegate, cancellationToken)(cancellationToken);
    }

    private Task SendInternalAsync(IRequest request, Type handlerBaseType, string methodName, CancellationToken cancellationToken)
    {
        var context = new RequestContext();
        var requestType = request.GetType();
        var handlerType = handlerBaseType.MakeGenericType(requestType);
        var method = handlerType.GetMethod(methodName)!;
        var handler = _provider.GetRequiredService(handlerType);

        var handlerDelegate = () => (Task)method.Invoke(handler, [request, context, cancellationToken])!;

        var pipeline = BuildPipeline((dynamic)request, context, handlerDelegate, cancellationToken);
        return pipeline();
    }

    private Task<TResult> SendInternalAsync<TResult>(IRequest<TResult> request, Type handlerBaseType, string methodName, CancellationToken cancellationToken)
    {
        var context = new RequestContext();
        var requestType = request.GetType();
        var handlerType = handlerBaseType.MakeGenericType(requestType, typeof(TResult));
        var method = handlerType.GetMethod(methodName)!;
        var handler = _provider.GetRequiredService(handlerType);

        var handlerDelegate = () => (Task<TResult>)method.Invoke(handler, [request, context, cancellationToken])!;

        var pipeline = BuildPipeline((dynamic)request, context, handlerDelegate, cancellationToken);
        return pipeline();
    }

    private Func<Task<TResult>> BuildPipeline<TRequest, TResult>(
        TRequest request,
        RequestContext context,
        Func<Task<TResult>> handlerInvocation,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResult>
    {
        var behaviors = _provider
            .GetServices<IPipelineBehavior<TRequest, TResult>>()
            .Where(b => b.IsEnabled)
            .OrderBy(b => b.Order);

        var pipeline = handlerInvocation;
        foreach (var behavior in behaviors.Reverse())
        {
            var next = pipeline;
            pipeline = () => behavior.HandleAsync(request, context, next, cancellationToken);
        }

        return pipeline;
    }

    private Func<CancellationToken, IAsyncEnumerable<Result<TResult>>> BuildStreamPipeline<TRequest, TResult>(
        TRequest request,
        RequestContext context,
        Func<CancellationToken, IAsyncEnumerable<Result<TResult>>> handlerInvocation,
        CancellationToken cancellationToken)
        where TRequest : IStreamRequest<TResult>
    {
        var behaviors = _provider
            .GetServices<IStreamPipelineBehavior<TRequest, TResult>>()
            .Where(b => b.IsEnabled)
            .OrderBy(b => b.Order);

        var pipeline = handlerInvocation;

        foreach (var behavior in behaviors.Reverse())
        {
            var next = pipeline;
            pipeline = ct => behavior.HandleAsync(request, context, next, ct);
        }

        return pipeline;
    }
}
