using System.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace Blueprint.Views.UserControls.Core;

internal sealed class TabItemsSourceCoordinator
{
    private readonly TearableTabControl _owner;
    private readonly TabControl _tabControl;
    private readonly TabItemAdapter _tabAdapter;

    public TabItemsSourceCoordinator(
        TearableTabControl owner,
        TabControl tabControl,
        TabItemAdapter tabAdapter)
    {
        _owner = owner;
        _tabControl = tabControl;
        _tabAdapter = tabAdapter;
    }

    public void OnItemsSourceChanged(IEnumerable? oldSource, IEnumerable? newSource)
    {
        if (oldSource is INotifyCollectionChanged oldNotifyCollectionChanged)
        {
            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.RemoveHandler(
                oldNotifyCollectionChanged,
                nameof(INotifyCollectionChanged.CollectionChanged),
                OnSourceCollectionChanged);
        }

        RebuildAllTabs(newSource);

        if (newSource is INotifyCollectionChanged newNotifyCollectionChanged)
        {
            WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>.AddHandler(
                newNotifyCollectionChanged,
                nameof(INotifyCollectionChanged.CollectionChanged),
                OnSourceCollectionChanged);
        }
    }

    public void RebuildAllTabs(IEnumerable? source)
    {
        foreach (TabItem existing in _tabControl.Items)
        {
            TabItemAdapter.Detach(existing);
        }

        _tabControl.Items.Clear();

        if (source == null)
        {
            return;
        }

        foreach (object? item in source)
        {
            if (item == null)
            {
                continue;
            }

            _tabControl.Items.Add(_tabAdapter.CreateTab(item));
        }
    }

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddItems(e);
                break;

            case NotifyCollectionChangedAction.Remove:
                RemoveItems(e);
                break;

            case NotifyCollectionChangedAction.Replace:
                ReplaceItems(e);
                break;

            case NotifyCollectionChangedAction.Move:
                MoveItems(e);
                break;

            case NotifyCollectionChangedAction.Reset:
                RebuildAllTabs(_owner.ItemsSource);
                break;
            default:
                // nothing to do here.
                break;
        }
    }

    private void AddItems(NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems == null)
        {
            return;
        }

        int insertAt = e.NewStartingIndex;

        foreach (object? item in e.NewItems)
        {
            if (item == null)
            {
                continue;
            }

            TabItem tab = _tabAdapter.CreateTab(item);

            if (insertAt < 0 || insertAt >= _tabControl.Items.Count)
            {
                _tabControl.Items.Add(tab);
            }
            else
            {
                _tabControl.Items.Insert(insertAt++, tab);
            }
        }
    }

    private void RemoveItems(NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems == null)
        {
            return;
        }

        foreach (object? item in e.OldItems)
        {
            if (item == null)
            {
                continue;
            }

            TabItem? tab = _tabAdapter.FindTab(item);
            if (tab == null)
            {
                continue;
            }

            TabItemAdapter.Detach(tab);
            _tabControl.Items.Remove(tab);
        }

        _owner.CloseOwnerFloatingWindowIfEmpty();
    }

    private void ReplaceItems(NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems == null || e.NewItems == null)
        {
            return;
        }

        for (int index = 0; index < e.OldItems.Count; index++)
        {
            object? oldItem = e.OldItems[index];
            object? newItem = e.NewItems[index];

            if (oldItem == null || newItem == null)
            {
                continue;
            }

            TabItem? oldTab = _tabAdapter.FindTab(oldItem);
            if (oldTab == null)
            {
                continue;
            }

            int oldTabIndex = _tabControl.Items.IndexOf(oldTab);
            TabItemAdapter.Detach(oldTab);
            _tabControl.Items[oldTabIndex] = _tabAdapter.CreateTab(newItem);
        }
    }

    private void MoveItems(NotifyCollectionChangedEventArgs e)
    {
        if (e.OldStartingIndex < 0 || e.NewStartingIndex < 0)
        {
            return;
        }

        if (_tabControl.Items[e.OldStartingIndex] is not TabItem tab)
        {
            return;
        }

        _tabControl.Items.RemoveAt(e.OldStartingIndex);
        _tabControl.Items.Insert(e.NewStartingIndex, tab);
    }
}
