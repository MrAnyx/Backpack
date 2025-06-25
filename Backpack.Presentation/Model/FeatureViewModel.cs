using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Model;

public abstract partial class FeatureViewModel : ViewModel
{
    public abstract string Name { get; }
    public abstract PackIconKind Icon { get; }
    public virtual uint Priority { get; } = 0;

    public virtual Task OnStartupAsync()
    {
        IsActive = true;
        return Task.CompletedTask;
    }

    public virtual Task OnLoadedAsync() => Task.CompletedTask;

    public virtual Task OnUnloadedAsync() => Task.CompletedTask;
}
