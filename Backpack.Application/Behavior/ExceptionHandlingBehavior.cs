using Microsoft.Extensions.Logging;
using Backpack.Domain.Contract;
using Backpack.Domain.Model;

namespace Backpack.Application.Behavior;

public class ExceptionHandlingBehavior<TRequest, TResult>(ILogger<ExceptionHandlingBehavior<TRequest, TResult>> _logger) : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    public uint Order => uint.MaxValue;
    public bool IsEnabled => true;

    public async Task<TResult> HandleAsync(TRequest request, RequestContext context, Func<Task<TResult>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = await next();

            if (result.IsFailure)
            {
                _logger.LogWarning("Request {Request} with ID {CorrelationId} safely failed with message {Message}", typeof(TRequest).Name, context.CorrelationId, result.Exception.Message);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request {Request} with ID {CorrelationId} failed with exception {Message}", typeof(TRequest).Name, context.CorrelationId, ex.Message);
            throw;
        }
    }
}
