using System.Collections;
using System.Windows.Controls;

namespace Blueprint.Views.UserControls.Core;

public partial class TearableTabControl : UserControl
{
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(TearableTabControl),
            new PropertyMetadata(null, OnItemsSourceChanged));

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(object),
            typeof(TearableTabControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged));

    public static new readonly DependencyProperty ContentTemplateProperty =
        DependencyProperty.Register(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(TearableTabControl),
            new PropertyMetadata(null, OnContentTemplateChanged));

    public static readonly DependencyProperty CaptionMemberPathProperty =
        DependencyProperty.Register(
            nameof(CaptionMemberPath),
            typeof(string),
            typeof(TearableTabControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IconMemberPathProperty =
        DependencyProperty.Register(
            nameof(IconMemberPath),
            typeof(string),
            typeof(TearableTabControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsPinnedMemberPathProperty =
        DependencyProperty.Register(
            nameof(IsPinnedMemberPath),
            typeof(string),
            typeof(TearableTabControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsDirtyMemberPathProperty =
        DependencyProperty.Register(
            nameof(IsDirtyMemberPath),
            typeof(string),
            typeof(TearableTabControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ContentMemberPathProperty =
        DependencyProperty.Register(
            nameof(ContentMemberPath),
            typeof(string),
            typeof(TearableTabControl),
            new PropertyMetadata(null));

    private readonly TabItemAdapter _tabAdapter;
    private readonly TabItemsSourceCoordinator _itemsSourceCoordinator;
    private readonly TabSelectionCoordinator _selectionCoordinator;
    private readonly TabDragDropCoordinator _dragDropCoordinator;

    public TearableTabControl()
    {
        InitializeComponent();

        _tabAdapter = new TabItemAdapter(this, MainTabControl);
        _itemsSourceCoordinator = new TabItemsSourceCoordinator(this, MainTabControl, _tabAdapter);
        _selectionCoordinator = new TabSelectionCoordinator(this, MainTabControl, _tabAdapter);
        _dragDropCoordinator = new TabDragDropCoordinator(this, MainTabControl);

        _selectionCoordinator.Attach();
        _dragDropCoordinator.Attach();
    }

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public string? CaptionMemberPath
    {
        get => (string?)GetValue(CaptionMemberPathProperty);
        set => SetValue(CaptionMemberPathProperty, value);
    }

    public string? IsDirtyMemberPath
    {
        get => (string?)GetValue(IsDirtyMemberPathProperty);
        set => SetValue(IsDirtyMemberPathProperty, value);
    }

    public string? ContentMemberPath
    {
        get => (string?)GetValue(ContentMemberPathProperty);
        set => SetValue(ContentMemberPathProperty, value);
    }

    public string? IsPinnedMemberPath
    {
        get => (string?)GetValue(IsPinnedMemberPathProperty);
        set => SetValue(IsPinnedMemberPathProperty, value);
    }

    public object? SelectedItem
    {
        get => (object?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public new DataTemplate? ContentTemplate
    {
        get => (DataTemplate?)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public string? IconMemberPath
    {
        get => (string?)GetValue(IconMemberPathProperty);
        set => SetValue(IconMemberPathProperty, value);
    }

    public int TabCount => MainTabControl.Items.Count;

    public void AddTab(TabItem tab)
    {
        if (tab.Parent is TabControl oldOwner)
        {
            oldOwner.Items.Remove(tab);
        }

        MainTabControl.Items.Add(tab);
        MainTabControl.SelectedItem = tab;
        tab.Focus();
    }

    public void RemoveTab(TabItem tab)
    {
        MainTabControl.Items.Remove(tab);
        CloseOwnerFloatingWindowIfEmpty();
    }

    public Window? GetOwnerWindow()
        => Window.GetWindow(this);

    public virtual void CloseOwnerFloatingWindowIfEmpty()
    {
        if (TabCount == 0 && GetOwnerWindow() is FloatingTabWindow floatingWindow)
        {
            floatingWindow.Close();
        }
    }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TearableTabControl)d;
        control._itemsSourceCoordinator.OnItemsSourceChanged(e.OldValue as IEnumerable, e.NewValue as IEnumerable);
    }

    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TearableTabControl)d;
        control._selectionCoordinator.OnExternalSelectedItemChanged(e.NewValue);
    }

    private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TearableTabControl)d;
        control._tabAdapter.ApplyContentTemplateToExistingTabs((DataTemplate?)e.NewValue);
    }
}
