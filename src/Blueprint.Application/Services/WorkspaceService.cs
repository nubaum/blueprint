using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Application.Services;

internal class WorkspaceService(
    IWriteWorkspaceStore workspaceStore)
{
    public async Task OpenProjectAsync(string path)
    {
        if (File.Exists(path))
        {
            workspaceStore.ClearItems(o => o.Kind == WorkspaceItemKind.Doucument);
        }

        await Task.CompletedTask;
    }
}
