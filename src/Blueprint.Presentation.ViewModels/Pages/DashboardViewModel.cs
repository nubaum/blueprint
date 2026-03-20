using System.Windows.Input;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Presentation.ViewModels.Pages;

public class DashboardViewModel : NotifyPropertyChangedBase
{
    private int _counter;

    public DashboardViewModel()
    {
        CounterIncrementCommand = new DelegateCommand(OnCounterIncrement);
    }

    public ICommand CounterIncrementCommand { get; }

    public int Counter
    {
        get => _counter;
        set => SetField(ref _counter, value);
    }

    private void OnCounterIncrement()
    {
        Counter++;
    }
}
