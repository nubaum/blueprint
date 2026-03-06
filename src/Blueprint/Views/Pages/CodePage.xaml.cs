using System.Windows.Controls;

namespace Blueprint.Views.Pages;

public partial class CodePage : Page
{
    public CodePage()
    {
        InitializeComponent();
        AddTab("Tab One", new TextBlock { Text = "Content of Tab One", Margin = new Thickness(10) });
        AddTab("Tab Two", new TextBlock { Text = "Content of Tab Two", Margin = new Thickness(10) });
        AddTab("Tab Three", new TextBlock { Text = "Content of Tab Three", Margin = new Thickness(10) });
    }

    private void AddTab(string header, UIElement content)
    {
        MainTabs.AddTab(new TabItem { Header = header, Content = content });
    }
}


