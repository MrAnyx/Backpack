using Backpack.Domain.Enum;
using Backpack.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

public class StatusBarMessageStore : ObservableObject
{
    private readonly ObservableCollection<StatusBarMessage> _messages = [];
    public IReadOnlyCollection<StatusBarMessage> Messages => _messages;

    private StatusBarMessage? _latestMessage;
    public StatusBarMessage? LatestMessage
    {
        get => _latestMessage;
        private set => SetProperty(ref _latestMessage, value);
    }

    private readonly StatusBarMessage _defaultMessage = new()
    {
        Message = "Ready",
        Type = eStatusBarMessageType.Info,
        AutoDismissAfter = null
    };

    public StatusBarMessageStore()
    {
        LatestMessage = _defaultMessage;
    }

    public void Add(StatusBarMessage message)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            _messages.Add(message);
            LatestMessage = message;
        });

        if (message.AutoDismissAfter.HasValue)
        {
            message.TimerToken = new CancellationTokenSource();
            _ = AutoDismissAsync(message, message.AutoDismissAfter.Value, message.TimerToken.Token);
        }
    }

    public void Remove(StatusBarMessage message)
    {
        message.TimerToken?.Cancel();

        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            _messages.Remove(message);

            if (_messages.Count == 0)
            {
                LatestMessage = _defaultMessage;
            }
            else if (LatestMessage == message)
            {
                LatestMessage = _messages.LastOrDefault() ?? _defaultMessage;
            }
        });
    }

    public void Clear()
    {
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
        {
            foreach (var message in _messages.ToList())
            {
                message.TimerToken?.Cancel();
            }

            _messages.Clear();
            LatestMessage = _defaultMessage;
        });
    }

    private async Task AutoDismissAsync(StatusBarMessage message, TimeSpan delay, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(delay, cancellationToken);
            Remove(message);
        }
        catch (TaskCanceledException)
        {
            // Ignore if the dismiss timer is cancelled
        }
    }
}
