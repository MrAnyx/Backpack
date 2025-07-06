using Microsoft.Extensions.Logging;
using Backpack.Domain.Contract;
using Backpack.Domain.Model;
using Backpack.Shared.Helper;

namespace Backpack.Application.Behavior;

public class ValidationBehavior<TRequest, TResult>(ILogger<ValidationBehavior<TRequest, TResult>> _logger) : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    public uint Order => 0;
    public bool IsEnabled => true;

    public async Task<TResult> HandleAsync(TRequest request, RequestContext context, Func<Task<TResult>> next, CancellationToken cancellationToken)
    {
        // TODO Ajouter un result failure plutôt qu'un exception
        Validator.Validate(request);

        return await next();
    }
}
