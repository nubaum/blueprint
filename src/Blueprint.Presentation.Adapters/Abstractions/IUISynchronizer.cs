namespace Blueprint.Application.Abstractions;

public interface IUISynchronizer
{
    void RunOnUiThread(Action action);

    Task RunOnUiThreadAsync(Action action);
}
