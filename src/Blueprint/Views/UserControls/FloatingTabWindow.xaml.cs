using System.Windows.Controls;
using System.Windows.Documents;
using Blueprint.Views.UserControls;

namespace TearableTab
{
    public partial class FloatingTabWindow
    {
        private DragAdorner? _adorner;
        private AdornerLayer? _adornerLayer;

        public FloatingTabWindow()
        {
            InitializeComponent();

            WeakEventManager<UIElement, DragEventArgs>.AddHandler(
                this,
                nameof(Drop),
                FloatingTabWindow_Drop);

            WeakEventManager<UIElement, DragEventArgs>.AddHandler(
                this,
                nameof(DragOver),
                FloatingTabWindow_DragOver);

            WeakEventManager<UIElement, DragEventArgs>.AddHandler(
                this,
                nameof(DragLeave),
                FloatingTabWindow_DragLeave);

            WeakEventManager<Window, EventArgs>.AddHandler(
                this,
                nameof(Closed),
                FloatingTabWindow_Closed);
        }

        public void AddTab(TabItem tab)
        {
            FloatingTabCtrl.AddTab(tab);
            Title = tab.Header?.ToString() ?? "Floating Tab";
        }

        private void FloatingTabWindow_DragOver(object? sender, DragEventArgs e)
        {
            if (!TabDragManager.IsDragging) return;

            e.Effects = DragDropEffects.Move;
            e.Handled = true;

            ShowAdorner(e.GetPosition(this));
        }

        private void FloatingTabWindow_DragLeave(object? sender, DragEventArgs e)
        {
            RemoveAdorner();
        }

        private void FloatingTabWindow_Drop(object? sender, DragEventArgs e)
        {
            RemoveAdorner();

            if (!TabDragManager.IsDragging) return;

            var tab = TabDragManager.DraggedTab!;
            var source = TabDragManager.SourceTabControl;

            source?.RemoveTab(tab);

            FloatingTabCtrl.AddTab(tab);
            Title = tab.Header?.ToString() ?? "Floating Tab";

            TabDragManager.Clear();
            e.Handled = true;

            if (source?.TabCount == 0 && source.GetOwnerWindow() is FloatingTabWindow fw && fw != this)
                fw.Close();
        }

        private void FloatingTabWindow_Closed(object? sender, System.EventArgs e)
        {
            RemoveAdorner();

            WeakEventManager<UIElement, DragEventArgs>.RemoveHandler(this, nameof(UIElement.Drop), FloatingTabWindow_Drop);
            WeakEventManager<UIElement, DragEventArgs>.RemoveHandler(this, nameof(UIElement.DragOver), FloatingTabWindow_DragOver);
            WeakEventManager<UIElement, DragEventArgs>.RemoveHandler(this, nameof(UIElement.DragLeave), FloatingTabWindow_DragLeave);
            WeakEventManager<Window, EventArgs>.RemoveHandler(this, nameof(Window.Closed), FloatingTabWindow_Closed);
        }

        private void ShowAdorner(Point pos)
        {
            if (_adornerLayer == null)
            {
                if (Content is not UIElement contentElement) return;

                _adornerLayer = AdornerLayer.GetAdornerLayer(contentElement);
                if (_adornerLayer == null) return;

                var ghost = new TextBlock
                {
                    Text = TabDragManager.DraggedTab?.Header?.ToString() ?? "Tab",
                    Background = System.Windows.Media.Brushes.LightYellow,
                    Padding = new Thickness(6, 2, 6, 2)
                };

                _adorner = new DragAdorner(contentElement, ghost);
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
    }
}