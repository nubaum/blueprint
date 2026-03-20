using System.Runtime.CompilerServices;

namespace Blueprint.Presentation.ViewModels.Core;

internal static class CommandManagerHelper
{
    private static readonly ConditionalWeakTable<EventHandler, EventHandler<EventArgs>> _wrapperCache = [];

    public static ICommandManager? CommandManager { get; set; }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Subscribe(EventHandler canExecuteChangedHandler)
    {
        if (CommandManager != null)
        {
            void Wrapper(object? s, EventArgs e) => canExecuteChangedHandler(s, e);

            _wrapperCache.AddOrUpdate(
                canExecuteChangedHandler,
                Wrapper);

            WeakEventManager<ICommandManager, EventArgs>.AddHandler(
                CommandManager,
                nameof(ICommandManager.RequerySuggested),
                Wrapper);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Unsubscribe(EventHandler canExecuteChangedHandler)
    {
        if (CommandManager != null &&
            _wrapperCache.TryGetValue(canExecuteChangedHandler, out EventHandler<EventArgs>? wrapper))
        {
            WeakEventManager<ICommandManager, EventArgs>.RemoveHandler(
                CommandManager,
                nameof(ICommandManager.RequerySuggested),
                wrapper);
            _wrapperCache.Remove(canExecuteChangedHandler);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void InvalidateRequerySuggested()
    {
        CommandManager?.InvalidateRequerySuggested();
    }
}
