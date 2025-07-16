using Backpack.Presentation.Attribute;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace Backpack.Presentation.Feature.Location.Dialog;

[View(typeof(AddOrUpdateLocationDialog))]
public partial class AddOrUpdateFileLocationDialogVM : AddOrUpdateLocationDialogVM
{
    public AddOrUpdateFileLocationDialogVM(Domain.Entity.FileLocation? fileLocation = null) : base(fileLocation)
    {
        Path = fileLocation?.Path ?? string.Empty;
    }

    [ObservableProperty]
    private string path;

    [RelayCommand]
    private void ExecuteOpenFileExplorer()
    {
        var openFileDialog = new OpenFolderDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            Path = openFileDialog.FolderName;
        }
    }
}
