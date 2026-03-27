using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Blueprint.Presentation.ViewModels.Core;

public abstract class BindableObject : INotifyPropertyChanged
{
    protected BindableObject(IUiCoreServices uiCoreServices)
    {
        Logger = uiCoreServices.LoggerFactory.CreateLogger(GetType());
        SnackbarService = uiCoreServices.SnackbarService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected ILogger Logger { get; }

    protected ISnackbarService SnackbarService { get; }

    protected void OnPropertyChanged([CallerMemberName] string name = default!)
    {
        UIDispatcher.RunOnUiThread(() => PropertyChanged?.Invoke(this, new(name)));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string name = null!)
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(name);
        return true;
    }

    protected void FireAndForget(
        Func<Task> action,
        string? errorMessage = null,
        [CallerMemberName] string caller = "")
    {
        FireForget.RunHandlingExceptionsInMainThread(action, (ex) =>
        {
            Logger.LogError(ex, "Unhandled exception in FireAndForget call from {Caller}", caller);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                SnackbarService.Show(
                    "Error",
                    errorMessage,
                    ControlAppearance.Danger,
                    null,
                    TimeSpan.FromSeconds(5));
            }
        });
    }
}
