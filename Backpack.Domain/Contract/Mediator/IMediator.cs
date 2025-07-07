using Backpack.Domain.Enum;
using Backpack.Domain.Model;

namespace Backpack.Domain.Contract.Mediator;

public interface IMediator
{
    Task<Result> SendAsync(ICommand command, CancellationToken cancellationToken = default);
    Task<Result<TResult>> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task<Result<TResult>> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task PublishAsync<TNotification>(INotification notification, eNotificationProcessingType processingType = eNotificationProcessingType.AwaitWhenAll, CancellationToken cancellationToken = default);
    IAsyncEnumerable<Result<TResult>> StreamQueryAsync<TResult>(IStreamQuery<TResult> query, CancellationToken cancellationToken = default);
    IAsyncEnumerable<Result<TResult>> StreamCommandAsync<TResult>(IStreamCommand<TResult> command, CancellationToken cancellationToken = default);
}