using Backpack.Domain.Model;

namespace Backpack.Domain.Contract;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> HandleAsync(TQuery query, RequestContext context, CancellationToken cancellationToken);
}