using ActiproSoftware.Text;
using Blueprint.Application;
using Blueprint.Application.Abstractions.Languages;
using Blueprint.Languages.Adapaters.Actipro.BlueLang;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Languages;

internal sealed class LanguageProvider : ILanguageProvider
{
    private readonly Dictionary<KnownLanguage, ISyntaxLanguage> _languages;

    private readonly Dictionary<string, KnownLanguage> _languageByExtension =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { ".blue", KnownLanguage.Blue }
        };

    public LanguageProvider(IServiceProvider serviceProvider)
    {
        _languages = new Dictionary<KnownLanguage, ISyntaxLanguage>
        {
            [KnownLanguage.Blue] = serviceProvider.GetRequiredService<BlueSyntaxLanguage>()
        };
    }

    public T GetLanguage<T>(KnownLanguage language)
    {
        // TODO: This should be returning plain text for languages known and unspported.
        if (typeof(T) == typeof(ISyntaxLanguage) && _languages.TryGetValue(language, out ISyntaxLanguage? result))
        {
            return (T)result;
        }

        throw new NotSupportedException(StringProvider.Application.Messages.InvalidLanguage(language.ToString()));
    }

    public T GetLanguageByExtension<T>(string extension)
    {
        if (typeof(T) == typeof(ISyntaxLanguage) && _languageByExtension.TryGetValue(extension, out KnownLanguage lang))
        {
            return GetLanguage<T>(lang);
        }

        throw new NotSupportedException(StringProvider.Application.Messages.UnknownExtension(extension));
    }
}
