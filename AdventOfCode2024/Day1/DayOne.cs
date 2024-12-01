using AdventOfCode.Generic;

namespace AdventOfCode2024.Day1;

public class DayOne : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider
            .GetAllLines()
            .Map(input => input.Split("   ").ToSeq())
            .Map(x => x.Map(int.Parse));

        var listOne = parsedData
            .Map(x => x[0])
            .OrderByDescending(x => x)
            .ToSeq();

        var listTwo = parsedData
            .Map(x => x[1])
            .OrderByDescending(x => x)
            .ToSeq();

        var resultPuzzleOne = listOne
            .Zip(listTwo)
            .Map(tuple => Math.Max(tuple.Item1, tuple.Item2) - Math.Min(tuple.Item1, tuple.Item2))
            .Sum();

        yield return new PuzzleResult(resultPuzzleOne);

        var resultPuzzleTwo = listOne
            .Map(itemOne => itemOne * listTwo.Count(itemTwo => itemTwo == itemOne))
            .Sum();

        yield return new PuzzleResult(resultPuzzleTwo);
    }
}