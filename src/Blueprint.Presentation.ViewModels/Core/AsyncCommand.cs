using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.ViewModels.Core;

public class AsyncCommand(
    Func<Task> execute,
    Func<bool>? canExecute = null,
    ILogger<AsyncCommand>? logger = null) : ICommand
{
    private readonly Func<Task> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<bool>? _canExecute = canExecute;
    private readonly ILogger<AsyncCommand>? _logger = logger;
    private bool _isExecuting;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManagerHelper.Subscribe(value!);
        remove => CommandManagerHelper.Unsubscribe(value!);
    }

    public static void NotifyCanExecuteChanged()
        => RaiseCanExecuteChanged();

    public bool CanExecute(object? parameter)
        => !_isExecuting && (_canExecute?.Invoke() ?? true);

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        _isExecuting = true;
        RaiseCanExecuteChanged();

        _ = ExecuteCoreAsync();
    }

    private static void RaiseCanExecuteChanged()
        => CommandManagerHelper.InvalidateRequerySuggested();

    private async Task ExecuteCoreAsync()
    {
        try
        {
            await _execute().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "An error occurred during command execution.");
        }
        finally
        {
            await Application.Current.Dispatcher.BeginInvoke(() =>
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            });
        }
    }
}
