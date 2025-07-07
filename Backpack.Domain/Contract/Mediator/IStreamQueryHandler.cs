using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface IStreamQueryHandler<TQuery, TResult> where TQuery : IStreamQuery<TResult>
{
    IAsyncEnumerable<Result<TResult>> HandleAsync(TQuery command, RequestContext context, CancellationToken cancellationToken);
}