namespace Blueprint.Domain.Languages.Core;

public abstract class Ast
{
    private readonly List<OutliningInfo> _outlines = [];

    private readonly List<ICustomClassificationSpan> _references = [];

    private readonly List<ParseErrorInfo> _errors = [];

    public IEnumerable<OutliningInfo> OutliningNodes => _outlines;

    public IReadOnlyList<ParseErrorInfo> Errors => _errors;

    public IReadOnlyList<ICustomClassificationSpan> References => _references;

    public OutliningInfo? LastOpenOutliningInfo
        => _outlines.Count > 0 ? _outlines.LastOrDefault(o => o.EndOffset == default) : default;

    public void AddOutliningNode(int offset)
        => _outlines.Add(new OutliningInfo().SetStartOffset(offset));

    public void SortTextSpans()
    {
        TextSpanProviderHelper.SortSpans(_errors);
        TextSpanProviderHelper.SortSpans(_references);
    }

    public void ClearPreviousSemanticAnalysis()
    {
        _references.Clear();
    }

    protected void AddOutliningInfoRange(IEnumerable<OutliningInfo> outliningInfos)
    {
        _outlines.AddRange(outliningInfos);
    }

    protected void ClearSemanticErrors()
    {
        _errors.RemoveAll(o => o.Type == ErrorKind.Semantic);
    }

    protected void AddErrors(IEnumerable<ParseErrorInfo> errors) => _errors.AddRange(errors);

    protected void AddError(ParseErrorInfo error) => _errors.Add(error);
}
