using Backpack.Domain.Enum;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Model;

public abstract partial class DialogViewModel : ViewModel
{
    public virtual eDialogIdentifier? Identifier { get; set; }

    private eDialogIdentifier? _internalIdentifier;

    private async Task<object?> ShowDialogAsync(eDialogIdentifier? identifier)
    {
        _internalIdentifier = identifier ?? Identifier ?? throw new ArgumentNullException(nameof(identifier), $"Dialog of type {GetType().Name}");

        return await DialogHost.Show(this, _internalIdentifier);
    }

    public async Task<TResult?> ShowAsync<TResult>(eDialogIdentifier? identifier = null)
    {
        object? response = await ShowDialogAsync(identifier);

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

    protected void Close<TResult>(TResult result)
    {
        if (!DialogHost.IsDialogOpen(_internalIdentifier))
        {
            throw new Exception($"No dialog found with id {_internalIdentifier}");
        }

        DialogHost.Close(_internalIdentifier, result);
    }

    protected void Close()
    {
        if (!DialogHost.IsDialogOpen(_internalIdentifier))
        {
            throw new Exception($"No dialog found with id {_internalIdentifier}");
        }

        DialogHost.Close(_internalIdentifier, null);
    }
}
