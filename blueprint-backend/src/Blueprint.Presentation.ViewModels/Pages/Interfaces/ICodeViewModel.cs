using Blueprint.Application.Abstractions.Workspace;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ICodeViewModel
{
    IReadWorkspaceStore WorkspaceStore { get; }

    IReadProjectTreeStore ProjectTreeStore { get; }
}
