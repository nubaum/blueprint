using System.Collections.ObjectModel;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ICodeViewModel
{
    ObservableCollection<ITabViewModel> Tabs { get; }

    object? SelectedTab { get; set; }
}
