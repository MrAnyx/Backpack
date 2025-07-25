using Backpack.Presentation.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Backpack.Presentation.Model;

public class FilterableObservableCollection<T> where T : INotifyPropertyChanged
{
    public readonly ObservableCollection<T> Items;
    public readonly ObservableCollection<T> OriginalItems;

    public FilterableObservableCollection(IEnumerable<T> items)
    {
        // Store the original list of items
        Items = [.. items];
        OriginalItems = [.. items];

        // Subscribe to property changes in each item to track updates
        foreach (var item in OriginalItems)
        {
            item.PropertyChanged += OriginalItem_PropertyChanged;
        }

        OriginalItems.CollectionChanged += OriginalItems_CollectionChanged;
    }

    private void OriginalItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OriginalItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // When an item property changes, ensure the original list reflects that change.
        if (sender is T updatedItem)
        {
            var index = Items.IndexOf(updatedItem);
            if (index >= 0)
            {
                Items[index] = updatedItem; // Update the original list with the modified item
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
            Items.AddRange(filteredItems);
        }
    }

    // Reset the filter and restore all items
    public void ResetFilter()
    {
        Items.Clear();
        Items.AddRange(OriginalItems);
    }

    public void Add(T item)
    {
        Items.Add(item);
        OriginalItems.Add(item); // Update the original list
        item.PropertyChanged += OriginalItem_PropertyChanged; // Subscribe to property changes of the new item
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
            item.PropertyChanged -= OriginalItem_PropertyChanged; // Unsubscribe from property changes
        }
        return result;
    }

    // Override Clear method to update the original list when the collection is cleared
    public void Clear()
    {
        Items.Clear();
        OriginalItems.Clear(); // Clear the original list as well
    }

    public void ReplaceAll(Func<T, bool> predicate, T newItem)
    {
        var subscribed = false;

        for (var i = 0; i < OriginalItems.Count; i++)
        {
            var item = OriginalItems[i];
            if (predicate(item) && !ReferenceEquals(item, newItem))
            {
                item.PropertyChanged -= OriginalItem_PropertyChanged;
                OriginalItems[i] = newItem;

                if (!subscribed)
                {
                    newItem.PropertyChanged += OriginalItem_PropertyChanged;
                    subscribed = true;
                }
            }
        }

        for (var i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (predicate(item) && !ReferenceEquals(item, newItem))
            {
                item.PropertyChanged -= OriginalItem_PropertyChanged;
                Items[i] = newItem;

                if (!subscribed)
                {
                    newItem.PropertyChanged += OriginalItem_PropertyChanged;
                    subscribed = true;
                }
            }
        }
    }

    public void UpdateItem(Func<T, bool> predicate, Action<T> updateAction)
    {
        foreach (var item in Items)
        {
            if (predicate(item))
            {
                updateAction(item);
            }
        }

        foreach (var item in OriginalItems)
        {
            if (predicate(item))
            {
                updateAction(item);
            }
        }
    }
}
