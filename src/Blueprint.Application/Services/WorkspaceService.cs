using Blueprint.Abstractions.Application.Languages;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.InternalAbstractions;

namespace Blueprint.Application.Services;

internal class WorkspaceService(
        IWriteDocumentStore documentStore,
        PathProvider pathProvider,
        IWriteWorkspaceStore workspaceStore,
        ILanguageProvider languageProvider)
{
    public async Task OpenProjectAsync(string path)
    {
        if (File.Exists(path))
        {
            string folder = Path.GetDirectoryName(path)!;
            string projectName = Path.GetFileNameWithoutExtension(path);
            workspaceStore.ClearItems(o => o.Kind == WorkspaceItemKind.Doucument);
            workspaceStore.SetCurrentProject(new ProjectInfo { ProjectFolder = folder, ProjectName = projectName, ProjectFullPath = path });
        }

        await Task.CompletedTask;
    }

    public async Task OpenDocumentAsync(string fileName)
    {
        object doc = GetOrCreateDocument(fileName);
        object lang = languageProvider.GetLanguage(SupportedLanguages.Blue);
        workspaceStore.AddDocument(fileName, doc, lang);
        await Task.CompletedTask;
    }

    private object GetOrCreateDocument(string fileName)
    {
        string documentPath = pathProvider.GetFullPathOfDocumentName(fileName);
        if (documentStore.GetDocument(documentPath) is object doc)
        {
            return doc;
        }

        return documentStore.CreateDocument(documentPath);
    }
}
