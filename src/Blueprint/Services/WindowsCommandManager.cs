using System.Windows.Input;
using Blueprint.Application.Abstractions;

namespace Blueprint.Services;

public class WindowsCommandManager : ICommandManager
{
    public event EventHandler RequerySuggested
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void InvalidateRequerySuggested() => CommandManager.InvalidateRequerySuggested();
}
