namespace Blueprint.Presentation.ViewModels.Core;

public static class UiDispatcher
{
    public static void Invoke(Action action)
    {
        Application.Current.Dispatcher.Invoke(action);
    }

    public static async Task InvokeAsync(Action action)
    {
        await Application.Current.Dispatcher.BeginInvoke(action);
    }
}
