using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Presentation.ViewModels.Pages.Interfaces;

public interface ICodeViewModel
{
    IReadWorkspaceStore WorkspaceStore { get; }

    IReadProjectTreeStore ProjectTreeStore { get; }
}
