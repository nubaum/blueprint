using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Presentation.ViewModels.Windows;

internal class MainWindowViewModel(IReadWorkspaceStore workspaceStore) : NotifyPropertyChangedBase, IMainWindowViewModel
{
    public IReadWorkspaceStore WorkspaceStore => workspaceStore;
}
