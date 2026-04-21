namespace Blueprint.Application.Abstractions.Workspace;

public interface IFolderPicker
{
    Task<string?> PickFolderAsync();
}
