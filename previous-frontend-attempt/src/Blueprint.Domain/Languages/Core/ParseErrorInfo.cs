namespace Blueprint.Domain.Languages.Core;

public record ParseErrorInfo : ITextSpanProvider
{
    public required int Id { get; init; }

    public required string Message { get; init; }

    public required ErrorKind Type { get; init; }

    public required ValidationLevel Level { get; init; }

    public required TextSpan TextSpan { get; init; }
}
