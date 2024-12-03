namespace AdventOfCode2023.Day5;

public static class DoubleExtensions
{
    public static IEnumerable<double> DoubleRange(double startValue, double count)
    {
        for (var d = startValue; d <= startValue + count; d += 1)
            yield return d;
    }
}