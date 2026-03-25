using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Application.Services;
using MediatR;

namespace Blueprint.Application.Handlers.Workspace;

internal class OpenProjectRequestHandler(WorkspaceService workspaceService, IViewNavigationHost viewNavigationHost) : IRequestHandler<OpenProjectRequest>
{
    public async Task Handle(OpenProjectRequest request, CancellationToken cancellationToken)
    {
        await workspaceService.OpenProjectAsync(request.ProjectFilePath);
        viewNavigationHost.NavigateToCode();
    }
}
