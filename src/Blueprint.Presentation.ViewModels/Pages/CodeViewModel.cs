using Blueprint.Application.ViewModelContracts.Enums;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class CodeViewModel : NotifyPropertyChangedBase, ICodeViewModel
{
    private readonly BPObservableCollection<TabContent> _tabs = [];
    private object? _selectedTab;

    public CodeViewModel()
    {
        _tabs.Add(new TabContent { Caption = "Welcome", IsPinned = true, Content = "Content 1", Kind = WorkspaceItemKind.Doucument });
        _tabs.Add(new TabContent { Caption = "Document 1", IsDirty = true, Content = "Content 2", Kind = WorkspaceItemKind.Doucument });
        _tabs.Add(new TabContent { Caption = "Document 2", Content = "Test 001", Kind = WorkspaceItemKind.Doucument });
        _tabs.Add(new TabContent { Caption = "Document 4", Content = "Test 004", Kind = WorkspaceItemKind.Doucument });
        SelectedTab = _tabs[0];
    }

    public IReadOnlyCollection<TabContent> Tabs => _tabs;

    public object? SelectedTab
    {
        get => _selectedTab;
        set => SetField(ref _selectedTab, value);
    }
}
