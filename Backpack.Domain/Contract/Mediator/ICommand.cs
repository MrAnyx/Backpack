using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface ICommand : IRequest<Result> { }

public interface ICommand<TResult> : IRequest<Result<TResult>> { }
