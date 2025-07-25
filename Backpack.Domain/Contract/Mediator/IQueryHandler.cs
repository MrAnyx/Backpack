using Backpack.Domain.Model;
using System.Threading;
using System.Threading.Tasks;
namespace Backpack.Domain.Contract.Mediator;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> HandleAsync(TQuery query, RequestContext context, CancellationToken cancellationToken);
}