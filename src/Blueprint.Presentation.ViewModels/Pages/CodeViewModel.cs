using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class CodeViewModel : NotifyPropertyChangedBase, ICodeViewModel
{
    public CodeViewModel(IWorkspaceService workspaceService, IReadWorkspaceStore readWorkspaceStore)
    {
        _ = workspaceService.OpenDocumentAsync();
        WorkspaceStore = readWorkspaceStore;
    }

    public IReadWorkspaceStore WorkspaceStore { get; }
}
