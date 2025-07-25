using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Backpack.Presentation.Dialog.Confirm;

public partial class ConfirmDialogVM : DialogViewModel
{
    [RelayCommand]
    private async Task ExecuteClose(bool result)
    {
        await CloseAsync(result);
    }
}
