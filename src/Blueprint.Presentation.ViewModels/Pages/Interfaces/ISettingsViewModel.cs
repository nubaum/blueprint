using Blueprint.Application.Abstractions.Workspace;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ISettingsViewModel
{
    string AppVersion { get; }

    IReadThemeStore ThemeStore { get; }
}
