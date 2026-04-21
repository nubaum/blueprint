using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Presentation.Adapters;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class CodeViewModel(IReadWorkspaceStore readWorkspaceStore, IReadProjectTreeStore projectTreeStore)
    : BindableObject, ICodeViewModel
{
    public IReadWorkspaceStore WorkspaceStore { get; } = readWorkspaceStore;

    public IReadProjectTreeStore ProjectTreeStore { get; } = projectTreeStore;
}
