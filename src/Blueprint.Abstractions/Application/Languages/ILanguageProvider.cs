namespace Blueprint.Abstractions.Application.Languages;

public interface ILanguageProvider
{
    object GetLanguage(SupportedLanguages language);
}
