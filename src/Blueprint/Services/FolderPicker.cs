using Blueprint.Application.Abstractions.Workspace;
using Microsoft.Win32;

namespace Blueprint.Services;

public sealed class FolderPicker(IPathProvider pathProvider) : IFolderPicker
{
    public async Task<string?> PickFolderAsync()
    {
        var dialog = new OpenFolderDialog
        {
            InitialDirectory = pathProvider.SourceFolderPath
        };

        if(dialog.ShowDialog() ?? false)
        {
            return dialog.FolderName;
        }

        return null;
    }
}
