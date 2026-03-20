namespace Blueprint.Application.Interfaces;

public interface IProjectWorkspace
{
    public IProjectInfo CurrentProject { get; }

    public IReadOnlyCollection<IDocumentInfo> OpenDocuments { get; }
}
