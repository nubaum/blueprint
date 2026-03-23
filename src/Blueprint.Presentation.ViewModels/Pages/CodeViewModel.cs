using System.Collections.ObjectModel;
using Blueprint.Application.ViewModelContracts.Enums;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class CodeViewModel : NotifyPropertyChangedBase, ICodeViewModel
{
    private object? _selectedTab;

    public CodeViewModel()
    {
        Tabs.Add(new TabContent { Caption = "Welcome", IsPinned = true, Content = "Content 1", Kind = WorkspaceItemKind.Doucument });
        Tabs.Add(new TabContent { Caption = "Document 1", IsDirty = true, Content = "Content 2", Kind = WorkspaceItemKind.Doucument });
        Tabs.Add(new TabContent { Caption = "Document 2", Content = "Test 001", Kind = WorkspaceItemKind.Doucument });
        Tabs.Add(new TabContent { Caption = "Document 4", Content = "Test 004", Kind = WorkspaceItemKind.Doucument });
        SelectedTab = Tabs[0];
    }

    public ObservableCollection<TabContent> Tabs { get; } = [];

    public object? SelectedTab
    {
        get => _selectedTab;
        set => SetField(ref _selectedTab, value);
    }
}
