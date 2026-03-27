using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Application.Services;
using MediatR;

namespace Blueprint.Application.Handlers.Workspace;

internal class OpenDocumentRequestHandler(WorkspaceService workspaceService) : IRequestHandler<OpenDocumentRequest>
{
    public async Task Handle(OpenDocumentRequest request, CancellationToken cancellationToken)
    {
        await workspaceService.OpenDocumentAsync(request.FileName);
    }
}
