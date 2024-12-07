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
            .Map(x => new OrderingRule(int.Parse(x[0]), int.Parse(x[1])))
            .ToSeq();

        var updates = parsedData.Tail[0]
            .Split("\r\n")
            .Map(y => y.Split(',').Map(int.Parse).ToArray())
            .ToSeq();

        var middleValues =
            from update in updates
            let isCorrect = orderingRules
                .Filter(x => update.Contains(x.First) && update.Contains(x.Second))
                .All(x => Array.IndexOf(update, x.First) < Array.IndexOf(update, x.Second))
            where isCorrect
            select update[update.Length / 2];

        yield return new PuzzleResult(middleValues.Sum());
    }

    private record OrderingRule(int First, int Second);
}