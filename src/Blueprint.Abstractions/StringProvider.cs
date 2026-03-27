namespace Blueprint.Abstractions;

public static class StringProvider
{
    public static class Application
    {
        public const string SourceFolderName = "source";
        public const string LogFilePrefix = "Blueprint.Diagnostics";
    }

    public static class ErrorMessages
    {
        public const string InvalidFolderToOpenDocument = "There is no open project to open a document";
    }
}
