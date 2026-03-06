using System.Collections.ObjectModel;

namespace Blueprint.ViewModels.UserControls;

public class CodeViewModel : NotifyPropertyChangedBase
{
    private DocumentTabViewModel? _selectedTab;

    public CodeViewModel()
    {
        Tabs.Add(new DocumentTabViewModel { Caption = "Welcome", IsPinned = true, Content = "Content 1" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 1", IsDirty = true, Content = "Content 2" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 2", Content = "Content 3" });
        SelectedTab = Tabs[0];
    }

    public ObservableCollection<DocumentTabViewModel> Tabs { get; } = new();

    public DocumentTabViewModel? SelectedTab
    {
        get => _selectedTab;
        set => SetField(ref _selectedTab, value);
    }
}
