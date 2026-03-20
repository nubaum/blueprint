using System.Windows.Input;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class DashboardViewModel : NotifyPropertyChangedBase, IDashboardViewModel
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
