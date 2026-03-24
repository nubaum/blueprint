using System.IO;
using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Presentation.ViewModels.Pages;
using Blueprint.Services.Interfaces;
using Blueprint.Views.UserControls;

namespace Blueprint.Services;

internal class WorkspaceService(
    IWriteWorkspaceStore workspaceStore) : IWorkspaceService
{
    public async Task OpenProjectAsync(string path)
    {
        if (File.Exists(path))
        {
            workspaceStore.ClearItems(o => o.Kind == WorkspaceItemKind.Doucument);
        }

        await Task.CompletedTask;
    }

    public async Task OpenDocumentAsync()
    {
        var result = new BlueLangEditor();
        workspaceStore.AddItem(new TabContent { Caption = "MyDoc", Content = result, Kind = WorkspaceItemKind.Doucument });
        await Task.CompletedTask;
    }
}
