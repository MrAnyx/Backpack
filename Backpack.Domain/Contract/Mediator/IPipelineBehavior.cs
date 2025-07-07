using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface IPipelineBehavior<TRequest, TResult> where TRequest : IRequest<TResult>
{
    uint Order { get; }
    bool IsEnabled { get; }

    Task<TResult> HandleAsync(TRequest request, RequestContext context, Func<Task<TResult>> next, CancellationToken cancellationToken);
}