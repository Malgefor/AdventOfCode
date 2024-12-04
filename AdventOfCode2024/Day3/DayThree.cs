using System.Text.RegularExpressions;

using AdventOfCode.Generic;

namespace AdventOfCode2024.Day3;

public class DayThree : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = FileProvider.GetAllLines(string.Empty);

        var regexPartOne = new Regex(@"mul\(([0-9]+)\,([0-9]+)\)");
        var puzzleOneResult = parsedInput
            .Map(input => regexPartOne.Matches(input).ToSeq())
            .Bind(x => x.Map(y => double.Parse(y.Groups[1].Value) * double.Parse(y.Groups[2].Value)))
            .Sum();

        yield return new PuzzleResult(puzzleOneResult);

        var regexPartTwo = new Regex(@"mul\(([0-9]+)\,([0-9]+)\)|do\(\)|don't\(\)");
        var puzzleTwoResult = parsedInput
            .Map(input => regexPartTwo.Matches(input).ToSeq())
            .Map(
                m => m.Fold(
                    (Score: (double)0, Flag: true),
                    (acc, current) => current.Groups[0].Value switch
                    {
                        "do()" => (acc.Score, true),
                        "don't()" => (acc.Score, false),
                        _ => (
                            acc.Flag
                                ? acc.Score + double.Parse(current.Groups[1].Value)
                                * double.Parse(current.Groups[2].Value)
                                : acc.Score,
                            acc.Flag)
                    }))
            .Sum(x => x.Score);

        yield return new PuzzleResult(puzzleTwoResult);
    }
}