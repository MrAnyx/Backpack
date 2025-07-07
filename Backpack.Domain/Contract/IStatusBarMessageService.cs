using Backpack.Domain.Enum;

namespace Backpack.Domain.Contract;

public interface IStatusBarMessageService
{
    void Post(string message, eStatusBarMessageType type = eStatusBarMessageType.Info);
}
