using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface IStreamCommandHandler<TCommand, TResult> where TCommand : IStreamCommand<TResult>
{
    IAsyncEnumerable<Result<TResult>> HandleAsync(TCommand command, RequestContext context, CancellationToken cancellationToken);
}