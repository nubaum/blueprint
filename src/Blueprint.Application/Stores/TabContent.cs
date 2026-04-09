using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Application.Core;

namespace Blueprint.Views.Models;

public class TabContent : BindableObject, IWorkspaceItem
{
    public required WorkspaceItemKind Kind { get; init; }

    public required string Caption { get; init; }

    public required object Content { get; init; }

    public object? Icon { get; init; }

    public bool IsDirty
    {
        get;
        set => SetField(ref field, value);
    }

    public bool IsPinned
    {
        get;
        set => SetField(ref field, value);
    }
}
