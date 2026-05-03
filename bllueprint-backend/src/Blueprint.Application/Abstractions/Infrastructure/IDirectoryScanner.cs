using Bllueprint.Application.Abstractions.Infrastructure.Models;

namespace Bllueprint.Application.Abstractions.Infrastructure;

public interface IDirectoryScanner
{
    Task<FileSystemItem> ScanAsync(FileSystemScanOptions options, CancellationToken cancellationToken = default);
}
