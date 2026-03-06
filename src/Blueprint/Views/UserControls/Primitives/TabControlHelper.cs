using System.Windows.Controls;
using System.Windows.Input;

namespace Blueprint.Views.UserControls.Primitives;

public static class TabControlHelper
{
    public static readonly RoutedUICommand CloseTabCommand =
        new("CloseTab", "CloseTab", typeof(TabControlHelper));

    public static readonly RoutedUICommand PinTabCommand =
        new("PinTab", "PinTab", typeof(TabControlHelper));

    public static readonly RoutedUICommand UnpinTabCommand =
        new("UnpinTab", "UnpinTab", typeof(TabControlHelper));

    public static readonly DependencyProperty IsPinnedProperty =
        DependencyProperty.RegisterAttached(
            "IsPinned",
            typeof(bool),
            typeof(TabControlHelper),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty IsDirtyProperty =
        DependencyProperty.RegisterAttached(
            "IsDirty",
            typeof(bool),
            typeof(TabControlHelper),
            new PropertyMetadata(false));

    static TabControlHelper()
    {
        CommandManager.RegisterClassCommandBinding(
            typeof(TabControl),
            new CommandBinding(CloseTabCommand, ExecuteCloseTab, CanExecuteCloseTab));

        CommandManager.RegisterClassCommandBinding(
            typeof(TabControl),
            new CommandBinding(PinTabCommand, ExecutePinTab, CanExecutePinTab));

        CommandManager.RegisterClassCommandBinding(
            typeof(TabControl),
            new CommandBinding(UnpinTabCommand, ExecuteUnpinTab, CanExecuteUnpinTab));
    }

    public static void SetIsPinned(DependencyObject element, bool value)
        => element.SetValue(IsPinnedProperty, value);

    public static bool GetIsPinned(DependencyObject element)
        => (bool)element.GetValue(IsPinnedProperty);

    public static void SetIsDirty(DependencyObject element, bool value)
        => element.SetValue(IsDirtyProperty, value);

    public static bool GetIsDirty(DependencyObject element)
        => (bool)element.GetValue(IsDirtyProperty);

    private static void CanExecuteCloseTab(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = e.Parameter is TabItem;
        e.Handled = true;
    }

    private static void ExecuteCloseTab(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is not TabItem tab)
        {
            return;
        }

        if (tab.Parent is not TabControl tabControl)
        {
            return;
        }

        if (GetIsPinned(tab))
        {
            return;
        }

        tabControl.Items.Remove(tab);
        e.Handled = true;
    }

    private static void CanExecutePinTab(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = e.Parameter is TabItem tab && !GetIsPinned(tab);
        e.Handled = true;
    }

    private static void ExecutePinTab(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is not TabItem tab)
        {
            return;
        }

        SetIsPinned(tab, true);
        e.Handled = true;
    }

    private static void CanExecuteUnpinTab(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = e.Parameter is TabItem tab && GetIsPinned(tab);
        e.Handled = true;
    }

    private static void ExecuteUnpinTab(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Parameter is not TabItem tab)
        {
            return;
        }

        SetIsPinned(tab, false);
        e.Handled = true;
    }
}
