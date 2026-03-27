namespace Blueprint.Abstractions.Application.Workspace;

public interface IProjectTreeNode
{
    string Name { get; }

    string FullPath { get; }

    string? Extension { get; }

    bool IsFolder { get; }

    IReadOnlyCollection<IProjectTreeNode> Children { get; }

    bool HasChildren { get; }
}
