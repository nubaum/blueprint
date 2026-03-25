using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class SettingsViewModel(IReadThemeStore readThemeStore) : NotifyPropertyChangedBase, ISettingsViewModel
{
    private bool _isInitialized;

    public IReadThemeStore ThemeStore => readThemeStore;

    public string AppVersion { get; private set; } = string.Empty;

    public Task OnNavigatedToAsync()
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }

        return Task.CompletedTask;
    }

    private static string GetAssemblyVersion() =>
        typeof(SettingsViewModel).Assembly.GetName().Version?.ToString() ?? string.Empty;

    private void InitializeViewModel()
    {
        AppVersion = $"Bllueprint - {GetAssemblyVersion()}";
        OnPropertyChanged(nameof(AppVersion));
        _isInitialized = true;
    }
}
