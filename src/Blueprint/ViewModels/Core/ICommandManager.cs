namespace Blueprint.ViewModels.Core;

public interface ICommandManager
{
    event EventHandler RequerySuggested;

    void InvalidateRequerySuggested();
}
