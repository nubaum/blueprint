using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;
using MediatR;

namespace Blueprint.Presentation.ViewModels.Pages;

internal class CodeViewModel : NotifyPropertyChangedBase, ICodeViewModel
{
    public CodeViewModel(IMediator mediator, IReadWorkspaceStore readWorkspaceStore)
    {
        WorkspaceStore = readWorkspaceStore;
        FireForget.RunAndCrashOnUiThread(async () =>
        {
            await mediator.Send(new OpenProjectRequest { ProjectFilePath = @"C:\Users\albin\source\repos\UAMConsulting\Personal\Blueprint\Directory.Build.props" });
            await mediator.Send(new OpenDocumentRequest { FileName = "Test" });
        });
    }

    public IReadWorkspaceStore WorkspaceStore { get; }
}
