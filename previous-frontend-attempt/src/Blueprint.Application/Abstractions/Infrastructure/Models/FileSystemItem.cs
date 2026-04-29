namespace Blueprint.Application.Abstractions.Infrastructure.Models;

public readonly record struct FileSystemItem
{
    public required string Name { get; init; }

    public required FileSystemItemKind Kind { get; init; }

    public required string FullPath { get; init; }

    public required IReadOnlyList<FileSystemItem> SubItems { get; init; }

    public bool HasChildren => Kind == FileSystemItemKind.Directory
                               && SubItems.Count > 0;

    public int TotalDescendants =>
        SubItems.Count + SubItems.Sum(c => c.TotalDescendants);

    public string Extension => Kind == FileSystemItemKind.File
        ? Path.GetExtension(Name)
        : string.Empty;
}
