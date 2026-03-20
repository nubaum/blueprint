namespace Blueprint.Application.ViewModelContracts.Models;

public record ProjectInfo
{
    public required string ProjectFullPath { get; init; }

    public required string ProjectFolder { get; init; }

    public required string ProjectName { get; init; }
}
