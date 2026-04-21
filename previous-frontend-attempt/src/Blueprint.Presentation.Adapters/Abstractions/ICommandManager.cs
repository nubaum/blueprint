namespace Blueprint.Presentation.Adapters.Abstractions;

public interface ICommandManager
{
    event EventHandler RequerySuggested;

    void InvalidateRequerySuggested();
}
