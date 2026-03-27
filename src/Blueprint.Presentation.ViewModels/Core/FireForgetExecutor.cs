using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Blueprint.Presentation.ViewModels.Core;

public abstract class FireForgetExecutor : BindableObject
{
    protected FireForgetExecutor(IUiCoreServices uiCoreServices)
    {
        Logger = uiCoreServices.LoggerFactory.CreateLogger(GetType());
        SnackbarService = uiCoreServices.SnackbarService;
    }

    protected ILogger Logger { get; }

    protected ISnackbarService SnackbarService { get; }

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
