namespace Blueprint.Presentation.ViewModels.Core;

public static class UIDispatcher
{
    public static void RunOnUiThread(Action action)
    {
        if (Application.Current.Dispatcher.CheckAccess())
        {
            action();
            return;
        }

        Application.Current.Dispatcher.Invoke(action);
    }
}
