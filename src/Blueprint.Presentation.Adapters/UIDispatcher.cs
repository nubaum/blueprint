using Blueprint.Presentation.Adapters.Abstractions;

namespace Blueprint.Presentation.Adapters;

public static class UIDispatcher
{
    public static IUISynchronizer? UIScynchronizer { get; set; }

    public static void RunOnUiThread(Action action)
    {
        if (UIScynchronizer == null)
        {
            action();
        }
        else
        {
            UIScynchronizer.RunOnUiThread(action);
        }
    }

    public static async Task RunOnUiThreadAsync(Action action)
    {
        if (UIScynchronizer == null)
        {
            action();
        }
        else
        {
            await UIScynchronizer.RunOnUiThreadAsync(action);
        }
    }
}
