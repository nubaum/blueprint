using System.Runtime.CompilerServices;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Application.Workspace.Models;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.ViewModels.Core;

public abstract class FireForgetExecutor : BindableObject
{
    protected FireForgetExecutor(IUiCoreServices uiCoreServices)
    {
        Logger = uiCoreServices.LoggerFactory.CreateLogger(GetType());
        NotificationService = uiCoreServices.NotificationService;
    }

    protected ILogger Logger { get; }

    protected INotificationService NotificationService { get; }

    protected void FireAndForget(
        Func<Task> action,
        string? errorMessage = null,
        [CallerMemberName] string caller = "")
    {
        FireForget.RunOnMainThread(action, (ex) =>
        {
            Logger.LogError(ex, "Unhandled exception in FireAndForget call from {Caller}", caller);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                _ = NotificationService.ShowAsync(new Notification
                {
                    Caption = "Error",
                    Kind = NotificationKind.Error,
                    Message = ex.Message,
                    LifespanSeconds = 5
                });
            }
        });
    }
}
