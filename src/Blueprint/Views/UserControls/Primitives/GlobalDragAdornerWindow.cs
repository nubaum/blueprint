using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Blueprint.Views.UserControls.Primitives;

public class GlobalDragAdornerWindow : Window
{
    // ── Win32 ─────────────────────────────────────────────────
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TRANSPARENT = 0x00000020;
    private const int WS_EX_LAYERED = 0x00080000;
    private const int WS_EX_NOACTIVATE = 0x08000000;
    private const int WS_EX_TOOLWINDOW = 0x00000080;

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
    // ─────────────────────────────────────────────────────────

    private GlobalDragAdornerWindow(string header)
    {
        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
        Background = Brushes.Transparent;
        ShowInTaskbar = false;
        Topmost = true;
        SizeToContent = SizeToContent.WidthAndHeight;
        Focusable = false;

        Left = -9999;
        Top = -9999;

        Content = new Border
        {
            Background = Brushes.LightYellow,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(1),
            Padding = new Thickness(8, 3, 8, 3),
            Opacity = 0.85,
            Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                BlurRadius = 12,
                ShadowDepth = 4,
                Opacity = 0.6
            },
            Child = new TextBlock { Text = header, FontSize = 12 }
        };

        WeakEventManager<Window, EventArgs>.AddHandler(
            this,
            nameof(SourceInitialized),
            OnSourceInitialized);
    }

    private void OnSourceInitialized(object? sender, EventArgs e)
    {
        WeakEventManager<Window, EventArgs>.RemoveHandler(
            this,
            nameof(SourceInitialized),
            OnSourceInitialized);

        var hwnd = new WindowInteropHelper(this).Handle;
        var style = GetWindowLong(hwnd, GWL_EXSTYLE);

        // WS_EX_TRANSPARENT  → mouse / drag events pass through
        // WS_EX_LAYERED      → required for transparency + WS_EX_TRANSPARENT
        // WS_EX_NOACTIVATE   → never steals focus
        // WS_EX_TOOLWINDOW   → hidden from Alt+Tab
        SetWindowLong(hwnd, GWL_EXSTYLE,
            style | WS_EX_TRANSPARENT | WS_EX_LAYERED | WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);
    }

    public static GlobalDragAdornerWindow Show(string header, Point initialScreenPoint)
    {
        var win = new GlobalDragAdornerWindow(header);
        win.Show(); // HWND + PresentationSource exist after this
        win.MoveTo(initialScreenPoint);
        return win;
    }

    /// <summary>
    /// Moves the ghost to the given raw screen-pixel point.
    /// Converts to WPF device-independent units using the window's own DPI,
    /// so it stays correctly positioned on scaled displays.
    /// </summary>
    public void MoveTo(Point screenPixelPoint)
    {
        var source = PresentationSource.FromVisual(this);
        if (source?.CompositionTarget != null)
        {
            var m = source.CompositionTarget.TransformFromDevice;
            var dip = m.Transform(screenPixelPoint);
            Left = dip.X + 12;
            Top = dip.Y - 8;
        }
        else
        {
            // Fallback before the window is fully rendered
            Left = screenPixelPoint.X + 12;
            Top = screenPixelPoint.Y - 8;
        }
    }
}