using Backpack.Domain.Contract;
using Backpack.Domain.Enum;
using Backpack.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class StatusBarMessageService(ITranslationManager _translation) : ObservableObject, IStatusBarMessageService
{
    public StatusBarMessage DefaultMessage => new()
    {
        Message = _translation.Translate("StatusBar_Ready"),
        Type = eStatusBarMessageType.Info,
        AutoDismissAfter = null
    };

    private StatusBarMessage? message;
    public StatusBarMessage Message
    {
        get => message ?? DefaultMessage;
        set => SetProperty(ref message, value);
    }

    private CancellationTokenSource? _currentTokenSource;

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
        Message = DefaultMessage;
    }

    private async Task AutoDismissAsync(TimeSpan delay, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(delay, cancellationToken);
            Message = DefaultMessage;
        }
        catch (TaskCanceledException)
        {
            // Ignore if the dismiss timer is cancelled
        }
    }
}
