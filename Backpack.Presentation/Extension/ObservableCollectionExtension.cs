using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Backpack.Presentation.Extension;
public static class ObservableCollectionExtension
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }

    public static void ReplaceAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate, T newItem)
    {
        for (var i = 0; i < collection.Count; i++)
        {
            if (predicate(collection[i]))
            {
                collection.RemoveAt(i);
                collection.Insert(i, newItem);
            }
        }
    }
}
