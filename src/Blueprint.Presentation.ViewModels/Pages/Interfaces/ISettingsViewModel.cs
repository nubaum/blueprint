using System.Windows.Input;
using Wpf.Ui.Appearance;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ISettingsViewModel
{
    ApplicationTheme CurrentTheme { get; set; }

    string AppVersion { get; }

    ICommand ChangeThemeCommand { get; }

    Task OnNavigatedFromAsync();

    Task OnNavigatedToAsync();
}
