using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Backpack.Presentation.Model;

public class FilterableObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
{
    public readonly List<T> OriginalItems = [];

    public FilterableObservableCollection(IEnumerable<T> items) : base(items)
    {
        // Store the original list of items
        OriginalItems.AddRange(items);

        // Subscribe to property changes in each item to track updates
        foreach (var item in items)
        {
            item.PropertyChanged += Item_PropertyChanged;
        }
    }

    private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // When an item property changes, ensure the original list reflects that change.
        if (sender is T updatedItem)
        {
            var index = OriginalItems.IndexOf(updatedItem);
            if (index >= 0)
            {
                OriginalItems[index] = updatedItem; // Update the original list with the modified item
            }
        }
    }

    // Filter method
    public void ApplyFilter(Func<T, bool> filter)
    {
        if (filter == null)
        {
            // No filter applied, restore all items
            ResetFilter();
        }
        else
        {
            // Apply the filter and update the collection
            var filteredItems = OriginalItems.Where(filter).ToList();
            base.Clear();
            foreach (var item in filteredItems)
            {
                base.Add(item);
            }
        }
    }

    // Reset the filter and restore all items
    public void ResetFilter()
    {
        base.Clear();
        foreach (var item in OriginalItems)
        {
            base.Add(item);
        }
    }

    // Override Add method to update the original list when an item is added
    public new void Add(T item)
    {
        base.Add(item);
        OriginalItems.Add(item); // Update the original list
        item.PropertyChanged += Item_PropertyChanged; // Subscribe to property changes of the new item
    }

    // Override Remove method to update the original list when an item is removed
    public new bool Remove(T item)
    {
        var result = base.Remove(item);
        if (result)
        {
            OriginalItems.Remove(item); // Update the original list
            item.PropertyChanged -= Item_PropertyChanged; // Unsubscribe from property changes
        }
        return result;
    }

    // Override Clear method to update the original list when the collection is cleared
    public new void Clear()
    {
        base.Clear();
        OriginalItems.Clear(); // Clear the original list as well
    }

    // Method to update an item in both the filtered collection and the original list
    public void Update(int index, T updatedItem)
    {
        if (index >= 0 && index < Count)
        {
            var originalItem = this[index];

            // Update the item in the filtered collection (ObservableCollection)
            this[index] = updatedItem;

            // Update the item in the original list
            var originalIndex = OriginalItems.IndexOf(originalItem);

            if (originalIndex >= 0)
            {
                OriginalItems[originalIndex] = updatedItem;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }
    }
}
