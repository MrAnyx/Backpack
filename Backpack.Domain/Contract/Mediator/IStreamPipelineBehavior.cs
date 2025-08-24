using Backpack.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Backpack.Domain.Contract.Mediator;

public interface IStreamPipelineBehavior<TRequest, TResponse> where TRequest : IStreamRequest<TResponse>
{
    uint Order { get; }
    bool IsEnabled { get; }

    IAsyncEnumerable<Result<TResponse>> HandleAsync(TRequest request, PipelineContext context, Func<CancellationToken, IAsyncEnumerable<Result<TResponse>>> next, CancellationToken cancellationToken);
}