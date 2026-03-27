using Blueprint.Abstractions;
using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Application.Services;

internal class PathProvider(IReadWorkspaceStore readWorkspaceStore) : IPathProvider
{
    public string SourceFolderPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), StringProvider.Application.SourceFolderName);

    public string GetFullPathOfDocumentName(string fileName)
    {
        if (readWorkspaceStore.CurrentProject?.ProjectFolder is string projectFolder)
        {
            return Path.Combine(projectFolder, fileName);
        }

        throw new InvalidOperationException(StringProvider.ErrorMessages.InvalidFolderToOpenDocument);
    }

    public string GetLoggFileName() =>
        Path.Combine(AppContext.BaseDirectory, StringProvider.Application.LogFilePrefix);

    public void CreateSourceFolderPath()
    {
        Directory.CreateDirectory(SourceFolderPath);
    }
}
