using MediatR;

namespace Blueprint.Application.Requests;

public record OpenDocumentRequest : IRequest
{
    public required string FileName { get; init; }
}
