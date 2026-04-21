namespace Blueprint.Domain.Languages.Core;

public interface ICustomClassificationSpan : ITextSpanProvider
{
    public ClassificationKind Classificaition { get; }
}
