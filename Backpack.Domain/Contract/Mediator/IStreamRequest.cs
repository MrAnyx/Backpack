using Backpack.Domain.Model;
using System.Collections.Generic;

namespace Backpack.Domain.Contract.Mediator;

public interface IStreamRequest<TResult> : IRequest<IAsyncEnumerable<Result<TResult>>> { }
