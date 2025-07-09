using Backpack.Domain.Enum;
using Backpack.Domain.Model;

namespace Backpack.Domain.Contract;

public interface IStatusBarMessageService
{
    StatusBarMessage Message { get; }

    void Post(string message, eStatusBarMessageType type = eStatusBarMessageType.Info);
}
