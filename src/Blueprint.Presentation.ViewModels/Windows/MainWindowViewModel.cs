using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Presentation.ViewModels.Windows;

internal class MainWindowViewModel(IReadWorkspaceStore workspaceStore) : BindableObject, IMainWindowViewModel
{
    public IReadWorkspaceStore WorkspaceStore => workspaceStore;
}
