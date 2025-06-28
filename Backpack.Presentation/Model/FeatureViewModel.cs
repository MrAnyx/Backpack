using MaterialDesignThemes.Wpf;

namespace Backpack.Presentation.Model;

public abstract partial class FeatureViewModel : ViewModel
{
    public abstract string Name { get; }
    public abstract PackIconKind Icon { get; }
    public virtual uint Priority { get; } = 0;


    /// <summary>
    /// Is executed once when the application starts
    /// </summary>
    public virtual Task OnStartupAsync()
    {
        IsActive = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Is executed everytime the current view model is loaded on a page switch.
    /// </summary>
    public virtual Task LoadAsync() => Task.CompletedTask;

    /// <summary>
    /// Is executed everytime the current view model is unloaded on a page switch.
    /// </summary>
    public virtual Task UnloadAsync() => Task.CompletedTask;
}
