using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Blueprint.Views.UserControls.Primitives;

internal static class TabDragVisualTreeHelper
{
    public static bool IsHeaderButtonClick(DependencyObject? source)
        => FindAncestor<Button>(source) != null || FindAncestor<ToggleButton>(source) != null;

    public static string GetTabHeaderText(TabItem tab)
        => tab.Header?.ToString() ?? "Tab";

    public static TabItem? GetTabItemFromSource(DependencyObject? source)
        => FindAncestor<TabItem>(source);

    private static T? FindAncestor<T>(DependencyObject? current)
        where T : DependencyObject
    {
        while (current != null)
        {
            if (current is T match)
            {
                return match;
            }

            current = VisualTreeHelper.GetParent(current) ?? LogicalTreeHelper.GetParent(current);
        }

        return null;
    }
}
