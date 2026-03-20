using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace Blueprint.ViewModels.Core;

public class AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null) : ICommand
{
    private readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole().AddDebug()).CreateLogger<AsyncCommand>();
    private readonly Func<Task> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<bool>? _canExecuteMethod = canExecute;
    private bool _isExecuting;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManagerHelper.Subscribe(value!);
        remove => CommandManagerHelper.Unsubscribe(value!);
    }

    public bool CanExecute(object? parameter) => !_isExecuting && (_canExecuteMethod?.Invoke() ?? true);

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

    private static void RaiseCanExecuteChanged() => CommandManagerHelper.InvalidateRequerySuggested();

    private async Task ExecuteCoreAsync()
    {
        try
        {
            await _execute().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during command execution.");
        }
        finally
        {
            await App.Current.Dispatcher.BeginInvoke(async () =>
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            });
        }
    }
}
