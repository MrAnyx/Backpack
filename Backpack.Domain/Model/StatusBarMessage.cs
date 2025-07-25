using Backpack.Domain.Enum;
using System;
using System.Threading;

namespace Backpack.Domain.Model;

public class StatusBarMessage
{
    public string Message { get; set; } = string.Empty;
    public eStatusBarMessageType Type { get; set; } = eStatusBarMessageType.Info;
    public TimeSpan? AutoDismissAfter { get; set; }

    public CancellationTokenSource? TimerToken { get; set; }
}