using Blueprint.Presentation.Adapters.Abstractions;

namespace Blueprint.Services;

internal class UISynchronizer : IUISynchronizer
{
    public void RunOnUiThread(Action action)
    {
        if (System.Windows.Application.Current.Dispatcher.CheckAccess())
        {
            action();
            return;
        }

        System.Windows.Application.Current.Dispatcher.Invoke(action);
    }

    public async Task RunOnUiThreadAsync(Action action)
    {
        if (System.Windows.Application.Current.Dispatcher.CheckAccess())
        {
            action();
            return;
        }

        await System.Windows.Application.Current.Dispatcher.InvokeAsync(action);
    }
}
