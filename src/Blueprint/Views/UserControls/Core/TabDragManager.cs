using System.Windows.Controls;

namespace Blueprint.Views.UserControls.Core;

/// <summary>
/// Holds shared state for an ongoing tab drag operation.
/// Acts as the bridge between the source TabControl and any FloatingTabWindow.
/// </summary>
public static class TabDragManager
{
    /// <summary>The TabItem currently being dragged.</summary>
    public static TabItem? DraggedTab { get; set; }

    /// <summary>The TearableTabControl that originally owned the dragged tab.</summary>
    public static TearableTabControl? SourceTabControl { get; set; }

    /// <summary>Whether a cross-window drag is currently in progress.</summary>
    public static bool IsDragging => DraggedTab != null;

    public static void Clear()
    {
        DraggedTab = null;
        SourceTabControl = null;
    }
}
