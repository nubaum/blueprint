namespace Blueprint.Abstractions.Application.Languages;

public interface IReadDocumentStore
{
    object GetDocument(string filePath);
}
