using System.Windows.Input;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface IDashboardViewModel
{
    ICommand CounterIncrementCommand { get; }

    int Counter { get; set; }
}
