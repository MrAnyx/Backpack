using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.Behavior;

public class ExceptionHandlingBehavior<TRequest, TResult>(ILogger<ExceptionHandlingBehavior<TRequest, TResult>> _logger) : IPipelineMiddleware<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public uint Order => uint.MaxValue;
    public bool IsEnabled => true;

    public async Task<TResult> HandleAsync(TRequest request, PipelineContext context, Func<Task<TResult>> next, CancellationToken cancellationToken)
    {
        try
        {
            var result = await next();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request {Request} with ID {CorrelationId} failed with exception {Message}", typeof(TRequest).Name, context.CorrelationId, ex.Message);
            throw;
        }
    }
}
