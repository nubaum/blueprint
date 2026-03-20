using System.Windows.Input;
using Blueprint.Presentation.ViewModels.Core;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;

namespace Blueprint.Presentation.ViewModels.Pages;

public class SettingsViewModel : NotifyPropertyChangedBase, INavigationAware
{
    private bool _isInitialized;

    public SettingsViewModel()
    {
        ChangeThemeCommand = new DelegateCommand<string>(OnChangeTheme!);
    }

    public ApplicationTheme CurrentTheme
    {
        get;
        set => SetField(ref field, value);
    }

    public string AppVersion { get; private set; } = string.Empty;

    public ICommand ChangeThemeCommand { get; }

    public Task OnNavigatedToAsync()
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }

        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync() => Task.CompletedTask;

    private static string GetAssemblyVersion() =>
        typeof(SettingsViewModel).Assembly.GetName().Version?.ToString() ?? string.Empty;

    private void InitializeViewModel()
    {
        CurrentTheme = ApplicationThemeManager.GetAppTheme();
        AppVersion = $"Bllueprint - {GetAssemblyVersion()}";
        OnPropertyChanged(nameof(AppVersion));
        _isInitialized = true;
    }

    private void OnChangeTheme(string parameter)
    {
        if (parameter == "theme_light")
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Light);
            CurrentTheme = ApplicationTheme.Light;
        }
        else
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Dark);
            CurrentTheme = ApplicationTheme.Dark;
        }
    }
}
