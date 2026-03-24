using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Application.Services;
using MediatR;

namespace Blueprint.Application.Handlers.Workspace;

internal class OpenProjectRequestHandler(WorkspaceService workspaceService) : IRequestHandler<OpenProjectRequest>
{
    public async Task Handle(OpenProjectRequest request, CancellationToken cancellationToken)
    {
        await workspaceService.OpenProjectAsync(request.ProjectFilePath);
    }
}
