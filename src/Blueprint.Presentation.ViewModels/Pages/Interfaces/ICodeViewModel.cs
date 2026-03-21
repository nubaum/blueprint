using System.Collections.ObjectModel;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ICodeViewModel
{
    ObservableCollection<TabContent> Tabs { get; }

    object? SelectedTab { get; set; }
}
