namespace Blueprint.Application.Abstractions.Workspace;

public interface IPathProvider
{
    string SourceFolderPath { get; }

    void CreateSourceFolderPath();

    string GetFullPathOfDocumentName(string fileName);

    string GetLoggFileName();
}
