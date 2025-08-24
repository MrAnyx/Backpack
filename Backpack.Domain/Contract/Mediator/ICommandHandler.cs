using Backpack.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Domain.Contract.Mediator;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<Result> HandleAsync(TCommand command, PipelineContext context, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> HandleAsync(TCommand command, PipelineContext context, CancellationToken cancellationToken);
}