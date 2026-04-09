namespace Blueprint.Abstractions.Infrastructure;

public interface IDirectoryInfoProvider
{
    IEnumerable<DirectoryInfo> GetDirectoriesSafely(DirectoryInfo directory);

    IEnumerable<FileInfo> GetFilesSafely(DirectoryInfo directory);
}
