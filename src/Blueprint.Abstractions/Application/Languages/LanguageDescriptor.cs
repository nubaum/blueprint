namespace Blueprint.Abstractions.Application.Languages;

public record LanguageDescriptor
{
    public required SupportedLanguages Language { get; init; }

    public required string Name { get; init; }

    public required object Instance { get; init; }
}
