using Backpack.Domain.Enum;

namespace Backpack.Domain.Model;

public class StatusBarMessage
{
    public string Message { get; set; } = string.Empty;
    public eStatusBarMessageType Type { get; set; } = eStatusBarMessageType.Info;
    public TimeSpan? AutoDismissAfter { get; set; }

    public CancellationTokenSource? TimerToken { get; set; }
}