using System.Windows.Input;
using Blueprint.ViewModels.Primitives;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;

namespace Blueprint.ViewModels.Pages;

public class SettingsViewModel : NotifyPropertyChangedBase, INavigationAware
{
    private bool _isInitialized;
    private string _appVersion = string.Empty;
    private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

    public SettingsViewModel()
    {
        ChangeThemeCommand = new DelegateCommand<string>(OnChangeTheme!);
    }

    public ApplicationTheme CurrentTheme
    {
        get => _currentTheme;
        set => SetField(ref _currentTheme, value);
    }

    public string AppVersion => _appVersion;

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
        _appVersion = $"Bllueprint - {GetAssemblyVersion()}";
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
