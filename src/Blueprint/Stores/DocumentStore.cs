using System.IO;
using ActiproSoftware.Text;
using ActiproSoftware.Text.Implementation;
using Blueprint.Abstractions;
using Blueprint.Abstractions.Application.Languages;
using Blueprint.Application.Core;
using Blueprint.Application.InternalAbstractions;

namespace Blueprint.Stores;

internal class DocumentStore(ILanguageProvider languageProvider) : BindableObject, IWriteDocumentStore
{
    private readonly Dictionary<string, EditorDocument> _documents = new(StringComparer.OrdinalIgnoreCase);

    public object GetDocument(string filePath)
    {
        if (_documents.TryGetValue(filePath, out EditorDocument? doc))
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
        return LoadDocument(filePath);
    }

    public object LoadDocument(string filePath)
    {
        EditorDocument doc = new();
        string docExtension = Path.GetExtension(filePath);
        doc.Language = languageProvider.GetLanguageByExtension<ISyntaxLanguage>(docExtension);
        doc.LoadFile(filePath);
        return doc;
    }

    public void ClearDocuemntStore()
    {
        _documents.Clear();
    }

    public object AddDocument(object document)
    {
        if (document is not EditorDocument doc)
        {
            throw new InvalidOperationException(StringProvider.Application.Messages.InvalidDocument);
        }

        _documents[doc.FileName] = doc;
        return doc;
    }
}
