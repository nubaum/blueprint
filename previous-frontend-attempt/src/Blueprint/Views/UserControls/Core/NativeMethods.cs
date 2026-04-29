using System.Runtime.InteropServices;

namespace Blueprint.Views.UserControls.Core;

/// <summary>
/// Small P/Invoke wrapper to get the real screen-space cursor position,
/// which is needed when spawning a FloatingTabWindow under the cursor
/// (WPF's Mouse.GetPosition only works relative to a visual element).
/// </summary>
internal static class NativeMethods
{
    public static System.Windows.Point GetCursorPosition()
    {
        GetCursorPos(out Point pt);
        return new System.Windows.Point(pt.X, pt.Y);
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out Point pt);

    [StructLayout(LayoutKind.Sequential)]
    private struct Point : IEquatable<Point>
    {
        public int X;
        public int Y;

        public readonly bool Equals(Point other)
            => other.X == X && other.Y == Y;
    }
}
