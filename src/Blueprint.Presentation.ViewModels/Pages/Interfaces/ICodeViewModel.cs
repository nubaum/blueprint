namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ICodeViewModel
{
    IReadOnlyCollection<TabContent> Tabs { get; }

    object? SelectedTab { get; set; }
}
