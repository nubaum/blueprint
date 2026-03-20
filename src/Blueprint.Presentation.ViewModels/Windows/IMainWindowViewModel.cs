using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace Blueprint.Presentation.ViewModels.Windows;

public interface IMainWindowViewModel
{
    string ApplicationTitle { get; }

    ObservableCollection<NavigationViewItem> MenuItems { get; }

    ObservableCollection<NavigationViewItem> FooterMenuItems { get; }

    ObservableCollection<MenuItem> TrayMenuItems { get; }

    void Initialize(Type settingsPageType, Type dashboardPageType, Type dataPageType, Type codePageType);
}
