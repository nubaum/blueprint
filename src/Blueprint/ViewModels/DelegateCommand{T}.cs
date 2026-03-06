using System.Windows.Input;

namespace Blueprint.ViewModels;

public class DelegateCommand<T>(Action<T?> execute, Func<T?, bool>? canExecute = null) : ICommand
{
    private readonly Action<T?> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<T?, bool>? _canExecuteMethod = canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManagerHelper.Subscribe(value!);
        remove => CommandManagerHelper.Unsubscribe(value!);
    }

    public bool CanExecute(object? parameter) => _canExecuteMethod?.Invoke((T?)parameter) ?? true;

    public void Execute(object? parameter) => _execute((T?)parameter);

    public void RaiseCanExecuteChanged() => CommandManagerHelper.InvalidateRequerySuggested();
}
