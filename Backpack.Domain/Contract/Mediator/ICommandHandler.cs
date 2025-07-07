using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<Result> HandleAsync(TCommand command, RequestContext context, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> HandleAsync(TCommand command, RequestContext context, CancellationToken cancellationToken);
}