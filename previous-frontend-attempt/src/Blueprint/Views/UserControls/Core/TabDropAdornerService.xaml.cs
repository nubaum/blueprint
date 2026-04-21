using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Blueprint.Views.UserControls.Core;

internal sealed class TabDropAdornerService
{
    private readonly UIElement _adornedElement;

    private DragAdorner? _adorner;
    private AdornerLayer? _adornerLayer;

    public TabDropAdornerService(UIElement adornedElement)
    {
        _adornedElement = adornedElement;
    }

    public void Show(TabItem tab, Point position)
    {
        if (_adornerLayer == null)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(_adornedElement);
            if (_adornerLayer == null)
            {
                return;
            }

            var ghost = new Border
            {
                Background = Brushes.LightYellow,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(6, 2, 6, 2),
                Child = new TextBlock
                {
                    Text = TabDragVisualTreeHelper.GetTabHeaderText(tab)
                }
            };

            _adorner = new DragAdorner(_adornedElement, ghost);
            _adornerLayer.Add(_adorner);
        }

        _adorner?.UpdatePosition(position);
    }

    public void Remove()
    {
        if (_adorner == null || _adornerLayer == null)
        {
            return;
        }

        _adornerLayer.Remove(_adorner);
        _adorner = null;
        _adornerLayer = null;
    }
}
