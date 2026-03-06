using System.Runtime.InteropServices;

namespace Blueprint.Views.UserControls.Primitives;

/// <summary>
/// Small P/Invoke wrapper to get the real screen-space cursor position,
/// which is needed when spawning a FloatingTabWindow under the cursor
/// (WPF's Mouse.GetPosition only works relative to a visual element).
/// </summary>
internal static class NativeMethods
{
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT { public int X; public int Y; }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT pt);

    public static Point GetCursorPosition()
    {
        GetCursorPos(out var pt);
        return new Point(pt.X, pt.Y);
    }
}
