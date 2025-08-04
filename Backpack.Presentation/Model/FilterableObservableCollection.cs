using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Backpack.Presentation.Model;

public class FilterableObservableCollection<T> : IEnumerable<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<T> Source { get; } = [];

    private Predicate<T>? _filter;

    public int Count => _filter == null ? Source.Count : Source.Count(i => _filter(i));

    public FilterableObservableCollection()
    {
        Source.CollectionChanged += (_, _) => RaiseReset();
    }

    public void SetFilter(Predicate<T>? predicate)
    {
        _filter = predicate;
        RaiseReset();
    }

    public void ClearFilter()
    {
        SetFilter(null);
    }

    public void Add(T item)
    {
        Source.Add(item);
        // Don't raise event here — already handled via Source.CollectionChanged
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public void Remove(T item)
    {
        Source.Remove(item);
        // Handled via Source.CollectionChanged
    }

    public void Clear()
    {
        Source.Clear();
        // Handled via Source.CollectionChanged
    }

    public void UpdateBy(Predicate<T> match, Action<T> update)
    {
        foreach (var item in Source.Where(i => match(i)))
        {
            update(item);
        }

        RaiseReset();
    }

    public void RemoveBy(Predicate<T> match)
    {
        foreach (var item in Source.Where(i => match(i)).ToList())
        {
            Remove(item);
        }

        RaiseReset();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return (_filter == null ? Source : Source.Where(i => _filter(i))).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void RaiseReset()
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }
}