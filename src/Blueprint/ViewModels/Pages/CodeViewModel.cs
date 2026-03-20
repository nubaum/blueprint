using System.Collections.ObjectModel;
using Blueprint.ViewModels.Core;
using Blueprint.ViewModels.UserControls;
using Blueprint.Views.UserControls;

namespace Blueprint.ViewModels.Pages;

public class CodeViewModel : NotifyPropertyChangedBase
{
    private object? _selectedTab;

    public CodeViewModel()
    {
        Tabs.Add(new DocumentTabViewModel { Caption = "Welcome", IsPinned = true, Content = "Content 1" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 1", IsDirty = true, Content = "Content 2" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 2", Content = new CodeEditor() });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 4", Content = new CodeEditor() });
        SelectedTab = Tabs[0];
    }

    public ObservableCollection<DocumentTabViewModel> Tabs { get; } = [];

    public object? SelectedTab
    {
        get => _selectedTab;
        set => SetField(ref _selectedTab, value);
    }
}
