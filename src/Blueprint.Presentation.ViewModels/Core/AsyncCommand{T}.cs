using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace Blueprint.Presentation.ViewModels.Core;

public class AsyncCommand<T>(
    Func<T?, Task> execute,
    ILogger logger,
    Func<T?, bool>? canExecute = null) : ICommand
{
    private readonly Func<T?, Task> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<T?, bool>? _canExecute = canExecute;
    private readonly ILogger _logger = logger;
    private bool _isExecuting;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManagerHelper.Subscribe(value!);
        remove => CommandManagerHelper.Unsubscribe(value!);
    }

    public bool CanExecute(object? parameter)
        => !_isExecuting && (_canExecute?.Invoke(CastParameter(parameter)) ?? true);

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        _isExecuting = true;
        RaiseCanExecuteChanged();

        _ = ExecuteCoreAsync(CastParameter(parameter));
    }

    public void NotifyCanExecuteChanged()
        => RaiseCanExecuteChanged();

    private static T? CastParameter(object? parameter)
    {
        if (parameter is null)
        {
            return default;
        }

        if (parameter is T typedParameter)
        {
            return typedParameter;
        }

        throw new InvalidCastException(
            $"{nameof(parameter)} expected: '{typeof(T).FullName} but found: '{parameter.GetType().FullName}'.");
    }

    private static void RaiseCanExecuteChanged()
        => CommandManagerHelper.InvalidateRequerySuggested();

    private async Task ExecuteCoreAsync(T? parameter)
    {
        try
        {
            await _execute(parameter).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "An error occurred during command execution.");
        }
        finally
        {
            await UIDispatcher.RunOnUiThreadAsync(() =>
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            });
        }
    }
}
