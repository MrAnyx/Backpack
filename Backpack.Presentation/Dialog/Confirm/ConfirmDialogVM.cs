using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.Input;

namespace Backpack.Presentation.Dialog.Confirm;

public partial class ConfirmDialogVM : DialogViewModel
{
    [RelayCommand]
    private void ExecuteClose(bool result)
    {
        base.Close(result);
    }
}
