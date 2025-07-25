using System;
using System.Collections.Generic;

namespace Backpack.Domain.Contract.Collection;

public interface ICollectionView<T> : IEnumerable<T>
{
    IEnumerable<T> Items { get; }

    void SetFilter(Predicate<T>? predicate);
    void Refresh();
}