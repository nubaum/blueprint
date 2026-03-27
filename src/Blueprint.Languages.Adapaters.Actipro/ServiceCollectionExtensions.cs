using Blueprint.Abstractions.Application.Languages;
using Blueprint.Languages.Adapaters.Actipro.BlueLang;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Languages.Adapaters.Actipro;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureLanguages(this IServiceCollection services)
    {
        services.AddSingleton<BlueSyntaxLanguage>();
        services.AddSingleton<ILanguageProvider, LanguageProvider>();
        return services;
    }
}
