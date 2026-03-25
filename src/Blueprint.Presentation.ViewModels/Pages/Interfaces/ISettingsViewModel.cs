using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ISettingsViewModel
{
    string AppVersion { get; }

    IReadThemeStore ThemeStore { get; }
}
