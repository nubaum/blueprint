using System.ComponentModel;
using System.Windows.Controls;

namespace Blueprint.Views.UserControls.Core;

internal sealed class TabItemAdapter
{
    private static readonly DependencyProperty ItemProperty =
        DependencyProperty.RegisterAttached(
            "_Item",
            typeof(object),
            typeof(TabItemAdapter),
            new PropertyMetadata(null));

    private readonly TearableTabControl _owner;
    private readonly TabControl _tabControl;

    public TabItemAdapter(TearableTabControl owner, TabControl tabControl)
    {
        _owner = owner;
        _tabControl = tabControl;
    }

    public static object? GetItem(TabItem tab)
        => tab.GetValue(ItemProperty);

    public static void Detach(TabItem tab)
    {
        tab.ClearValue(ItemProperty);
    }

    public TabItem CreateTab(object item)
    {
        var tab = new TabItem
        {
            Content = ResolveContent(item),
            ContentTemplate = _owner.ContentTemplate,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
        };

        SetItem(tab, item);
        SyncTabFromItem(tab, item);

        if (item is INotifyPropertyChanged notifyPropertyChanged)
        {
            WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
                notifyPropertyChanged,
                nameof(INotifyPropertyChanged.PropertyChanged),
                (_, args) => OnItemPropertyChanged(tab, item, args.PropertyName));
        }

        var isPinnedDescriptor = DependencyPropertyDescriptor.FromProperty(
            TabControlHelper.IsPinnedProperty,
            typeof(TabItem));

        isPinnedDescriptor.AddValueChanged(tab, (_, _) =>
        {
            TryPushPinnedToItem(tab, item);
        });

        return tab;
    }

    public TabItem? FindTab(object item)
    {
        foreach (TabItem tab in _tabControl.Items)
        {
            if (ReferenceEquals(GetItem(tab), item))
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

    private static void SetItem(TabItem tab, object item)
    {
        tab.SetValue(ItemProperty, item);
    }

    private static bool Matches(string? changedPropertyName, string? memberPath)
    {
        if (string.IsNullOrWhiteSpace(changedPropertyName) || string.IsNullOrWhiteSpace(memberPath))
        {
            return false;
        }

        string leafProperty = memberPath!.Split('.')[^1];
        return string.Equals(changedPropertyName, leafProperty, StringComparison.Ordinal);
    }

    private object ResolveContent(object item)
    {
        if (string.IsNullOrWhiteSpace(_owner.ContentMemberPath))
        {
            return item;
        }

        return TabItemPropertyResolver.GetValue(item, _owner.ContentMemberPath) ?? item;
    }

    private void SyncTabFromItem(TabItem tab, object item)
    {
        tab.Header = TabItemPropertyResolver.GetString(item, _owner.CaptionMemberPath, "Untitled");
        tab.Tag = TabItemPropertyResolver.GetValue(item, _owner.IconMemberPath);
        TabControlHelper.SetIsPinned(tab, TabItemPropertyResolver.GetBool(item, _owner.IsPinnedMemberPath));
        TabControlHelper.SetIsDirty(tab, TabItemPropertyResolver.GetBool(item, _owner.IsDirtyMemberPath));
        tab.Content = ResolveContent(item);
    }

    private void OnItemPropertyChanged(TabItem tab, object item, string? propertyName)
    {
        if (!_owner.Dispatcher.CheckAccess())
        {
            _owner.Dispatcher.Invoke(() => OnItemPropertyChanged(tab, item, propertyName));
            return;
        }

        if (Matches(propertyName, _owner.CaptionMemberPath))
        {
            tab.Header = TabItemPropertyResolver.GetString(item, _owner.CaptionMemberPath, "Untitled");
        }

        if (Matches(propertyName, _owner.IconMemberPath))
        {
            tab.Tag = TabItemPropertyResolver.GetValue(item, _owner.IconMemberPath);
        }

        if (Matches(propertyName, _owner.IsPinnedMemberPath))
        {
            TabControlHelper.SetIsPinned(tab, TabItemPropertyResolver.GetBool(item, _owner.IsPinnedMemberPath));
        }

        if (Matches(propertyName, _owner.IsDirtyMemberPath))
        {
            TabControlHelper.SetIsDirty(tab, TabItemPropertyResolver.GetBool(item, _owner.IsDirtyMemberPath));
        }

        if (Matches(propertyName, _owner.ContentMemberPath))
        {
            tab.Content = ResolveContent(item);
        }
    }

    private void TryPushPinnedToItem(TabItem tab, object item)
    {
        if (string.IsNullOrWhiteSpace(_owner.IsPinnedMemberPath))
        {
            return;
        }

        string propertyName = _owner.IsPinnedMemberPath!;

        PropertyDescriptor? property = TypeDescriptor.GetProperties(item)[propertyName];
        if (property == null || property.IsReadOnly || property.PropertyType != typeof(bool))
        {
            return;
        }

        bool isPinned = TabControlHelper.GetIsPinned(tab);
        bool currentValue = TabItemPropertyResolver.GetBool(item, propertyName);

        if (currentValue != isPinned)
        {
            property.SetValue(item, isPinned);
        }
    }
}
