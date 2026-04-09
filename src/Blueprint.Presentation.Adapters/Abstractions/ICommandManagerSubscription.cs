namespace Blueprint.Application.Abstractions;

public interface ICommandManagerSubscription
{
    void Subscribe(EventHandler handler);

    void Unsubscribe(EventHandler handler);

    void InvalidateRequerySuggested();
}
