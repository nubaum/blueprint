using Blueprint.Application.ViewModelContracts.Enums;

namespace Blueprint.Application.ViewModelContracts.Models;

public record WorkspaceItem
{
    public WorkspaceItemKind Kind { get; init; }

    public required string Caption { get; init; }

    public required object Content { get; init; }
}
