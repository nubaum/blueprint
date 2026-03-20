using System.ComponentModel;
using System.Windows.Controls;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Views.UserControls.Core;

internal sealed class TabViewModelTabAdapter
{
    private static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.RegisterAttached(
            "_ViewModel",
            typeof(ITabViewModel),
            typeof(TabViewModelTabAdapter),
            new PropertyMetadata(null));

    private readonly TearableTabControl _owner;
    private readonly TabControl _tabControl;

    public TabViewModelTabAdapter(TearableTabControl owner, TabControl tabControl)
    {
        _owner = owner;
        _tabControl = tabControl;
    }

    public static ITabViewModel? GetViewModel(TabItem tab)
        => (ITabViewModel?)tab.GetValue(ViewModelProperty);

    public static void Detach(TabItem tab)
    {
        tab.ClearValue(ViewModelProperty);
    }

    public TabItem CreateTab(ITabViewModel viewModel)
    {
        var tab = new TabItem
        {
            Content = viewModel,
            ContentTemplate = _owner.ContentTemplate,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
        };

        SetViewModel(tab, viewModel);
        SyncTabFromViewModel(tab, viewModel);

        WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
            viewModel,
            nameof(INotifyPropertyChanged.PropertyChanged),
            (_, args) => OnViewModelPropertyChanged(tab, viewModel, args.PropertyName));

        var isPinnedDescriptor = DependencyPropertyDescriptor.FromProperty(
            TabControlHelper.IsPinnedProperty,
            typeof(TabItem));

        isPinnedDescriptor.AddValueChanged(tab, (_, _) =>
        {
            bool isPinned = TabControlHelper.GetIsPinned(tab);
            if (viewModel.IsPinned != isPinned)
            {
                viewModel.IsPinned = isPinned;
            }
        });

        return tab;
    }

    public TabItem? FindTab(ITabViewModel viewModel)
    {
        foreach (TabItem tab in _tabControl.Items)
        {
            if (ReferenceEquals(GetViewModel(tab), viewModel))
            {
                return tab;
            }
        }

        return null;
    }

    public void ApplyContentTemplateToExistingTabs(DataTemplate? template)
    {
        foreach (TabItem tab in _tabControl.Items)
        {
            tab.ContentTemplate = template;
        }
    }

    private static void SyncTabFromViewModel(TabItem tab, ITabViewModel viewModel)
    {
        tab.Header = viewModel.Caption;
        tab.Tag = viewModel.Icon;
        TabControlHelper.SetIsPinned(tab, viewModel.IsPinned);
        TabControlHelper.SetIsDirty(tab, viewModel.IsDirty);
    }

    private static void SetViewModel(TabItem tab, ITabViewModel viewModel)
    {
        tab.SetValue(ViewModelProperty, viewModel);
    }

    private void OnViewModelPropertyChanged(TabItem tab, ITabViewModel viewModel, string? propertyName)
    {
        if (!_owner.Dispatcher.CheckAccess())
        {
            _owner.Dispatcher.Invoke(() => OnViewModelPropertyChanged(tab, viewModel, propertyName));
            return;
        }

        switch (propertyName)
        {
            case nameof(ITabViewModel.Caption):
                tab.Header = viewModel.Caption;
                break;

            case nameof(ITabViewModel.Icon):
                tab.Tag = viewModel.Icon;
                break;

            case nameof(ITabViewModel.IsPinned):
                TabControlHelper.SetIsPinned(tab, viewModel.IsPinned);
                break;

            case nameof(ITabViewModel.IsDirty):
                TabControlHelper.SetIsDirty(tab, viewModel.IsDirty);
                break;
            default:
                // Nothing to do here.
                break;
        }
    }
}
