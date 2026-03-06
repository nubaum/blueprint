using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Blueprint.Views.UserControls;

namespace TearableTab
{
    public partial class TearableTabControl : UserControl
    {
        // ── drag state ────────────────────────────────────────────
        private Point _dragStartPoint;
        private bool _isDragging;
        private TabItem? _draggingTab;
        private DragAdorner? _adorner;
        private AdornerLayer? _adornerLayer;

        // Minimum distance before we treat a mouse move as a drag
        private const double DragThreshold = 6.0;

        public TearableTabControl()
        {
            InitializeComponent();

            // Use WeakEventManager instead of direct subscriptions
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

            // Drop: accept tabs from other windows / tab-controls
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
        }

        // ── public helpers ────────────────────────────────────────

        public int TabCount => MainTabControl.Items.Count;

        public void AddTab(TabItem tab)
        {
            // Re-parent correctly (WPF requires the item not to have a logical parent)
            if (tab.Parent is TabControl oldOwner)
                oldOwner.Items.Remove(tab);

            MainTabControl.Items.Add(tab);
            MainTabControl.SelectedItem = tab;
            tab.Focus();
        }

        public void RemoveTab(TabItem tab)
        {
            MainTabControl.Items.Remove(tab);
            MainTabControl.Items.Remove(tab);
            CloseOwnerFloatingWindowIfEmpty();
        }

        /// <summary>Returns the Window that hosts this control (could be FloatingTabWindow).</summary>
        public Window? GetOwnerWindow() => Window.GetWindow(this);

        // ── mouse events on the tab headers ──────────────────────

        private void Tab_MouseLeftButtonDown(object? sender, MouseButtonEventArgs e)
        {
            var tab = GetTabItemFromPoint(e.GetPosition(MainTabControl));
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
                BeginTabDrag(_draggingTab, e);
            }
        }

        private void Tab_MouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
        {
            _draggingTab = null;
            _isDragging = false;
        }

        // ── drag initiation ───────────────────────────────────────

        private void BeginTabDrag(TabItem tab, MouseEventArgs e)
        {
            // Register in the shared manager so other windows can see it
            TabDragManager.DraggedTab = tab;
            TabDragManager.SourceTabControl = this;

            // Fullscreen transparent overlay — renders the ghost anywhere on screen,
            // including outside the source window.
            var overlay = GlobalDragAdornerWindow.Show(
                tab.Header?.ToString() ?? "Tab",
                NativeMethods.GetCursorPosition());

            // GiveFeedback fires continuously during DoDragDrop.
            // We hide the default cursor and reposition the overlay ghost instead.
            void GiveFeedbackHandler(object? _, GiveFeedbackEventArgs fe)
            {
                fe.UseDefaultCursors = false;
                Mouse.SetCursor(Cursors.None);
                overlay.MoveTo(NativeMethods.GetCursorPosition());
                fe.Handled = true;
            }

            WeakEventManager<UIElement, GiveFeedbackEventArgs>.AddHandler(
                MainTabControl,
                nameof(UIElement.GiveFeedback),
                GiveFeedbackHandler);

            try
            {
                var data = new DataObject("TearableTab", tab.Header?.ToString() ?? "");
                DragDrop.DoDragDrop(MainTabControl, data, DragDropEffects.Move);
            }
            finally
            {
                // Cleanup after drop or cancel
                WeakEventManager<UIElement, GiveFeedbackEventArgs>.RemoveHandler(
                    MainTabControl,
                    nameof(UIElement.GiveFeedback),
                    GiveFeedbackHandler);

                overlay.Close();
                Mouse.SetCursor(Cursors.Arrow);

                RemoveAdorner();

                // If the drag wasn't handled by any drop target, tear out into a new window
                if (TabDragManager.IsDragging)
                {
                    TearOutToNewWindow(tab);
                    TabDragManager.Clear();
                }

                _draggingTab = null;
                _isDragging = false;
            }
        }

        // ── tear-out ──────────────────────────────────────────────

        private void TearOutToNewWindow(TabItem tab)
        {
            if (TabCount <= 1)
            {
                // Don't tear if it's the only tab — optionally you can allow it
                // Comment out this guard to allow tearing the last tab too.
                // return;
            }

            RemoveTab(tab);

            var win = new FloatingTabWindow();
            win.Show(); // HWND must exist before PresentationSource is available

            // Convert raw screen pixels -> DIPs using the new window's own DPI context
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

        // ── drop-target: accept tabs dragged from another window ──

        private void TabControl_DragOver(object? sender, DragEventArgs e)
        {
            if (!TabDragManager.IsDragging) return;

            e.Effects = DragDropEffects.Move;
            e.Handled = true;

            ShowAdorner(TabDragManager.DraggedTab!, e.GetPosition(this));
        }

        private void TabControl_DragLeave(object? sender, DragEventArgs e)
        {
            RemoveAdorner();
        }

        private void TabControl_Drop(object? sender, DragEventArgs e)
        {
            RemoveAdorner();

            if (!TabDragManager.IsDragging) return;
            // Don't drop onto the same control
            if (TabDragManager.SourceTabControl == this) return;

            var tab = TabDragManager.DraggedTab!;
            var source = TabDragManager.SourceTabControl;

            source?.RemoveTab(tab);
            AddTab(tab);

            TabDragManager.Clear();
            e.Handled = true;

            // If source floating window is now empty, close it
            if (source?.TabCount == 0 && source.GetOwnerWindow() is FloatingTabWindow fw)
                fw.Close();
        }

        // ── adorner helpers ───────────────────────────────────────

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
                    Child = new TextBlock { Text = tab.Header?.ToString() ?? "Tab" }
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

        // ── hit-test helper ───────────────────────────────────────

        private static TabItem? GetTabItemFromPoint(Point point)
        {
            var element = Mouse.DirectlyOver as DependencyObject;
            while (element != null)
            {
                if (element is TabItem ti) return ti;
                element = VisualTreeHelper.GetParent(element)
                       ?? LogicalTreeHelper.GetParent(element);
            }
            return null;
        }
    }
}