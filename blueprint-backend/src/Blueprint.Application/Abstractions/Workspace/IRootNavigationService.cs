namespace Blueprint.Application.Abstractions.Workspace;

public interface IRootNavigationService
{
    public object Settings { get; }

    public object Home { get; }

    public object Data { get; }

    public object Code { get; }

    public object HomeMenu { get; }
}
