namespace AdventOfCode2023.Day6;

public static class LongExtensions
{
    public static IEnumerable<long> RangeLong(long start, long count)
    {
        for (long i = 0; i < start + count; i++)
            yield return i;
    }
}