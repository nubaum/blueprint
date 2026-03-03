using Wpf.Ui.Appearance;

namespace Blueprint.Views.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            SystemThemeWatcher.Watch(this);
            InitializeComponent();
        }

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

    }
}
