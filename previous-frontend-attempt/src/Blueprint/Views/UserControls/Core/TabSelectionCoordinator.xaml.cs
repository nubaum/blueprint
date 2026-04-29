using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Blueprint.Views.UserControls.Core;

internal sealed class TabSelectionCoordinator
{
    private readonly TearableTabControl _owner;
    private readonly TabControl _tabControl;
    private readonly TabItemAdapter _tabAdapter;

    private bool _selectionChanging;

    public TabSelectionCoordinator(
        TearableTabControl owner,
        TabControl tabControl,
        TabItemAdapter tabAdapter)
    {
        _owner = owner;
        _tabControl = tabControl;
        _tabAdapter = tabAdapter;
    }

    public void Attach()
    {
        WeakEventManager<Selector, SelectionChangedEventArgs>.AddHandler(
            _tabControl,
            nameof(Selector.SelectionChanged),
            OnInternalSelectionChanged);
    }

    public void OnExternalSelectedItemChanged(object? item)
    {
        if (item == null)
        {
            _tabControl.SelectedItem = null;
            return;
        }

        TabItem? tab = _tabAdapter.FindTab(item);
        if (tab != null)
        {
            _tabControl.SelectedItem = tab;
        }
    }

    private void OnInternalSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_selectionChanging)
        {
            return;
        }

        _selectionChanging = true;

        try
        {
            if (_tabControl.SelectedItem is TabItem tab)
            {
                _owner.SelectedItem = TabItemAdapter.GetItem(tab);
            }
            else
            {
                _owner.SelectedItem = null;
            }
        }
        finally
        {
            _selectionChanging = false;
        }
    }
}
