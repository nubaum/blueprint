using ActiproSoftware.Text.Implementation;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;
using Blueprint.Views.Models;
using Blueprint.Views.UserControls;

namespace Blueprint.Stores;

internal class WorskpaceStore : NotifyPropertyChangedBase, IWriteWorkspaceStore
{
    private readonly BPObservableCollection<IWorkspaceItem> _openItems = [];
    private IWorkspaceItem? _selectedItem;

    public ProjectInfo? CurrentProject { get; private set; }

    public IWorkspaceItem? SelectedItem
    {
        get => _selectedItem;
        set => SetField(ref _selectedItem, value);
    }

    public IReadOnlyCollection<IWorkspaceItem> OpenItems => _openItems;

    public void AddItem(IWorkspaceItem item)
    {
        _openItems.Add(item);
        SelectedItem = item;
    }

    public void AddItems(IEnumerable<IWorkspaceItem> items)
    {
        _openItems.AddRange(items);
    }

    public void ClearItems()
    {
        _openItems.Clear();
    }

    public void ClearItems(Predicate<IWorkspaceItem> criteria)
    {
        _openItems.RemoveAll(criteria);
    }

    public void SetCurrentProject(ProjectInfo projectInfo)
    {
        CurrentProject = projectInfo;
    }

    public void AddDocument(string caption, object document, object language)
    {
        if (document is EditorDocument doc && language is SyntaxLanguage lang)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var result = new BlueLangEditor();
                if (result.DataContext is IBlueLangEditorViewModel viewModel)
                {
                    doc.Language = lang;
                    viewModel.Document = doc;
                }

                AddItem(new TabContent { Caption = caption, Content = result, Kind = WorkspaceItemKind.Doucument });
            });
        }
    }
}
