using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.ViewModels.Core;

public class AsyncCommand<T>(Func<T?, Task> execute, Func<T?, bool>? canExecute = null) : ICommand
{
    private readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole().AddDebug()).CreateLogger<AsyncCommand<T>>();
    private readonly Func<T?, Task> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<T?, bool>? _canExecuteMethod = canExecute;
    private bool _isExecuting;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManagerHelper.Subscribe(value!);
        remove => CommandManagerHelper.Unsubscribe(value!);
    }

    public bool CanExecute(object? parameter) => !_isExecuting && (_canExecuteMethod?.Invoke((T?)parameter) ?? true);

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        _isExecuting = true;
        RaiseCanExecuteChanged();

        _ = ExecuteCoreAsync(parameter);
    }

    private static void RaiseCanExecuteChanged() => CommandManagerHelper.InvalidateRequerySuggested();

    private async Task ExecuteCoreAsync(object? parameter)
    {
        try
        {
            await _execute((T?)parameter).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during command execution.");
        }
        finally
        {
            await UiDispatcher.InvokeAsync(async () =>
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            });
        }
    }
}
