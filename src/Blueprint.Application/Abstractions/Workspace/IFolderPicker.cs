namespace Blueprint.Abstractions.Application.Workspace;

public interface IFolderPicker
{
    Task<string?> PickFolderAsync();
}
