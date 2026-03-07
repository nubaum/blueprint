using System.IO;
using Microsoft.Web.WebView2.Core;

namespace Blueprint.Views.UserControls;

public partial class CodeEditor
{
    public CodeEditor()
    {
        InitializeComponent();
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        if (Browser.CoreWebView2 is null)
        {
            string userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Bllueprint", "WebView2UserData");

            if (!Directory.Exists(userDataFolder))
            {
                Directory.CreateDirectory(userDataFolder);
            }

            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder);
            await Browser.EnsureCoreWebView2Async(environment);
        }

        await Browser.EnsureCoreWebView2Async();

        string hostFolder = Path.Combine(
                AppContext.BaseDirectory,
                "monaco-host");

        Browser.CoreWebView2!.SetVirtualHostNameToFolderMapping(
                "monaco.local",
                hostFolder,
                CoreWebView2HostResourceAccessKind.Allow);

#pragma warning disable S1075 // URIs should not be hardcoded
        Browser.Source = new Uri("https://monaco.local/index.html");
#pragma warning restore S1075 // URIs should not be hardcoded

    }
}
