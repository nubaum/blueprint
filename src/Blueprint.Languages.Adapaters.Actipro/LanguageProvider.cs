using Blueprint.Abstractions.Application.Languages;
using Blueprint.Languages.Adapaters.Actipro.BlueLang;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Languages.Adapaters.Actipro;

public sealed class LanguageProvider : ILanguageProvider
{
    private readonly Dictionary<SupportedLanguages, LanguageDescriptor> _languages;

    public LanguageProvider(IServiceProvider serviceProvider)
    {
        _languages = new Dictionary<SupportedLanguages, LanguageDescriptor>
        {
            [SupportedLanguages.Blue] = new LanguageDescriptor
            {
                Language = SupportedLanguages.Blue,
                Name = "Blue",
                Instance = serviceProvider.GetRequiredService<BlueSyntaxLanguage>()
            }
        };
    }

    public object GetLanguage(SupportedLanguages language)
    {
        return !_languages.TryGetValue(language, out LanguageDescriptor? descriptor)
            ? throw new NotSupportedException($"Language '{language}' is not supported.")
            : descriptor.Instance;
    }
}
