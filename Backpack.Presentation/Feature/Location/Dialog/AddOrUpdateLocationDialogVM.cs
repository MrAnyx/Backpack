using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Backpack.Presentation.Feature.Location.Dialog;

public abstract partial class AddOrUpdateLocationDialogVM : DialogViewModel
{
    public AddOrUpdateLocationDialogVM(Domain.Entity.Location? location = null)
    {
        Name = location?.Name ?? string.Empty;

        ActionLabel = location == null ? "Create" : "Update";
    }

    [ObservableProperty]
    private string name;

    public string ActionLabel { get; }

    [RelayCommand]
    private async Task ExecuteClose() => await CloseAsync(false);

    [RelayCommand]
    private async Task ExecuteSave() => await CloseAsync(true);
}
