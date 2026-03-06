using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Blueprint.ViewModels.Primitives;

namespace Blueprint.Views.UserControls.Primitives;

public partial class TearableTabControl : UserControl
{
    // -------------------------------------------------------------------------
    //  Drag state
    // -------------------------------------------------------------------------
    private Point _dragStartPoint;
    private bool _isDragging;
    private TabItem? _draggingTab;
    private DragAdorner? _adorner;
    private AdornerLayer? _adornerLayer;

    private const double DragThreshold = 6.0;

    // -------------------------------------------------------------------------
    //  ctor
    // -------------------------------------------------------------------------
    public TearableTabControl()
    {
        InitializeComponent();

        WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(
            MainTabControl, nameof(UIElement.PreviewMouseLeftButtonDown), Tab_MouseLeftButtonDown);
        WeakEventManager<UIElement, MouseEventArgs>.AddHandler(
            MainTabControl, nameof(UIElement.PreviewMouseMove), Tab_MouseMove);
        WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(
            MainTabControl, nameof(UIElement.PreviewMouseLeftButtonUp), Tab_MouseLeftButtonUp);
        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            MainTabControl, nameof(UIElement.Drop), TabControl_Drop);
        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            MainTabControl, nameof(UIElement.DragOver), TabControl_DragOver);
        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            MainTabControl, nameof(UIElement.DragLeave), TabControl_DragLeave);

        WeakEventManager<Selector, SelectionChangedEventArgs>.AddHandler(
            MainTabControl, nameof(Selector.SelectionChanged), OnInternalSelectionChanged);

        WeakEventManager<ItemContainerGenerator, ItemsChangedEventArgs>.AddHandler(
            MainTabControl.ItemContainerGenerator,
            nameof(ItemContainerGenerator.ItemsChanged),
            TabControl_ItemsChanged);

    }

    // =========================================================================
    //  Dependency Properties
    // =========================================================================

    // -- ItemsSource ----------------------------------------------------------

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(TearableTabControl),
            new PropertyMetadata(null, OnItemsSourceChanged));

    /// <summary>
    /// Bind to an ObservableCollection&lt;T&gt; where T : ITabViewModel.
    /// The control will synthesize a TabItem for every element.
    /// </summary>
    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (TearableTabControl)d;

        // Unsubscribe from old collection
        if (e.OldValue is INotifyCollectionChanged oldNcc)
            oldNcc.CollectionChanged -= ctrl.OnSourceCollectionChanged;

        ctrl.RebuildAllTabs(e.NewValue as IEnumerable);

        // Subscribe to new collection
        if (e.NewValue is INotifyCollectionChanged newNcc)
            newNcc.CollectionChanged += ctrl.OnSourceCollectionChanged;
    }

    // -- SelectedItem ---------------------------------------------------------

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(
            nameof(SelectedItem),
            typeof(ITabViewModel),
            typeof(TearableTabControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged));

    /// <summary>
    /// Two-way bindable selected tab view-model.
    /// </summary>
    public ITabViewModel? SelectedItem
    {
        get => (ITabViewModel?)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (TearableTabControl)d;
        if (e.NewValue is ITabViewModel vm)
        {
            var tab = ctrl.FindTabForViewModel(vm);
            if (tab != null)
                ctrl.MainTabControl.SelectedItem = tab;
        }
    }

    // -- ContentTemplate ------------------------------------------------------

    public static readonly DependencyProperty ContentTemplateProperty =
        DependencyProperty.Register(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(TearableTabControl),
            new PropertyMetadata(null, OnContentTemplateChanged));

    /// <summary>
    /// DataTemplate used to render the body of each tab.
    /// The DataContext will be the ITabViewModel.
    /// </summary>
    public DataTemplate? ContentTemplate
    {
        get => (DataTemplate?)GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    private static void OnContentTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl = (TearableTabControl)d;
        // Propagate to all existing synthesised tabs
        foreach (TabItem tab in ctrl.MainTabControl.Items)
            tab.ContentTemplate = (DataTemplate?)e.NewValue;
    }

    // =========================================================================
    //  Public surface (imperative API — still works for tear-off windows etc.)
    // =========================================================================

    public int TabCount => MainTabControl.Items.Count;

    /// <summary>
    /// Directly add a fully-constructed TabItem (used by tear-off / drop logic).
    /// </summary>
    public void AddTab(TabItem tab)
    {
        if (tab.Parent is TabControl oldOwner)
            oldOwner.Items.Remove(tab);

        MainTabControl.Items.Add(tab);
        MainTabControl.SelectedItem = tab;
        tab.Focus();
    }

    /// <summary>
    /// Remove a TabItem from the inner control.
    /// </summary>
    public void RemoveTab(TabItem tab)
    {
        MainTabControl.Items.Remove(tab);
        CloseOwnerFloatingWindowIfEmpty();
    }

    public Window? GetOwnerWindow() => Window.GetWindow(this);

    // =========================================================================
    //  ItemsSource plumbing
    // =========================================================================

    private void RebuildAllTabs(IEnumerable? source)
    {
        // Detach property-changed listeners from existing VMs
        foreach (TabItem existing in MainTabControl.Items)
            DetachViewModel(existing);

        MainTabControl.Items.Clear();

        if (source == null)
            return;

        foreach (var item in source)
        {
            if (item is ITabViewModel vm)
                MainTabControl.Items.Add(CreateTabForViewModel(vm));
        }
    }

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems == null) break;
                int insertAt = e.NewStartingIndex;
                foreach (var item in e.NewItems)
                {
                    if (item is not ITabViewModel vm) continue;
                    var tab = CreateTabForViewModel(vm);
                    if (insertAt < 0 || insertAt >= MainTabControl.Items.Count)
                        MainTabControl.Items.Add(tab);
                    else
                        MainTabControl.Items.Insert(insertAt++, tab);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems == null) break;
                foreach (var item in e.OldItems)
                {
                    if (item is not ITabViewModel vm) continue;
                    var tab = FindTabForViewModel(vm);
                    if (tab == null) continue;
                    DetachViewModel(tab);
                    MainTabControl.Items.Remove(tab);
                }
                CloseOwnerFloatingWindowIfEmpty();
                break;

            case NotifyCollectionChangedAction.Replace:
                if (e.OldItems == null || e.NewItems == null) break;
                for (int i = 0; i < e.OldItems.Count; i++)
                {
                    if (e.OldItems[i] is ITabViewModel oldVm)
                    {
                        var tab = FindTabForViewModel(oldVm);
                        if (tab == null) continue;
                        DetachViewModel(tab);
                        int idx = MainTabControl.Items.IndexOf(tab);
                        if (e.NewItems[i] is ITabViewModel newVm)
                            MainTabControl.Items[idx] = CreateTabForViewModel(newVm);
                    }
                }
                break;

            case NotifyCollectionChangedAction.Move:
                {
                    var tab = MainTabControl.Items[e.OldStartingIndex] as TabItem;
                    if (tab == null) break;
                    MainTabControl.Items.RemoveAt(e.OldStartingIndex);
                    MainTabControl.Items.Insert(e.NewStartingIndex, tab);
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                RebuildAllTabs(ItemsSource);
                break;
        }
    }

    // -------------------------------------------------------------------------
    //  Tab ↔ ViewModel binding helpers
    // -------------------------------------------------------------------------

    /// <summary>Tag used to store the VM reference on the synthesised TabItem.</summary>
    private static readonly object TabViewModelKey = new();

    private TabItem CreateTabForViewModel(ITabViewModel vm)
    {
        var tab = new TabItem
        {
            // Header text & icon are set via SyncTab; content comes from the template.
            Content = vm,
            ContentTemplate = ContentTemplate,
        };

        // Store VM reference so we can look the tab up later
        tab.SetValue(FrameworkElement.TagProperty, TabViewModelKey); // sentinel
        SetTabViewModel(tab, vm);

        // Apply the VM's initial property values
        SyncTabFromViewModel(tab, vm);

        // Wire up two-way syncing:
        //   VM → Tab  (INotifyPropertyChanged)
        WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>.AddHandler(
            vm, nameof(INotifyPropertyChanged.PropertyChanged),
            (s, e) => OnViewModelPropertyChanged(tab, vm, e.PropertyName));

        //   Tab (IsPinned) → VM  (via DependencyPropertyDescriptor)
        var isPinnedDesc = DependencyPropertyDescriptor.FromProperty(
            TabControlHelper.IsPinnedProperty, typeof(TabItem));
        isPinnedDesc.AddValueChanged(tab, (s, _) =>
        {
            var isPinned = TabControlHelper.GetIsPinned(tab);
            if (vm.IsPinned != isPinned)
                vm.IsPinned = isPinned;
        });

        return tab;
    }

    private static void SyncTabFromViewModel(TabItem tab, ITabViewModel vm)
    {
        tab.Header = vm.Caption;
        // The style uses TabItem.Tag for the icon slot
        tab.Tag = vm.Icon;
        TabControlHelper.SetIsPinned(tab, vm.IsPinned);
        TabControlHelper.SetIsDirty(tab, vm.IsDirty);
    }

    private void OnViewModelPropertyChanged(TabItem tab, ITabViewModel vm, string? propertyName)
    {
        // Make sure we're on the UI thread (VM may raise from a background thread)
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => OnViewModelPropertyChanged(tab, vm, propertyName));
            return;
        }

        switch (propertyName)
        {
            case nameof(ITabViewModel.Caption):
                tab.Header = vm.Caption;
                break;
            case nameof(ITabViewModel.Icon):
                tab.Tag = vm.Icon;
                break;
            case nameof(ITabViewModel.IsPinned):
                TabControlHelper.SetIsPinned(tab, vm.IsPinned);
                break;
            case nameof(ITabViewModel.IsDirty):
                TabControlHelper.SetIsDirty(tab, vm.IsDirty);
                break;
        }
    }

    private static void DetachViewModel(TabItem tab)
    {
        // Nothing explicit needed — WeakEventManager and DependencyPropertyDescriptor
        // callbacks use weak references, so they won't prevent GC.
        // Clear the stored reference anyway.
        tab.ClearValue(ViewModelProperty);
    }

    // Attached DP to store the VM on the TabItem without touching Tag (Tag = icon)
    private static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.RegisterAttached(
            "_ViewModel",
            typeof(ITabViewModel),
            typeof(TearableTabControl),
            new PropertyMetadata(null));

    private static void SetTabViewModel(TabItem tab, ITabViewModel vm)
        => tab.SetValue(ViewModelProperty, vm);

    private static ITabViewModel? GetTabViewModel(TabItem tab)
        => (ITabViewModel?)tab.GetValue(ViewModelProperty);

    private TabItem? FindTabForViewModel(ITabViewModel vm)
    {
        foreach (TabItem tab in MainTabControl.Items)
            if (ReferenceEquals(GetTabViewModel(tab), vm))
                return tab;
        return null;
    }

    // -------------------------------------------------------------------------
    //  Two-way SelectedItem sync
    // -------------------------------------------------------------------------

    private bool _selectionChanging;

    private void OnInternalSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_selectionChanging) return;
        _selectionChanging = true;
        try
        {
            if (MainTabControl.SelectedItem is TabItem tab)
                SelectedItem = GetTabViewModel(tab);
            else
                SelectedItem = null;
        }
        finally
        {
            _selectionChanging = false;
        }
    }

    // =========================================================================
    //  Drag-and-drop / tear-off (unchanged logic, kept intact)
    // =========================================================================

    private void Tab_MouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
    {
        var originalSource = e.OriginalSource as DependencyObject;
        if (IsHeaderButtonClick(originalSource)) return;

        var tab = GetTabItemFromSource(originalSource);
        if (tab == null) return;

        _dragStartPoint = e.GetPosition(null);
        _draggingTab = tab;
        _isDragging = false;
    }

    private void Tab_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_draggingTab == null || e.LeftButton != MouseButtonState.Pressed) return;

        var pos = e.GetPosition(null);
        var diff = pos - _dragStartPoint;

        if (!_isDragging &&
            (Math.Abs(diff.X) > DragThreshold || Math.Abs(diff.Y) > DragThreshold))
        {
            _isDragging = true;
            BeginTabDrag(_draggingTab);
        }
    }

    private void Tab_MouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
    {
        _draggingTab = null;
        _isDragging = false;
    }

    private void BeginTabDrag(TabItem tab)
    {
        TabDragManager.DraggedTab = tab;
        TabDragManager.SourceTabControl = this;

        var overlay = GlobalDragAdornerWindow.Show(
            GetTabHeaderText(tab),
            NativeMethods.GetCursorPosition());

        void GiveFeedbackHandler(object? _, GiveFeedbackEventArgs fe)
        {
            fe.UseDefaultCursors = false;
            Mouse.SetCursor(Cursors.None);
            overlay.MoveTo(NativeMethods.GetCursorPosition());
            fe.Handled = true;
        }

        WeakEventManager<UIElement, GiveFeedbackEventArgs>.AddHandler(
            MainTabControl, nameof(GiveFeedback), GiveFeedbackHandler);

        try
        {
            var data = new DataObject("TearableTab", GetTabHeaderText(tab));
            DragDrop.DoDragDrop(MainTabControl, data, DragDropEffects.Move);
        }
        finally
        {
            WeakEventManager<UIElement, GiveFeedbackEventArgs>.RemoveHandler(
                MainTabControl, nameof(GiveFeedback), GiveFeedbackHandler);

            overlay.Close();
            Mouse.SetCursor(Cursors.Arrow);
            RemoveAdorner();

            if (TabDragManager.IsDragging)
            {
                TearOutToNewWindow(tab);
                TabDragManager.Clear();
            }

            _draggingTab = null;
            _isDragging = false;
        }
    }

    private void TearOutToNewWindow(TabItem tab)
    {
        RemoveTab(tab);

        var win = new FloatingTabWindow();
        win.Show();

        var pixelPos = NativeMethods.GetCursorPosition();
        var source = PresentationSource.FromVisual(win);
        var dip = source?.CompositionTarget != null
            ? source.CompositionTarget.TransformFromDevice.Transform(pixelPos)
            : pixelPos;

        win.Left = dip.X - 60;
        win.Top = dip.Y - 10;
        win.AddTab(tab);
        win.Activate();
        win.Focus();
    }

    private void TabControl_DragOver(object? sender, DragEventArgs e)
    {
        if (!TabDragManager.IsDragging) return;
        e.Effects = DragDropEffects.Move;
        e.Handled = true;
        ShowAdorner(TabDragManager.DraggedTab!, e.GetPosition(this));
    }

    private void TabControl_DragLeave(object? sender, DragEventArgs e) => RemoveAdorner();
    private void TabControl_ItemsChanged(object? sender, ItemsChangedEventArgs e)
    {
        if (sender is ItemContainerGenerator containerGenerator)
        {
            if (containerGenerator?.Items.Count == 0 && Window.GetWindow(this) is FloatingTabWindow fw)
                fw.Close();
        }
    }

    private void TabControl_Drop(object? sender, DragEventArgs e)
    {
        RemoveAdorner();
        if (!TabDragManager.IsDragging) return;
        if (TabDragManager.SourceTabControl == this) return;

        var tab = TabDragManager.DraggedTab!;
        var sourceCtrl = TabDragManager.SourceTabControl;

        sourceCtrl?.RemoveTab(tab);
        AddTab(tab);

        TabDragManager.Clear();
        e.Handled = true;

        if (sourceCtrl?.TabCount == 0 && sourceCtrl.GetOwnerWindow() is FloatingTabWindow fw)
            fw.Close();
    }

    private void ShowAdorner(TabItem tab, Point pos)
    {
        if (_adornerLayer == null)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (_adornerLayer == null) return;

            var ghost = new Border
            {
                Background = Brushes.LightYellow,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(6, 2, 6, 2),
                Child = new TextBlock { Text = GetTabHeaderText(tab) }
            };

            _adorner = new DragAdorner(this, ghost);
            _adornerLayer.Add(_adorner);
        }

        _adorner?.UpdatePosition(pos);
    }

    private void RemoveAdorner()
    {
        if (_adorner != null && _adornerLayer != null)
        {
            _adornerLayer.Remove(_adorner);
            _adorner = null;
            _adornerLayer = null;
        }
    }

    private void CloseOwnerFloatingWindowIfEmpty()
    {
        if (TabCount == 0 && GetOwnerWindow() is FloatingTabWindow floatingWindow)
            floatingWindow.Close();
    }

    // =========================================================================
    //  Utilities
    // =========================================================================

    private static bool IsHeaderButtonClick(DependencyObject? source)
        => FindAncestor<Button>(source) != null || FindAncestor<ToggleButton>(source) != null;

    private static string GetTabHeaderText(TabItem tab)
        => tab.Header?.ToString() ?? "Tab";

    private static TabItem? GetTabItemFromSource(DependencyObject? source)
        => FindAncestor<TabItem>(source);

    private static T? FindAncestor<T>(DependencyObject? current) where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T match) return match;
            current = VisualTreeHelper.GetParent(current) ?? LogicalTreeHelper.GetParent(current);
        }
        return null;
    }
}