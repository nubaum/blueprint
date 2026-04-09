using System.Windows.Input;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.Abstractions;
using Blueprint.Application.Core;
using Blueprint.Application.InternalAbstractions;

namespace Blueprint.Stores;

internal class ThemeStore : IWriteThemeStore
{
    private readonly IBlueprintThemeService _themeService;

    public ThemeStore(IBlueprintThemeService themeService)
    {
        ChangeThemeCommand = new DelegateCommand<BlueprintTheme>(SetTheme!);
        _themeService = themeService;
    }

    public ICommand ChangeThemeCommand { get; }

    public BlueprintTheme CurrentTheme { get; private set; }

    public void SetTheme(BlueprintTheme parameter)
    {
        if (parameter == BlueprintTheme.Light)
        {
            _themeService.SetLightTheme();
            CurrentTheme = BlueprintTheme.Light;
        }
        else
        {
            _themeService.SetDarkTheme();
            CurrentTheme = BlueprintTheme.Dark;
        }
    }
}
