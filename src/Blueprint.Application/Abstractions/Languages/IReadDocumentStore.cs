namespace Blueprint.Application.Abstractions.Languages;

public interface IReadDocumentStore
{
    object GetDocument(string filePath);
}
