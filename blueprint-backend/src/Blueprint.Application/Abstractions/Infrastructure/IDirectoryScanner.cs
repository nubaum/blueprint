using Blueprint.Application.Abstractions.Infrastructure.Models;

namespace Blueprint.Application.Abstractions.Infrastructure;

public interface IDirectoryScanner
{
    Task<FileSystemItem> ScanAsync(FileSystemScanOptions options, CancellationToken cancellationToken = default);
}
