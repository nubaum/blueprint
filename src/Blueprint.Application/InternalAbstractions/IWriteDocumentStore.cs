using Blueprint.Abstractions.Application.Languages;

namespace Blueprint.Application.InternalAbstractions;

internal interface IWriteDocumentStore : IReadDocumentStore
{
    void ClearDocuemntStore();

    object CreateDocument(string filePath);
}
