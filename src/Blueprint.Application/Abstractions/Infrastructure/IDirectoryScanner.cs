using Blueprint.Application.Abstractions.Infrastructure.Models;

namespace Blueprint.Application.Abstractions.Infrastructure;

public interface IDirectoryScanner
{
    FileSystemItem Scan(FileSystemScanOptions options, CancellationToken cancellationToken = default);
}
