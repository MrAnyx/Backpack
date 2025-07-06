using Microsoft.Extensions.Logging;
using Backpack.Domain.Contract;
using Backpack.Domain.Model;
using System.Diagnostics;

namespace Backpack.Application.Behavior;

public class PerformanceBehavior<TRequest, TResult>(ILogger<ExceptionHandlingBehavior<TRequest, TResult>> _logger) : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    public uint Order => 2;
    public bool IsEnabled => true;

    public async Task<TResult> HandleAsync(TRequest request, RequestContext context, Func<Task<TResult>> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await next();
        stopwatch.Stop();

        _logger.LogInformation(
            "Handled {RequestType} with ID {RequestId} in {Duration}ms",
            typeof(TRequest).Name,
            context.CorrelationId,
            stopwatch.ElapsedMilliseconds
        );

        return result;
    }
}
