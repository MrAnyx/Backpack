using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface IQuery<TResult> : IRequest<Result<TResult>> { }
