using AdventOfCode.Generic;

namespace AdventOfCode2024.Day5;

public class DayFive : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider.GetAllLines("\r\n\r\n");
        var orderingRules = parsedData.Head
            .Split("\r\n")
            .Map(x => x.Split("|"))
            .Map(x => new OrderingRule(int.Parse(x[0]), int.Parse(x[1])));
        var updates = parsedData.Tail[0].Split("\r\n");

        yield return new PuzzleResult(1);
    }

    private record OrderingRule(int First, int Second);
}