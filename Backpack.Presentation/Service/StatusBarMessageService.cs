using Backpack.Domain.Contract;
using Backpack.Domain.Enum;
using Backpack.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class StatusBarMessageService : ObservableObject, IStatusBarMessageService
{
    private readonly StatusBarMessage _defaultMessage = new()
    {
        Message = "Ready",
        Type = eStatusBarMessageType.Info,
        AutoDismissAfter = null
    };

    [ObservableProperty]
    private StatusBarMessage message;

    private CancellationTokenSource? _currentTokenSource;

    public StatusBarMessageService()
    {
        Message = _defaultMessage;
    }

    public void Post(string message, eStatusBarMessageType type = eStatusBarMessageType.Info)
    {
        TimeSpan? autoDismiss = type switch
        {
            eStatusBarMessageType.Info => TimeSpan.FromSeconds(5),
            eStatusBarMessageType.Success => TimeSpan.FromSeconds(3),
            _ => null
        };

        _currentTokenSource?.Cancel();

        var status = new StatusBarMessage
        {
            Message = message,
            Type = type,
            AutoDismissAfter = autoDismiss
        };

        Message = status;

        if (status.AutoDismissAfter.HasValue)
        {
            _currentTokenSource = new CancellationTokenSource();
            _ = AutoDismissAsync(status.AutoDismissAfter.Value, _currentTokenSource.Token);
        }
    }

    public void Clear()
    {
        _currentTokenSource?.Cancel();
        Message = _defaultMessage;
    }

    private async Task AutoDismissAsync(TimeSpan delay, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(delay, cancellationToken);
            Message = _defaultMessage;
        }
        catch (TaskCanceledException)
        {
            // Ignore if the dismiss timer is cancelled
        }
    }
}
