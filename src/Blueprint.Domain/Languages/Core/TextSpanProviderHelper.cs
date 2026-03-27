namespace Blueprint.Domain.Languages.Core;

public static class TextSpanProviderHelper
{
    public static void SortSpans(List<ICustomClassificationSpan> textSpans)
    {
        SortTextSpansInternal(textSpans);
    }

    public static void SortSpans(List<ParseErrorInfo> textSpans)
    {
        SortTextSpansInternal(textSpans);
    }

    public static int FindFirstCandidate<T>(IReadOnlyList<T> textSpans, int queryStart)
    where T : ITextSpanProvider
    {
        int lo = 0;
        int hi = textSpans.Count - 1;

        while (lo <= hi)
        {
            int mid = (lo + hi) >>> 1;

            if (textSpans[mid].TextSpan.EndOffset <= queryStart)
            {
                lo = mid + 1;
            }
            else
            {
                hi = mid - 1;
            }
        }

        return lo;
    }

    private static void SortTextSpansInternal<T>(List<T> textSpans)
    where T : ITextSpanProvider
    {
        textSpans.Sort(static (a, b) =>
        {
            TextSpan sa = a.TextSpan;
            TextSpan sb = b.TextSpan;

            int c = sa.StartOffset.CompareTo(sb.StartOffset);

            return c != 0 ? c : sa.EndOffset.CompareTo(sb.EndOffset);
        });
    }
}
