using Backpack.Domain.Model;

namespace Backpack.Domain.Contract;

public interface INotificationHandler<TNotification> where TNotification : INotification
{
    Task HandleAsync(TNotification notification, RequestContext context, CancellationToken cancellationToken = default);
}