using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace Blueprint.Views.Pages;

public partial class CodePage : Page
{
    private Point? _dragStart;
    private TabItem? _pressedTab;
    private double _lastLeftPx = 260;

    private AdornerLayer? _adornerLayer;
    public CodePage()
    {
        InitializeComponent();
        AddTab("Tab One", new TextBlock { Text = "Content of Tab One", Margin = new Thickness(10) });
        AddTab("Tab Two", new TextBlock { Text = "Content of Tab Two", Margin = new Thickness(10) });
        AddTab("Tab Three", new TextBlock { Text = "Content of Tab Three", Margin = new Thickness(10) });
    }

    private void SplitThumb_OnDragStarted(object sender, DragStartedEventArgs e)
    {
        if (LeftColumn.ActualWidth > 1)
            _lastLeftPx = LeftColumn.ActualWidth;
    }

    private void SplitThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
    {
        // Moving mouse right -> positive HorizontalChange -> left column grows
        var newLeft = LeftColumn.ActualWidth + e.HorizontalChange;

        // clamp between MinWidth and (total - rightMin - splitter)
        var total = ActualWidth;
        var splitterWidth = 14.0;

        var minLeft = LeftColumn.MinWidth;
        var minRight = RightColumn.MinWidth;

        // total might be 0 early in layout; handle safely
        if (total <= 1)
            total = (LeftColumn.ActualWidth + RightColumn.ActualWidth + splitterWidth);

        var maxLeft = Math.Max(minLeft, total - minRight - splitterWidth - 24 /*padding-ish*/);

        newLeft = Math.Max(minLeft, Math.Min(maxLeft, newLeft));

        LeftColumn.Width = new GridLength(newLeft, GridUnitType.Pixel);
        RightColumn.Width = new GridLength(1, GridUnitType.Star);

        if (newLeft > 1)
            _lastLeftPx = newLeft;
    }

    private void RightTabs_OnPreviewDragOver(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(typeof(TabItem)))
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }

        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }


    private void SplitThumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
    {
        // You could snap-to if you want (example):
        // if (LeftColumn.ActualWidth < 180) LeftColumn.Width = new GridLength(160);
    }
    private void AddTab(string header, UIElement content)
    {
        MainTabs.AddTab(new TabItem { Header = header, Content = content });
    }
}


