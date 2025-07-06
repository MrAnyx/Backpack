using Backpack.Domain.Model;

namespace Backpack.Domain.Contract;

public interface IQuery<TResult> : IRequest<Result<TResult>> { }
