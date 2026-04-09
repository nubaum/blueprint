using MediatR;

namespace Blueprint.Abstractions.Messages.Workspace;

public record OpenDocumentRequest : IRequest
{
    public required string FileName { get; init; }
}
