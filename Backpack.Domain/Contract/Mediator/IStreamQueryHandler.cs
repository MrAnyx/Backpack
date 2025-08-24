using Backpack.Domain.Model;
using System.Collections.Generic;
using System.Threading;

namespace Backpack.Domain.Contract.Mediator;

public interface IStreamQueryHandler<TQuery, TResult> where TQuery : IStreamQuery<TResult>
{
    IAsyncEnumerable<Result<TResult>> HandleAsync(TQuery command, PipelineContext context, CancellationToken cancellationToken);
}