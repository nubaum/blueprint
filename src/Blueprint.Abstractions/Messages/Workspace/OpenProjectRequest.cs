using MediatR;

namespace Blueprint.Abstractions.Messages.Workspace;

public record OpenProjectRequest : IRequest
{
    public required string ProjectFilePath { get; init; }
}
