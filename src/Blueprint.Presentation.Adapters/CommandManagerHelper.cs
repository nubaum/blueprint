using System.Runtime.CompilerServices;
using Blueprint.Presentation.Adapters.Abstractions;

namespace Blueprint.Presentation.Adapters;

public static class CommandManagerHelper
{
    public static ICommandManagerSubscription? Subscription { get; set; }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Subscribe(EventHandler canExecuteChangedHandler)
    {
        Subscription?.Subscribe(canExecuteChangedHandler);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Unsubscribe(EventHandler canExecuteChangedHandler)
    {
        Subscription?.Unsubscribe(canExecuteChangedHandler);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void InvalidateRequerySuggested()
    {
        Subscription?.InvalidateRequerySuggested();
    }
}
