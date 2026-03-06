using System.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;
using Blueprint.ViewModels.Primitives;

namespace Blueprint.Views.UserControls.Primitives;

internal sealed class TabItemsSourceCoordinator
{
    private readonly TearableTabControl _owner;
    private readonly TabControl _tabControl;
    private readonly TabViewModelTabAdapter _tabAdapter;

    public TabItemsSourceCoordinator(
        TearableTabControl owner,
        TabControl tabControl,
        TabViewModelTabAdapter tabAdapter)
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
            TabViewModelTabAdapter.Detach(existing);
        }

        _tabControl.Items.Clear();

        if (source == null)
        {
            return;
        }

        foreach (object? item in source)
        {
            if (item is ITabViewModel viewModel)
            {
                _tabControl.Items.Add(_tabAdapter.CreateTab(viewModel));
            }
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
            if (item is not ITabViewModel viewModel)
            {
                continue;
            }

            TabItem tab = _tabAdapter.CreateTab(viewModel);

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
            if (item is not ITabViewModel viewModel)
            {
                continue;
            }

            TabItem? tab = _tabAdapter.FindTab(viewModel);
            if (tab == null)
            {
                continue;
            }

            TabViewModelTabAdapter.Detach(tab);
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
            if (e.OldItems[index] is not ITabViewModel oldViewModel)
            {
                continue;
            }

            TabItem? oldTab = _tabAdapter.FindTab(oldViewModel);
            if (oldTab == null)
            {
                continue;
            }

            int oldTabIndex = _tabControl.Items.IndexOf(oldTab);
            TabViewModelTabAdapter.Detach(oldTab);

            if (e.NewItems[index] is ITabViewModel newViewModel)
            {
                _tabControl.Items[oldTabIndex] = _tabAdapter.CreateTab(newViewModel);
            }
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
