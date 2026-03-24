using ActiproSoftware.Text.Implementation;
using Blueprint.Application.InternalAbstractions;

namespace Blueprint.Stores;

internal class DocumentStore : IWriteDocumentStore
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
        System.Windows.Application.Current.Dispatcher.Invoke(() =>
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
