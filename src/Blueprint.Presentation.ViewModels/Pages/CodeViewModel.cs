using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class CodeViewModel : BindableObject, ICodeViewModel
{
    public CodeViewModel(IReadWorkspaceStore readWorkspaceStore, IReadProjectTreeStore projectTreeStore)
    {
        WorkspaceStore = readWorkspaceStore;
        ProjectTreeStore = projectTreeStore;
    }

    public IReadWorkspaceStore WorkspaceStore { get; }

    public IReadProjectTreeStore ProjectTreeStore { get; }
}
