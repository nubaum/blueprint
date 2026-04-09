using System.Windows.Input;

namespace Blueprint.Application.Abstractions.Workspace;

public interface IReadThemeStore
{
    BlueprintTheme CurrentTheme { get; }

    ICommand ChangeThemeCommand { get; }
}
