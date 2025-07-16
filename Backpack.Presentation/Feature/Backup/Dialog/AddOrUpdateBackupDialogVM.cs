using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Backpack.Presentation.Feature.Backup.Dialog;

public partial class AddOrUpdateBackupDialogVM : DialogViewModel
{
    public AddOrUpdateBackupDialogVM(Domain.Entity.Backup? backup = null)
    {
        Name = backup?.Name ?? string.Empty;

        ActionLabel = backup == null ? "Create" : "Update";
    }

    [ObservableProperty]
    private string name;

    public string ActionLabel { get; }

    [RelayCommand]
    private void ExecuteClose() => Close(false);

    [RelayCommand]
    private void ExecuteSave() => Close(true);
}
