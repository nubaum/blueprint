using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Presentation.Adapters;

namespace Blueprint.Presentation.ViewModels.Windows;

internal class MainWindowViewModel(IReadWorkspaceStore workspaceStore) : BindableObject, IMainWindowViewModel
{
    public IReadWorkspaceStore WorkspaceStore => workspaceStore;
}
