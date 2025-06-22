using Backpack.Presentation.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Backpack.Presentation.Feature.Core;

public partial class MainVM() : ViewModel
{
    [ObservableProperty]
    private readonly ObservableRecipient? currentVM;

    [RelayCommand]
    private static void Loaded()
    {
    }
}
