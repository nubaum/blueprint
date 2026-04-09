using System.Runtime.CompilerServices;
using Blueprint.Application.Abstractions;

namespace Blueprint.Services;

internal sealed class WpfCommandManagerSubscription(ICommandManager commandManager) : ICommandManagerSubscription
{
    private readonly ICommandManager _commandManager = commandManager ?? throw new ArgumentNullException(nameof(commandManager));
    private readonly ConditionalWeakTable<EventHandler, EventHandler<EventArgs>> _wrapperCache = [];

    public void Subscribe(EventHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        void Wrapper(object? sender, EventArgs args) => handler(sender, args);

        _wrapperCache.AddOrUpdate(handler, Wrapper);

        WeakEventManager<ICommandManager, EventArgs>.AddHandler(
            _commandManager,
            nameof(ICommandManager.RequerySuggested),
            Wrapper);
    }

    public void Unsubscribe(EventHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        if (_wrapperCache.TryGetValue(handler, out EventHandler<EventArgs>? wrapper))
        {
            WeakEventManager<ICommandManager, EventArgs>.RemoveHandler(
                _commandManager,
                nameof(ICommandManager.RequerySuggested),
                wrapper);

            _wrapperCache.Remove(handler);
        }
    }

    public void InvalidateRequerySuggested()
    {
        _commandManager.InvalidateRequerySuggested();
    }
}
