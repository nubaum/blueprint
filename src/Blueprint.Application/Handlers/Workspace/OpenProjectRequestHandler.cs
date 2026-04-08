using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Application.Services;
using MediatR;

namespace Blueprint.Application.Handlers.Workspace;

internal class OpenProjectRequestHandler(
    IFolderPicker folderPicker,
    WorkspaceService workspaceService,
    IViewNavigationHost viewNavigationHost,
    IWriteProjectTreeStore projectTreeStore,
    FolderTreeBuilder folderTreeBuilder)
: IRequestHandler<OpenProjectRequest>
{
    public async Task Handle(OpenProjectRequest request, CancellationToken cancellationToken)
    {
        string? selectedFolderResult = await folderPicker.PickFolderAsync();
        if (selectedFolderResult is string selectedFolder)
        {
            await workspaceService.OpenProjectAsync(selectedFolder);
            IProjectTreeNode root = folderTreeBuilder.Build(selectedFolder);
            projectTreeStore.SetRoot(root);
            viewNavigationHost.NavigateToCode();
        }
    }
}
