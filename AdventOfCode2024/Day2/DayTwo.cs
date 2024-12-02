using AdventOfCode.Generic;

namespace AdventOfCode2024.Day2;

public class DayTwo : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider
            .GetAllLines()
            .Map(input => input.Split(" ").ToSeq())
            .Map(x => x.Map(int.Parse))
            .Filter(
                x => x.Tail.FoldWhile(
                        (x.Head, Math.Sign(x.Head - x.Tail.Head), true),
                        (acc, current) => (
                            current,
                            acc.Item2,
                            Math.Max(current, acc.Item1) - Math.Min(current, acc.Item1) <= 3
                            && Math.Sign(acc.Item1 - current) == acc.Item2),
                        acc => acc.Item3)
                    .Item3)
            .Count;

        yield return new PuzzleResult(parsedData);
    }
}