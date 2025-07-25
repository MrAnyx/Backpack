using Backpack.Domain.Contract.Mediator;
using Backpack.Domain.Contract.Persistence;
using Backpack.Domain.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Application.Behavior;

public class DatabaseTransactionBehavior<TRequest, TResult>(IUnitOfWork _unitOfWork) : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public uint Order => 50;
    public bool IsEnabled => false;

    public async Task<TResult> HandleAsync(TRequest request, RequestContext context, Func<Task<TResult>> next, CancellationToken cancellationToken)
    {
        if (request is not ICommand and not ICommand<TResult>)
        {
            return await next();
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return response;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}