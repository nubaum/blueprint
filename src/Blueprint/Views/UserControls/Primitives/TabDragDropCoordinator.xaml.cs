using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Blueprint.Views.UserControls.Primitives;

internal sealed class TabDragDropCoordinator
{
    private const double DragThreshold = 6.0;

    private readonly TearableTabControl _owner;
    private readonly TabControl _tabControl;
    private readonly TabDropAdornerService _adornerService;
    private readonly FloatingTabWindowService _floatingWindowService;

    private Point _dragStartPoint;
    private bool _isDragging;
    private TabItem? _draggingTab;

    public TabDragDropCoordinator(TearableTabControl owner, TabControl tabControl)
    {
        _owner = owner;
        _tabControl = tabControl;
        _adornerService = new TabDropAdornerService(owner);
        _floatingWindowService = new FloatingTabWindowService(owner);
    }

    public void Attach()
    {
        WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(
            _tabControl,
            nameof(UIElement.PreviewMouseLeftButtonDown),
            OnMouseLeftButtonDown);

        WeakEventManager<UIElement, MouseEventArgs>.AddHandler(
            _tabControl,
            nameof(UIElement.PreviewMouseMove),
            OnMouseMove);

        WeakEventManager<UIElement, MouseButtonEventArgs>.AddHandler(
            _tabControl,
            nameof(UIElement.PreviewMouseLeftButtonUp),
            OnMouseLeftButtonUp);

        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            _tabControl,
            nameof(UIElement.Drop),
            OnDrop);

        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            _tabControl,
            nameof(UIElement.DragOver),
            OnDragOver);

        WeakEventManager<UIElement, DragEventArgs>.AddHandler(
            _tabControl,
            nameof(UIElement.DragLeave),
            OnDragLeave);

        WeakEventManager<ItemContainerGenerator, ItemsChangedEventArgs>.AddHandler(
            _tabControl.ItemContainerGenerator,
            nameof(ItemContainerGenerator.ItemsChanged),
            OnItemsChanged);
    }

    private void OnMouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
    {
        var originalSource = e.OriginalSource as DependencyObject;

        if (TabDragVisualTreeHelper.IsHeaderButtonClick(originalSource))
        {
            return;
        }

        TabItem? tab = TabDragVisualTreeHelper.GetTabItemFromSource(originalSource);
        if (tab == null)
        {
            return;
        }

        _dragStartPoint = e.GetPosition(null);
        _draggingTab = tab;
        _isDragging = false;
    }

    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        if (_draggingTab == null || e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        Point currentPosition = e.GetPosition(null);
        Vector difference = currentPosition - _dragStartPoint;

        if (!_isDragging &&
            (Math.Abs(difference.X) > DragThreshold || Math.Abs(difference.Y) > DragThreshold))
        {
            _isDragging = true;
            BeginDrag(_draggingTab);
        }
    }

    private void OnMouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
    {
        _draggingTab = null;
        _isDragging = false;
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (!TabDragManager.IsDragging)
        {
            return;
        }

        e.Effects = DragDropEffects.Move;
        e.Handled = true;

        _adornerService.Show(TabDragManager.DraggedTab!, e.GetPosition(_owner));
    }

    private void OnDragLeave(object? sender, DragEventArgs e)
    {
        _adornerService.Remove();
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        _adornerService.Remove();

        if (!TabDragManager.IsDragging)
        {
            return;
        }

        if (TabDragManager.SourceTabControl == _owner)
        {
            return;
        }

        TabItem draggedTab = TabDragManager.DraggedTab!;
        TearableTabControl? sourceControl = TabDragManager.SourceTabControl;

        sourceControl?.RemoveTab(draggedTab);
        _owner.AddTab(draggedTab);

        TabDragManager.Clear();
        e.Handled = true;

        FloatingTabWindowService.CloseIfEmpty(sourceControl);
    }

    private void OnItemsChanged(object? sender, ItemsChangedEventArgs e)
    {
        _floatingWindowService.CloseOwnerWindowIfEmpty(sender);
    }

    private void BeginDrag(TabItem tab)
    {
        TabDragManager.DraggedTab = tab;
        TabDragManager.SourceTabControl = _owner;

        string headerText = TabDragVisualTreeHelper.GetTabHeaderText(tab);

        var overlay = GlobalDragAdornerWindow.Show(
            headerText,
            NativeMethods.GetCursorPosition());

        void GiveFeedbackHandler(object? sender, GiveFeedbackEventArgs args)
        {
            args.UseDefaultCursors = false;
            Mouse.SetCursor(Cursors.None);
            overlay.MoveTo(NativeMethods.GetCursorPosition());
            args.Handled = true;
        }

        WeakEventManager<UIElement, GiveFeedbackEventArgs>.AddHandler(
            _tabControl,
            nameof(UIElement.GiveFeedback),
            GiveFeedbackHandler);

        try
        {
            var data = new DataObject("TearableTab", headerText);
            DragDrop.DoDragDrop(_tabControl, data, DragDropEffects.Move);
        }
        finally
        {
            WeakEventManager<UIElement, GiveFeedbackEventArgs>.RemoveHandler(
                _tabControl,
                nameof(UIElement.GiveFeedback),
                GiveFeedbackHandler);

            overlay.Close();
            Mouse.SetCursor(Cursors.Arrow);
            _adornerService.Remove();

            if (TabDragManager.IsDragging)
            {
                _floatingWindowService.TearOutToNewWindow(tab);
                TabDragManager.Clear();
            }

            _draggingTab = null;
            _isDragging = false;
        }
    }
}
