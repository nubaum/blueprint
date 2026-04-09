namespace Blueprint.Abstractions.Application.Languages;

public interface ILanguageProvider
{
    T GetLanguage<T>(KnownLanguage language);

    T GetLanguageByExtension<T>(string extension);
}
