using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class CodeViewModel : BindableObject, ICodeViewModel
{
    public CodeViewModel(IUiCoreServices uiCoreServices, IReadWorkspaceStore readWorkspaceStore, IReadProjectTreeStore projectTreeStore)
        : base(uiCoreServices)
    {
        WorkspaceStore = readWorkspaceStore;
        ProjectTreeStore = projectTreeStore;
    }

    public IReadWorkspaceStore WorkspaceStore { get; }

    public IReadProjectTreeStore ProjectTreeStore { get; }
}
