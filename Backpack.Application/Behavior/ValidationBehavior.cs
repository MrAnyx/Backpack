using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Model;
using Backpack.Shared.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.Behavior;

public class ValidationBehavior<TRequest, TResult>() : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public uint Order => 0;
    public bool IsEnabled => true;

    public async Task<TResult> HandleAsync(TRequest request, RequestContext context, Func<Task<TResult>> next, CancellationToken cancellationToken)
    {
        Validator.Validate(request);

        return await next();
    }
}
