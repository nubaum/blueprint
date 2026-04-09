namespace Blueprint.Presentation.Adapters.Abstractions;

public interface ICommandManagerSubscription
{
    void Subscribe(EventHandler handler);

    void Unsubscribe(EventHandler handler);

    void InvalidateRequerySuggested();
}
