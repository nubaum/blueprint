using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Blueprint.Views.UserControls.Primitives;

public partial class TearableTabControl : UserControl
{
    private Point _dragStartPoint;
    private bool _isDragging;
    private TabItem? _draggingTab;
    private DragAdorner? _adorner;
    private AdornerLayer? _adornerLayer;

    private const double DragThreshold = 6.0;

    public TearableTabControl()
    {
        InitializeComponent();

        WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(
            MainTabControl,
            nameof(UIElement.PreviewMouseLeftButtonDown),
            Tab_MouseLeftButtonDown);

        WeakEventManager<UIElement, MouseEventArgs>.AddHandler(
            MainTabControl,
            nameof(UIElement.PreviewMouseMove),
            Tab_MouseMove);

        WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(
            MainTabControl,
            nameof(UIElement.PreviewMouseLeftButtonUp),
            Tab_MouseLeftButtonUp);

        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            MainTabControl,
            nameof(UIElement.Drop),
            TabControl_Drop);

        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            MainTabControl,
            nameof(UIElement.DragOver),
            TabControl_DragOver);

        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            MainTabControl,
            nameof(UIElement.DragLeave),
            TabControl_DragLeave);

        WeakEventManager<ItemContainerGenerator, ItemsChangedEventArgs>.AddHandler(
            MainTabControl.ItemContainerGenerator,
            nameof(ItemContainerGenerator.ItemsChanged),
            TabControl_ItemsChanged);
    }

    public int TabCount => MainTabControl.Items.Count;

    public void AddTab(TabItem tab)
    {
        if (tab.Parent is TabControl oldOwner)
            oldOwner.Items.Remove(tab);

        MainTabControl.Items.Add(tab);
        MainTabControl.SelectedItem = tab;
        tab.Focus();
    }

    public void RemoveTab(TabItem tab)
    {
        MainTabControl.Items.Remove(tab);
        CloseOwnerFloatingWindowIfEmpty();
    }

    public Window? GetOwnerWindow() => Window.GetWindow(this);

    private void Tab_MouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
    {
        var originalSource = e.OriginalSource as DependencyObject;

        if (IsHeaderButtonClick(originalSource))
            return;

        var tab = GetTabItemFromSource(originalSource);
        if (tab == null)
            return;

        _dragStartPoint = e.GetPosition(null);
        _draggingTab = tab;
        _isDragging = false;
    }

    private void Tab_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_draggingTab == null || e.LeftButton != MouseButtonState.Pressed)
            return;

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
            MainTabControl,
            nameof(GiveFeedback),
            GiveFeedbackHandler);

        try
        {
            var data = new DataObject("TearableTab", GetTabHeaderText(tab));
            DragDrop.DoDragDrop(MainTabControl, data, DragDropEffects.Move);
        }
        finally
        {
            WeakEventManager<UIElement, GiveFeedbackEventArgs>.RemoveHandler(
                MainTabControl,
                nameof(GiveFeedback),
                GiveFeedbackHandler);

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
        if (!TabDragManager.IsDragging)
            return;

        e.Effects = DragDropEffects.Move;
        e.Handled = true;

        ShowAdorner(TabDragManager.DraggedTab!, e.GetPosition(this));
    }

    private void TabControl_DragLeave(object? sender, DragEventArgs e)
    {
        RemoveAdorner();
    }

    private void TabControl_ItemsChanged(object? sender, ItemsChangedEventArgs e)
    {
        var source = TabDragManager.SourceTabControl;

        if (sender is ItemContainerGenerator containerGenerator)
        {
            if (containerGenerator?.Items.Count == 0 && Window.GetWindow(this) is FloatingTabWindow fw)
                fw.Close();
        }
    }

    private void TabControl_Drop(object? sender, DragEventArgs e)
    {
        RemoveAdorner();

        if (!TabDragManager.IsDragging)
            return;

        if (TabDragManager.SourceTabControl == this)
            return;

        var tab = TabDragManager.DraggedTab!;
        var source = TabDragManager.SourceTabControl;

        source?.RemoveTab(tab);
        AddTab(tab);

        TabDragManager.Clear();
        e.Handled = true;

        if (source?.TabCount == 0 && source.GetOwnerWindow() is FloatingTabWindow fw)
            fw.Close();
    }

    private void ShowAdorner(TabItem tab, Point pos)
    {
        if (_adornerLayer == null)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (_adornerLayer == null)
                return;

            var ghost = new Border
            {
                Background = Brushes.LightYellow,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(6, 2, 6, 2),
                Child = new TextBlock
                {
                    Text = GetTabHeaderText(tab)
                }
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

    private void TabCloseButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not DependencyObject dependencyObject)
            return;

        var tab = FindAncestor<TabItem>(dependencyObject);
        if (tab == null)
            return;

        RemoveTab(tab);
        e.Handled = true;
    }

    private static bool IsHeaderButtonClick(DependencyObject? source)
    {
        return FindAncestor<Button>(source) != null
            || FindAncestor<ToggleButton>(source) != null;
    }

    private static string GetTabHeaderText(TabItem tab)
    {
        return tab.Header?.ToString() ?? "Tab";
    }

    private static TabItem? GetTabItemFromSource(DependencyObject? source)
    {
        return FindAncestor<TabItem>(source);
    }

    private static T? FindAncestor<T>(DependencyObject? current)
        where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T match)
                return match;

            current = VisualTreeHelper.GetParent(current)
                   ?? LogicalTreeHelper.GetParent(current);
        }

        return null;
    }
}
