using Wpf.Ui.Controls;

namespace Blueprint.Presentation.ViewModels.Windows;

public interface IMainWindowViewModel
{
    string ApplicationTitle { get; }

    IReadOnlyCollection<NavigationViewItem> MenuItems { get; }

    IReadOnlyCollection<NavigationViewItem> FooterMenuItems { get; }

    IReadOnlyCollection<MenuItem> TrayMenuItems { get; }

    void Initialize(Type settingsPageType, Type dashboardPageType, Type dataPageType, Type codePageType);
}
