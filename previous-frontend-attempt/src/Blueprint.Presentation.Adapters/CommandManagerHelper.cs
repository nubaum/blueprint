using System.Runtime.CompilerServices;
using Blueprint.Presentation.Adapters.Abstractions;

namespace Blueprint.Presentation.Adapters;

public static class CommandManagerHelper
{
    public static ICommandManager? CommandManager { get; set; }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Subscribe(EventHandler canExecuteChangedHandler)
    {
        if (CommandManager != null)
        {
            CommandManager!.RequerySuggested += canExecuteChangedHandler;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Unsubscribe(EventHandler canExecuteChangedHandler)
    {
        if (CommandManager != null)
        {
            CommandManager!.RequerySuggested -= canExecuteChangedHandler;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void InvalidateRequerySuggested()
    {
        CommandManager?.InvalidateRequerySuggested();
    }
}
