using Blueprint.Application.Abstractions.Infrastructure;
using Blueprint.Application.Abstractions.Infrastructure.Models;
using MediatR;

namespace Blueprint.Application.Messages.FolderManagement;

public readonly record struct GetFolderStructureRequest : IRequest<FileSystemItem>
{
    public required string RootPath { get; init; }
}

public class GetFolderStructureRequestHandler(IDirectoryScanner directoryScanner) : IRequestHandler<GetFolderStructureRequest, FileSystemItem>
{
    public async Task<FileSystemItem> Handle(GetFolderStructureRequest request, CancellationToken cancellationToken)
    {
        FileSystemScanOptions options = new()
        {
            RootPath = request.RootPath,
            DegreeOfParallelism = 0
        };

        return await directoryScanner.ScanAsync(options, cancellationToken);
    }
}
