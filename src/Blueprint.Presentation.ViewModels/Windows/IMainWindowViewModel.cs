using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Presentation.ViewModels.Windows;

public interface IMainWindowViewModel
{
    IReadWorkspaceStore WorkspaceStore { get; }
}
