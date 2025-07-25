using CommunityToolkit.Mvvm.ComponentModel;

namespace Backpack.Presentation.Model;

public partial class TableItem<T>(T item) : ObservableObject
{
    [ObservableProperty]
    private T item = item;

    [ObservableProperty]
    private bool isChecked;
}
