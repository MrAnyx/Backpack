using Backpack.Domain.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Backpack.Domain.Contract.Mediator;

public interface INotificationHandler<TNotification> where TNotification : INotification
{
    Task HandleAsync(TNotification notification, RequestContext context, CancellationToken cancellationToken = default);
}