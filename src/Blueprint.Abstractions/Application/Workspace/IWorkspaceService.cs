namespace Blueprint.Abstractions.Application.Workspace;

public interface IWorkspaceService
{
    Task OpenDocumentAsync();

    Task OpenProjectAsync(string path);
}
