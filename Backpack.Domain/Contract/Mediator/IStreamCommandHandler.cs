using Backpack.Domain.Model;
using System.Collections.Generic;
using System.Threading;

namespace Backpack.Domain.Contract.Mediator;

public interface IStreamCommandHandler<TCommand, TResult> where TCommand : IStreamCommand<TResult>
{
    IAsyncEnumerable<Result<TResult>> HandleAsync(TCommand command, PipelineContext context, CancellationToken cancellationToken);
}