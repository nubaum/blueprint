using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Application.Services;

internal class PathProvider(IReadWorkspaceStore readWorkspaceStore)
{
    public string GetFullPathOfDocumentName(string fileName)
    {
        if (readWorkspaceStore.CurrentProject?.ProjectFolder is string projectFolder)
        {
            return Path.Combine(projectFolder, fileName);
        }

        throw new InvalidOperationException("There is no open project to open a document");
    }
}
