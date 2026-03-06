using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Blueprint.Views.UserControls.Primitives;

public class GridSplitterThumb : Thumb
{
    public ColumnDefinition? LeftColumn
    {
        get => (ColumnDefinition?)GetValue(LeftColumnProperty);
        set => SetValue(LeftColumnProperty, value);
    }

    public static readonly DependencyProperty LeftColumnProperty =
        DependencyProperty.Register(nameof(LeftColumn), typeof(ColumnDefinition), typeof(GridSplitterThumb));

    public ColumnDefinition? RightColumn
    {
        get => (ColumnDefinition?)GetValue(RightColumnProperty);
        set => SetValue(RightColumnProperty, value);
    }

    public static readonly DependencyProperty RightColumnProperty =
        DependencyProperty.Register(nameof(RightColumn), typeof(ColumnDefinition), typeof(GridSplitterThumb));

    private double _lastLeftPx = 260;

    public GridSplitterThumb()
    {
        DragStarted += OnDragStarted;
        DragDelta += OnDragDelta;
    }

    private void OnDragStarted(object sender, DragStartedEventArgs e)
    {
        if (LeftColumn?.ActualWidth > 1)
            _lastLeftPx = LeftColumn.ActualWidth;
    }

    private void OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        if (LeftColumn == null || RightColumn == null)
            return;

        var newLeft = LeftColumn.ActualWidth + e.HorizontalChange;

        var total = LeftColumn.ActualWidth + RightColumn.ActualWidth + ActualWidth;

        var minLeft = LeftColumn.MinWidth;
        var minRight = RightColumn.MinWidth;

        var maxLeft = Math.Max(minLeft, total - minRight - ActualWidth);

        newLeft = Math.Max(minLeft, Math.Min(maxLeft, newLeft));

        LeftColumn.Width = new GridLength(newLeft, GridUnitType.Pixel);
        RightColumn.Width = new GridLength(1, GridUnitType.Star);

        if (newLeft > 1)
            _lastLeftPx = newLeft;
    }
}