namespace Blueprint.ViewModels;

public interface ICommandManager
{
    event EventHandler RequerySuggested;

    void InvalidateRequerySuggested();
}
