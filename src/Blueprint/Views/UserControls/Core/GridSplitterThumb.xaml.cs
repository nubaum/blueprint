using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Blueprint.Views.UserControls.Core;

public class GridSplitterThumb : Thumb
{
    public static readonly DependencyProperty LeftColumnProperty =
        DependencyProperty.Register(nameof(LeftColumn), typeof(ColumnDefinition), typeof(GridSplitterThumb));

    public static readonly DependencyProperty RightColumnProperty =
        DependencyProperty.Register(nameof(RightColumn), typeof(ColumnDefinition), typeof(GridSplitterThumb));

    public ColumnDefinition? LeftColumn
    {
        get => (ColumnDefinition?)GetValue(LeftColumnProperty);
        set => SetValue(LeftColumnProperty, value);
    }

    public ColumnDefinition? RightColumn
    {
        get => (ColumnDefinition?)GetValue(RightColumnProperty);
        set => SetValue(RightColumnProperty, value);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        WeakEventManager<Thumb, DragDeltaEventArgs>.AddHandler(
            this,
            nameof(DragDelta),
            OnDragDelta);
    }

    private void OnDragDelta(object? sender, DragDeltaEventArgs e)
    {
        if (LeftColumn == null || RightColumn == null)
        {
            return;
        }

        double newLeft = LeftColumn.ActualWidth + e.HorizontalChange;

        double total = LeftColumn.ActualWidth + RightColumn.ActualWidth + ActualWidth;

        double minLeft = LeftColumn.MinWidth;
        double minRight = RightColumn.MinWidth;

        double maxLeft = Math.Max(minLeft, total - minRight - ActualWidth);

        newLeft = Math.Max(minLeft, Math.Min(maxLeft, newLeft));

        LeftColumn.Width = new GridLength(newLeft, GridUnitType.Pixel);
        RightColumn.Width = new GridLength(1, GridUnitType.Star);
    }
}
