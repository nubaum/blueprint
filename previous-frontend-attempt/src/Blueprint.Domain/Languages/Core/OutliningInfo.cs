namespace Blueprint.Domain.Languages.Core;

public class OutliningInfo
{
    public int? StartOffset { get; private set; }

    public int? EndOffset { get; private set; }

    public OutliningInfo SetStartOffset(int? offset)
    {
        StartOffset = offset;
        return this;
    }

    public OutliningInfo SetEndOffset(int? offset)
    {
        EndOffset = offset;
        return this;
    }
}
