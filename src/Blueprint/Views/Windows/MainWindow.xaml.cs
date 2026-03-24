using Blueprint.Presentation.ViewModels.Windows;
using Blueprint.Views.Pages;
using Wpf.Ui.Appearance;

namespace Blueprint.Views.Windows;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        if (DataContext is IMainWindowViewModel viewModel)
        {
            viewModel.Initialize(typeof(SettingsPage), typeof(DashboardPage), typeof(DataPage), typeof(CodePage));
        }
    }

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        System.Windows.Application.Current.Shutdown();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        SystemThemeWatcher.Watch(this);
    }
}
