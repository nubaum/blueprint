using System.Windows.Input;
using Blueprint.Presentation.Adapters.Abstractions;

namespace Blueprint.Services;

internal class WindowsCommandManager : ICommandManager
{
    public event EventHandler RequerySuggested
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void InvalidateRequerySuggested() => CommandManager.InvalidateRequerySuggested();
}
