using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Application.Services;
using MediatR;

namespace Blueprint.Application.Handlers.Workspace;

internal class OpenProjectRequestHandler(
    WorkspaceService workspaceService,
    IViewNavigationHost viewNavigationHost,
    IWriteProjectTreeStore projectTreeStore,
    FolderTreeBuilder folderTreeBuilder)
: IRequestHandler<OpenProjectRequest>
{
    public async Task Handle(OpenProjectRequest request, CancellationToken cancellationToken)
    {
        await workspaceService.OpenProjectAsync(request.ProjectFilePath);
        IProjectTreeNode root = folderTreeBuilder.Build(request.ProjectFilePath);
        projectTreeStore.SetRoot(root);
        viewNavigationHost.NavigateToCode();
    }
}
