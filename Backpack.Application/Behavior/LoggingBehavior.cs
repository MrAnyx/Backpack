using Microsoft.Extensions.Logging;
using Backpack.Domain.Contract;
using Backpack.Domain.Model;

namespace Backpack.Application.Behavior;

public class LoggingBehavior<TRequest, TResult>(ILogger<LoggingBehavior<TRequest, TResult>> _logger) : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    public uint Order => 1;
    public bool IsEnabled => true;

    public async Task<TResult> HandleAsync(TRequest request, RequestContext context, Func<Task<TResult>> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {RequestType} with ID {RequestId} at {UtcTime}",
            typeof(TRequest).Name,
            context.CorrelationId,
            context.UtcTimestamp
        );

        var result = await next();

        _logger.LogInformation(
            "Handled {RequestType} with ID {RequestId}",
            typeof(TRequest).Name,
            context.CorrelationId
        );

        return result;
    }
}
