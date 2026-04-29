namespace Blueprint.Application.Abstractions.Infrastructure.Models;

public readonly record struct FileSystemScanOptions
{
    public required int DegreeOfParallelism { get; init; }

    public required string RootPath { get; init; }
}
