namespace Blueprint.Presentation.ViewModels.Core;

public interface ICommandManager
{
    event EventHandler RequerySuggested;

    void InvalidateRequerySuggested();
}
