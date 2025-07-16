using Backpack.Domain.Enum;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Model;

public abstract partial class DialogViewModel : ViewModel
{
    public virtual eDialogIdentifier? Identifier { get; set; }

    private eDialogIdentifier? _internalIdentifier;

    public virtual Task OnActivatedAsync() => Task.CompletedTask;
    public virtual Task OnDeactivatedAsync() => Task.CompletedTask;

    private async Task<object?> ShowDialogAsync(eDialogIdentifier? identifier)
    {
        _internalIdentifier = identifier ?? Identifier ?? throw new ArgumentNullException(nameof(identifier), $"Dialog of type {GetType().Name}");

        await OnActivatedAsync();

        return await DialogHost.Show(this, _internalIdentifier);
    }

    public async Task<TResult?> ShowAsync<TResult>(eDialogIdentifier? identifier = null)
    {
        var response = await ShowDialogAsync(identifier);

        if (response is TResult result)
        {
            return result;
        }

        return default;
    }

    public async Task ShowAsync(eDialogIdentifier? identifier = null)
    {
        await ShowDialogAsync(identifier);
    }

    protected async Task CloseAsync<TResult>(TResult result)
    {
        if (!DialogHost.IsDialogOpen(_internalIdentifier))
        {
            throw new Exception($"No dialog found with id {_internalIdentifier}");
        }

        await OnDeactivatedAsync();

        DialogHost.Close(_internalIdentifier, result);
    }

    protected async Task CloseAsync()
    {
        if (!DialogHost.IsDialogOpen(_internalIdentifier))
        {
            throw new Exception($"No dialog found with id {_internalIdentifier}");
        }

        await OnDeactivatedAsync();

        DialogHost.Close(_internalIdentifier, null);
    }
}
