using Blueprint.Application.ViewModelContracts.Models;

namespace Blueprint.Application.ViewModelContracts;

public interface IReadWorkspaceStore
{
    public ProjectInfo? CurrentProject { get; }

    public IReadOnlyCollection<IWorkspaceItem> OpenItems { get; }
}
