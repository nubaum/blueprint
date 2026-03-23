using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Blueprint.Presentation.ViewModels.Core;

public class BPObservableCollection<T> : IReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
where T : class
{
    private readonly List<T> _items;

    public BPObservableCollection(IEnumerable<T> values)
    {
        _items = values?.ToList() ?? [];
    }

    public BPObservableCollection()
    {
        _items = [];
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int Count => _items.Count;

    public T this[int index] => _items[index];

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
        NotifyChanges(NotifyCollectionChangedAction.Add, item, _items.Count - 1);
    }

    public void Insert(int index, T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Insert(index, item);
        NotifyChanges(NotifyCollectionChangedAction.Add, item, index);
    }

    public void AddRange(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        var addedItems = items.ToList();
        if (addedItems.Count == 0)
        {
            return;
        }

        int startIndex = _items.Count;
        _items.AddRange(addedItems);
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged("Item[]");
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(
            NotifyCollectionChangedAction.Add, addedItems, startIndex));
    }

    public void SetItems(IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items.Clear();
        _items.AddRange(items);
        NotifyReset();
    }

    public int IndexOf(T item)
    {
        return _items.IndexOf(item);
    }

    public bool Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        int index = _items.IndexOf(item);
        if (index < 0)
        {
            return false;
        }

        _items.RemoveAt(index);
        NotifyChanges(NotifyCollectionChangedAction.Remove, item, index);
        return true;
    }

    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            return false;
        }

        T item = _items[index];
        _items.RemoveAt(index);
        NotifyChanges(NotifyCollectionChangedAction.Remove, item, index);
        return true;
    }

    public int RemoveAll(Predicate<T> match)
    {
        ArgumentNullException.ThrowIfNull(match);
        int removedCount = _items.RemoveAll(match);
        if (removedCount > 0)
        {
            NotifyReset();
        }

        return removedCount;
    }

    public void Clear()
    {
        if (_items.Count == 0)
        {
            return;
        }

        _items.Clear();
        NotifyReset();
    }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    protected virtual void OnPropertyChanged(string propertyName) =>
        System.Windows.Application.Current.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e) =>
        System.Windows.Application.Current.Dispatcher.Invoke(() => CollectionChanged?.Invoke(this, e));

    private void NotifyReset()
    {
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged("Item[]");
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private void NotifyChanges(NotifyCollectionChangedAction action, T item, int index)
    {
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged("Item[]");
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
    }
}
