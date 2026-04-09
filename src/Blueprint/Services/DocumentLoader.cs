using System.IO;
using ActiproSoftware.Text;
using ActiproSoftware.Text.Implementation;
using Blueprint.Abstractions.Application.Languages;
using Blueprint.Application.Abstractions;

namespace Blueprint.Services;

internal class DocumentLoader(ILanguageProvider languageProvider) : IDocumentLoader
{
    public object LoadDocument(string filePath)
    {
        EditorDocument doc = new();
        string docExtension = Path.GetExtension(filePath);
        doc.Language = languageProvider.GetLanguageByExtension<ISyntaxLanguage>(docExtension);
        doc.LoadFile(filePath);
        return doc;
    }
}
