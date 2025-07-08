using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Backpack.Presentation.Model;

public class FilterableObservableCollection<T> where T : INotifyPropertyChanged
{
    public readonly ObservableCollection<T> Items;
    public readonly List<T> OriginalItems;

    public FilterableObservableCollection(IEnumerable<T> items)
    {
        // Store the original list of items
        Items = new(items);
        OriginalItems = [.. items];

        // Subscribe to property changes in each item to track updates
        foreach (var item in Items)
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
            Items.Clear();
            foreach (var item in filteredItems)
            {
                Items.Add(item);
            }
        }
    }

    // Reset the filter and restore all items
    public void ResetFilter()
    {
        Items.Clear();
        foreach (var item in OriginalItems)
        {
            Items.Add(item);
        }
    }

    public void Add(T item)
    {
        Items.Add(item);
        OriginalItems.Add(item); // Update the original list
        item.PropertyChanged += Item_PropertyChanged; // Subscribe to property changes of the new item
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public bool Remove(T item)
    {
        var result = Items.Remove(item);
        if (result)
        {
            OriginalItems.Remove(item); // Update the original list
            item.PropertyChanged -= Item_PropertyChanged; // Unsubscribe from property changes
        }
        return result;
    }

    // Override Clear method to update the original list when the collection is cleared
    public void Clear()
    {
        Items.Clear();
        OriginalItems.Clear(); // Clear the original list as well
    }

    // Method to update an item in both the filtered collection and the original list
    public void Update(int index, T updatedItem)
    {
        if (index >= 0 && index < Items.Count)
        {
            var originalItem = Items[index];

            // Update the item in the filtered collection (ObservableCollection)
            Items[index] = updatedItem;

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
