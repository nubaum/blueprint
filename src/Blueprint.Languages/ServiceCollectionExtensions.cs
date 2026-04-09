using Blueprint.Application.Abstractions.Languages;
using Blueprint.Languages.Adapaters.Actipro.BlueLang;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Languages;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureLanguages(this IServiceCollection services)
    {
        services.AddSingleton<BlueSyntaxLanguage>();
        services.AddSingleton<ILanguageProvider, LanguageProvider>();
        return services;
    }
}
