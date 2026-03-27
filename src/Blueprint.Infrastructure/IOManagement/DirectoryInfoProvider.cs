using Blueprint.Abstractions.Infrastructure;

namespace Blueprint.Infrastructure.IOManagement;

internal class DirectoryInfoProvider : IDirectoryInfoProvider
{
    public IEnumerable<DirectoryInfo> GetDirectoriesSafely(DirectoryInfo directory)
    {
        try
        {
            return directory.GetDirectories();
        }
        catch (UnauthorizedAccessException)
        {
            return [];
        }
        catch (DirectoryNotFoundException)
        {
            return [];
        }
        catch (IOException)
        {
            return [];
        }
    }

    public IEnumerable<FileInfo> GetFilesSafely(DirectoryInfo directory)
    {
        try
        {
            return directory.GetFiles();
        }
        catch (UnauthorizedAccessException)
        {
            return [];
        }
        catch (DirectoryNotFoundException)
        {
            return [];
        }
        catch (IOException)
        {
            return [];
        }
    }
}
