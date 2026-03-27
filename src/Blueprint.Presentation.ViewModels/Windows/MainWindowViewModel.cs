using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Presentation.ViewModels.Windows;

internal class MainWindowViewModel(IUiCoreServices uiCoreServices, IReadWorkspaceStore workspaceStore) : BindableObject(uiCoreServices), IMainWindowViewModel
{
    public IReadWorkspaceStore WorkspaceStore => workspaceStore;
}
