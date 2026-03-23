using Blueprint.Application.ViewModelContracts.Enums;

namespace Blueprint.Application.ViewModelContracts.Models;

public interface IWorkspaceItem
{
    public string Caption { get; }

    public object Content { get; }

    public object? Icon { get; }

    public bool IsDirty { get; set; }

    public bool IsPinned { get; set; }

    public WorkspaceItemKind Kind { get; }
}
