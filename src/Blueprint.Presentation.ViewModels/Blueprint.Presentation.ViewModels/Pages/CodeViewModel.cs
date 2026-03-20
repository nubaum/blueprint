using System.Collections.ObjectModel;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.UserControls;

namespace Blueprint.Presentation.ViewModels.Pages;

public class CodeViewModel : NotifyPropertyChangedBase
{
    private object? _selectedTab;

    public CodeViewModel()
    {
        Tabs.Add(new DocumentTabViewModel { Caption = "Welcome", IsPinned = true, Content = "Content 1" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 1", IsDirty = true, Content = "Content 2" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 2" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 4" });
        SelectedTab = Tabs[0];
    }

    public ObservableCollection<DocumentTabViewModel> Tabs { get; } = [];

    public object? SelectedTab
    {
        get => _selectedTab;
        set => SetField(ref _selectedTab, value);
    }
}
