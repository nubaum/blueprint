using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.InternalAbstractions;

namespace Blueprint.Application.Services;

internal class WorkspaceService(
        IWriteDocumentStore documentStore,
        IWriteWorkspaceStore workspaceStore)
{
    public async Task OpenProjectAsync(string path)
    {
        if (File.Exists(path))
        {
            string folder = Path.GetDirectoryName(path)!;
            string projectName = Path.GetFileNameWithoutExtension(path);
            workspaceStore.ClearItems(o => o.Kind == WorkspaceItemKind.Doucument);
            workspaceStore.SetCurrentProject(new ProjectInfo { ProjectFolder = folder, ProjectName = projectName, ProjectFullPath = path });
        }

        await Task.CompletedTask;
    }

    public async Task OpenDocumentAsync(string fileName)
    {
        object doc = documentStore.GetDocument(fileName);
        workspaceStore.AddEditor(fileName, doc);
        await Task.CompletedTask;
    }
}
