using Blueprint.Abstractions;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Application.Workspace.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Blueprint.Services;

internal sealed class NotificationService(ISnackbarService snackbarService) : INotificationService
{
    public async Task ShowAsync(Notification notification)
    {
        // TODO: adjust the icon.
        snackbarService.Show(notification.Caption, notification.Message, GetApearanceByNotificationKind(notification.Kind), null, TimeSpan.FromSeconds(notification.LifespanSeconds));
        await Task.CompletedTask;
    }

    private static ControlAppearance GetApearanceByNotificationKind(NotificationKind kind)
    {
        return kind switch
        {
            NotificationKind.Error => ControlAppearance.Danger,
            NotificationKind.Warning => ControlAppearance.Danger,
            NotificationKind.Information => ControlAppearance.Info,
            _ => throw new InvalidOperationException(StringProvider.Application.Messages.InvalidNotificationKind(kind)),
        };
    }
}
