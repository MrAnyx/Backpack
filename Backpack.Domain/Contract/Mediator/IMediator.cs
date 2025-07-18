using Backpack.Domain.Enum;
using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface IMediator
{
    Task<Result> SendAsync(ICommand command, CancellationToken cancellationToken = default);
    Task<Result<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task<Result<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task PublishAsync<TNotification>(INotification notification, eNotificationProcessingType processingType = eNotificationProcessingType.AwaitWhenAll, CancellationToken cancellationToken = default);
}