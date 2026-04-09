using Blueprint.Abstractions;
using Blueprint.Application.Abstractions;
using Blueprint.Application.Core;
using Blueprint.Application.InternalAbstractions;

namespace Blueprint.Stores;

internal class DocumentStore(IDocumentLoader documentLoader) : BindableObject, IWriteDocumentStore
{
    private readonly Dictionary<string, object> _documents = new(StringComparer.OrdinalIgnoreCase);

    public object GetDocument(string filePath)
    {
        if (_documents.TryGetValue(filePath, out object? doc))
        {
            return doc;
        }

        throw new InvalidOperationException(StringProvider.Application.Messages.DocumentNotFound(filePath));
    }

    public object CreateDocument(string filePath)
    {
        if(File.Exists(filePath))
        {
            throw new InvalidOperationException(StringProvider.Application.Messages.FileAlreadyExists(filePath));
        }

        File.Create(filePath);
        return documentLoader.LoadDocument(filePath);
    }

    public void ClearDocuemntStore()
    {
        _documents.Clear();
    }

    public object AddDocument(object document, string fileName)
    {
        _documents[fileName] = document;
        return document;
    }
}
