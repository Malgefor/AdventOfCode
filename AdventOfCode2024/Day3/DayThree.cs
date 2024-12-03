using System.Text.RegularExpressions;

using AdventOfCode.Generic;

namespace AdventOfCode2024.Day3;

public class DayThree : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var mulRegex = new Regex("mul\\(([0-9]+)\\,([0-9]+)\\)");
        var parsedInput = FileProvider
            .GetAllLines();

        var puzzleOneResult = parsedInput
            .Map(input => mulRegex.Matches(input).ToSeq())
            .Bind(x => x.Map(y => double.Parse(y.Groups[1].Value) * double.Parse(y.Groups[2].Value)))
            .Sum();

        yield return new PuzzleResult(puzzleOneResult);
    }
}