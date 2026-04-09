using Blueprint.Application.Abstractions.Workspace;

namespace Blueprint.Presentation.ViewModels.Windows;

public interface IMainWindowViewModel
{
    IReadWorkspaceStore WorkspaceStore { get; }
}
