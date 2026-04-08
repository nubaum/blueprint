using System.Globalization;
using Blueprint.Abstractions.Application.Workspace.Models;

namespace Blueprint.Abstractions;

public static class StringProvider
{
    public static class Application
    {
        public const string SourceFolderName = "source";
        public const string LogFilePrefix = "Blueprint.Diagnostics";

        public static class Messages
        {
            public const string InvalidNotificationKindConst = "Notification kind is invalid: {0}";
            public const string InvalidDocument = "Document isn't a valid EditorDocument";
            public const string InvalidLanguageConst = "Language is not supported: {0}";
            public const string UnknownExtensionConst = "Extension is not supported: {0}";
            public const string DocumentNotFoundConst = "Document not found: {0}";
            private const string FileAlreadyExistsConst = "File already exists:{0}";

            public static string FileAlreadyExists(string path) => string.Format(CultureInfo.InvariantCulture, FileAlreadyExistsConst, path);

            public static string DocumentNotFound(string path) => string.Format(CultureInfo.InvariantCulture, FileAlreadyExistsConst, path);

            public static string InvalidLanguage(string language) => string.Format(CultureInfo.InvariantCulture, InvalidLanguageConst, language);

            public static string UnknownExtension(string extension) => string.Format(CultureInfo.InvariantCulture, UnknownExtensionConst, extension);

            public static string InvalidNotificationKind(NotificationKind notificationKind) => string.Format(CultureInfo.InvariantCulture, InvalidNotificationKindConst, notificationKind);
        }
    }

    public static class ErrorMessages
    {
        public const string InvalidFolderToOpenDocument = "There is no open project to open a document";
    }
}
