namespace Blueprint.Presentation.ViewModels.Core;

public static class UiDispatcher
{
    public static void Invoke(Action action)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(action);
    }

    public static async Task InvokeAsync(Action action)
    {
        await System.Windows.Application.Current.Dispatcher.BeginInvoke(action);
    }
}
