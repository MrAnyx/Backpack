using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface IStreamRequest<TResult> : IRequest<IAsyncEnumerable<Result<TResult>>> { }
