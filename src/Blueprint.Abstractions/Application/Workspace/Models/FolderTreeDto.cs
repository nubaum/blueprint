namespace Blueprint.Abstractions.Application.Workspace.Models;

public sealed class FolderTreeDto
{
    public required string Name { get; init; }

    public required string FullPath { get; init; }

    public required bool IsFolder { get; init; }

    public List<FolderTreeDto> Children { get; init; } = [];

    public bool HasChildren => Children.Count > 0;

    public string? Extension { get; init; }
}
