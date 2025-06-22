using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Model;

public abstract class ViewModel : ObservableRecipient
{
    public abstract string Name { get; }
    public abstract PackIconKind Icon { get; }

    public virtual Task OnStartupAsync()
    {
        IsActive = true;
        return Task.CompletedTask;
    }

    public virtual Task OnLoadedAsync() => Task.CompletedTask;

    public virtual Task OnUnloadedAsync() => Task.CompletedTask;
}
