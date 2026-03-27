using System.Windows.Input;
using ActiproSoftware.Windows.Themes;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.ViewModels.Core;
using Wpf.Ui.Appearance;

namespace Blueprint.Stores;

public class ThemeStore : IWriteThemeStore
{
    public ThemeStore()
    {
        ChangeThemeCommand = new DelegateCommand<BlueprintTheme>(SetTheme!);
    }

    public ICommand ChangeThemeCommand { get; }

    public BlueprintTheme CurrentTheme { get; private set; }

    private void SetTheme(BlueprintTheme parameter)
    {
        if (parameter == BlueprintTheme.Light)
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Light);
            CurrentTheme = BlueprintTheme.Light;
            ThemeManager.CurrentTheme = ThemeNames.Light;
        }
        else
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Dark);
            CurrentTheme = BlueprintTheme.Dark;
            ThemeManager.CurrentTheme = ThemeNames.Dark;
        }
    }
}
