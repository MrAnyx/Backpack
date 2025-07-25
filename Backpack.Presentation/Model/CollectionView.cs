using Backpack.Domain.Contract.Collection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

public class CollectionViewAdapter<T> : ICollectionView<T>
{
    private readonly ICollectionView _view;
    private Predicate<T>? _filter;

    public ObservableCollection<T> Source { get; }
    public IEnumerable<T> Items => _view.Cast<T>();

    public CollectionViewAdapter(ObservableCollection<T> source)
    {
        Source = source;
        _view = CollectionViewSource.GetDefaultView(source);
        _view.Filter = InternalFilter;
    }

    private bool InternalFilter(object item) => item is T t && (_filter?.Invoke(t) ?? true);

    public void SetFilter(Predicate<T>? predicate)
    {
        _filter = predicate;
        _view.Refresh();
    }

    public void Refresh() => _view.Refresh();

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
