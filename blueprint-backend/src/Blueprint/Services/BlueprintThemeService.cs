using ActiproSoftware.Windows.Themes;
using Blueprint.Application.Abstractions;
using Wpf.Ui.Appearance;

namespace Blueprint.Services;

internal class BlueprintThemeService : IBlueprintThemeService
{
    public void SetDarkTheme()
    {
        ApplicationThemeManager.Apply(ApplicationTheme.Dark);
        ThemeManager.CurrentTheme = ThemeNames.Dark;
    }

    public void SetLightTheme()
    {
        ApplicationThemeManager.Apply(ApplicationTheme.Light);
        ThemeManager.CurrentTheme = ThemeNames.Light;
    }
}
