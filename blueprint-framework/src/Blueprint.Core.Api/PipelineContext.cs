using Blueprint.Core.Domain;

namespace Blueprint.Core.Api;

internal sealed class PipelineContext(INotificationContext notifications)
{
    public INotificationContext Notifications { get; } = notifications;

    public bool Failed => Notifications.HasErrors;

    public void Fail(FailureDetail failureDetail)
    {
        Notifications.Add(new Notification
        {
            TransitionName = failureDetail.TransitionName,
            Message = failureDetail.Message,
            Kind = failureDetail.Kind ?? NotificationKind.ValidationError
        });
    }

    public void Fail(string message)
    {
        Notifications.Add(new Notification
        {
            TransitionName = "Pipeline",
            Message = message,
            Kind = NotificationKind.ValidationError
        });
    }
}
