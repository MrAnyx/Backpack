using Backpack.Presentation.Attribute;
using CommunityToolkit.Mvvm.ComponentModel;

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
}
