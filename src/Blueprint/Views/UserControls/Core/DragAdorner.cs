using System.Windows.Documents;
using System.Windows.Media;

namespace Blueprint.Views.UserControls.Core;

/// <summary>
/// An adorner that renders a semi-transparent "ghost" of the dragged tab header
/// with a drop-shadow effect, following the mouse cursor.
/// </summary>
public class DragAdorner : Adorner
{
    private readonly UIElement _child;
    private double _left;
    private double _top;

    public DragAdorner(UIElement adornedElement, UIElement child)
        : base(adornedElement)
    {
        _child = child;
        IsHitTestVisible = false;

        // Semi-transparent ghost
        Opacity = 0.6;

        // Drop shadow
        Effect = new System.Windows.Media.Effects.DropShadowEffect
        {
            Color = Colors.Black,
            BlurRadius = 10,
            ShadowDepth = 4,
            Opacity = 0.7
        };
    }

    protected override int VisualChildrenCount => 1;

    public void UpdatePosition(Point position)
    {
        _left = position.X + 10;
        _top = position.Y + 10;
        InvalidateArrange();
    }

    public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
    {
        var result = new GeneralTransformGroup();
        result.Children.Add(base.GetDesiredTransform(transform));
        result.Children.Add(new TranslateTransform(_left, _top));
        return result;
    }

    protected override Visual GetVisualChild(int index) => _child;

    protected override Size MeasureOverride(Size constraint)
    {
        _child.Measure(constraint);
        return _child.DesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        _child.Arrange(new Rect(_child.DesiredSize));
        return finalSize;
    }
}
