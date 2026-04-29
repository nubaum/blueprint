using Blueprint.Application.Abstractions.Languages;

namespace Blueprint.Application.InternalAbstractions;

internal interface IWriteDocumentStore : IReadDocumentStore
{
    void ClearDocuemntStore();

    object CreateDocument(string filePath);

    object AddDocument(object document, string fileName);
}
