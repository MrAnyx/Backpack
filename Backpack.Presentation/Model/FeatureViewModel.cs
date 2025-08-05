using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace Backpack.Presentation.Model;

public abstract partial class FeatureViewModel : ViewModel
{
    public abstract string Name { get; }
    public abstract PackIconKind Icon { get; }

    public virtual uint Order { get; } = uint.MaxValue;

    [ObservableProperty]
    private bool isEnabled = true;

    /// <summary>
    /// Is executed once when the application starts
    /// </summary>
    public virtual Task OnStartupAsync() => Task.CompletedTask;

    /// <summary>
    /// Is executed everytime the current view model is loaded on a page switch.
    /// </summary>
    public virtual Task OnActivatedAsync() => Task.CompletedTask;

    /// <summary>
    /// Is executed everytime the current view model is unloaded on a page switch.
    /// </summary>
    public virtual Task OnDeactivatedAsync() => Task.CompletedTask;

    /// <summary>
    /// Is executed everytime the current view model is unloaded on a page switch.
    /// </summary>
    public virtual Task DisposeAsync() => Task.CompletedTask;
}
