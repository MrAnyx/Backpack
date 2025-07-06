using Backpack.Domain.Model;

namespace Backpack.Domain.Contract;

public interface IRequest<TResult> where TResult : Result { }
