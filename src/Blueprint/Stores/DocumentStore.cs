using ActiproSoftware.Text.Implementation;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Stores;

internal class DocumentStore : BindableObject, IWriteDocumentStore
{
    private readonly Dictionary<string, EditorDocument> _documents = new(StringComparer.OrdinalIgnoreCase);

    public object? GetDocument(string filePath)
    {
        if (_documents.TryGetValue(filePath, out EditorDocument? doc))
        {
            return doc;
        }

        return default;
    }

    public object CreateDocument(string filePath)
    {
        EditorDocument result = new();
        _documents[filePath] = result;

        return result;
    }

    public void ClearDocuemntStore()
    {
        _documents.Clear();
    }
}
