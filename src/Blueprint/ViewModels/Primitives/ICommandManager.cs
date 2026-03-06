namespace Blueprint.ViewModels.Primitives;

public interface ICommandManager
{
    event EventHandler RequerySuggested;

    void InvalidateRequerySuggested();
}
