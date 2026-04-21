namespace Blueprint.Application.Abstractions;

public interface IDocumentLoader
{
    public object LoadDocument(string filePath);
}
