namespace Blueprint.Domain.Languages.Core;

public sealed record TextSpan(int StartOffset)
{
    public int EndOffset { get; private set; }

    public int EndOfLineOffset { get; private set; }

    public int Length => EndOffset - StartOffset;

    public TextSpan SetEndOffset(int endOffset)
    {
        EndOffset = endOffset;
        return this;
    }

    public void SetEndOfLineOffset(int endOffset) => EndOfLineOffset = endOffset;
}
