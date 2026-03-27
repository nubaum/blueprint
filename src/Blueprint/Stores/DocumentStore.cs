using ActiproSoftware.Text.Implementation;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Stores;

internal class DocumentStore(IUiCoreServices uiCoreServices) : BindableObject(uiCoreServices), IWriteDocumentStore
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
        UIDispatcher.RunOnUiThread(() =>
        {
            _documents[filePath] = result;
        });

        return result;
    }

    public void ClearDocuemntStore()
    {
        _documents.Clear();
    }
}
