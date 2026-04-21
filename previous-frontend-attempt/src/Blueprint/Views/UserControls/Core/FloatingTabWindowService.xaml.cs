using System.Windows.Controls;

namespace Blueprint.Views.UserControls.Core;

internal sealed class FloatingTabWindowService
{
    private readonly TearableTabControl _owner;

    public FloatingTabWindowService(TearableTabControl owner)
    {
        _owner = owner;
    }

    public static void CloseIfEmpty(TearableTabControl? sourceControl)
    {
        if (sourceControl?.TabCount == 0 &&
            sourceControl.GetOwnerWindow() is FloatingTabWindow floatingWindow)
        {
            floatingWindow.Close();
        }
    }

    public void CloseOwnerWindowIfEmpty(object? sender)
    {
        if (sender is ItemContainerGenerator generator &&
            generator.Items.Count == 0 &&
            Window.GetWindow(_owner) is FloatingTabWindow floatingWindow)
        {
            floatingWindow.Close();
        }
    }

    public void TearOutToNewWindow(TabItem tab)
    {
        _owner.RemoveTab(tab);

        FloatingTabWindow window = new();
        window.Show();

        Point pixelPosition = NativeMethods.GetCursorPosition();
        var source = PresentationSource.FromVisual(window);

        Point dipPosition = source?.CompositionTarget != null
            ? source.CompositionTarget.TransformFromDevice.Transform(pixelPosition)
            : pixelPosition;

        window.Left = dipPosition.X - 60;
        window.Top = dipPosition.Y - 10;
        window.AddTab(tab);
        window.Activate();
        window.Focus();
    }
}
