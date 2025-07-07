using Backpack.Domain.Contract;
using Backpack.Domain.Enum;
using Backpack.Domain.Model;
using Backpack.Presentation.Model;

namespace Backpack.Presentation.Service;

public class StatusBarMessageService(StatusBarMessageStore store) : IStatusBarMessageService
{
    private readonly StatusBarMessageStore _store = store;

    public void Post(string message, eStatusBarMessageType type = eStatusBarMessageType.Info)
    {
        TimeSpan? autoDismiss = type switch
        {
            eStatusBarMessageType.Info => TimeSpan.FromSeconds(5),
            eStatusBarMessageType.Success => TimeSpan.FromSeconds(3),
            _ => null
        };

        var status = new StatusBarMessage
        {
            Message = message,
            Type = type,
            AutoDismissAfter = autoDismiss
        };

        _store.Add(status);
    }
}
