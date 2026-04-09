using System.Windows.Input;

namespace Blueprint.Abstractions.Application.Workspace;

public interface IReadThemeStore
{
    BlueprintTheme CurrentTheme { get; }

    ICommand ChangeThemeCommand { get; }
}
