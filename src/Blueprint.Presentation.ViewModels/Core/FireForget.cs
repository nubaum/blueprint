namespace Blueprint.Presentation.ViewModels.Core;

internal static class FireForget
{
    public static void RunOnMainThread(Func<Task> asyncAction, Action<Exception> onException)
        => InternalRunSafely(asyncAction, onException, true);

    private static Task InternalRunSafelyAsync(Func<Task> asyncFunc, Action<Exception> onException, bool handleInMainThread)
        => Task.Run(async () =>
            {
                try
                {
                    await asyncFunc().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(ex, onException, handleInMainThread);
                }
            });

    private static void InternalRunSafely(Func<Task> asyncFunc, Action<Exception> onException, bool handleInMainThread)
    {
        _ = InternalRunSafelyAsync(asyncFunc, onException, handleInMainThread);
    }

    private static async Task HandleExceptionAsync(Exception ex, Action<Exception> onException, bool handleInMainThread)
    {
        if (handleInMainThread)
        {
            await Application.Current.Dispatcher.BeginInvoke(() => onException.Invoke(ex));
        }
        else
        {
            onException.Invoke(ex);
        }
    }
}
