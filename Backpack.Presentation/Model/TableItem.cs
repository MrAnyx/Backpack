using CommunityToolkit.Mvvm.ComponentModel;

namespace Backpack.Presentation.Model;

public partial class TableItem<T> : ObservableObject
{
    public TableItem(T item)
    {
        Item = item;
    }

    [ObservableProperty]
    private T item;

    [ObservableProperty]
    private bool isChecked;
}
