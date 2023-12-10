using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2022.Day4;

public class DayFour : IPuzzleDay
{
    public int DayNumber => 4;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var puzzleOneAnswer = GetParsedInput()
            .Filter(
                x =>
                {
                    if (x.First.Start <= x.Second.Start && x.First.End >= x.Second.End)
                    {
                        return true;
                    }

                    if (x.Second.Start <= x.First.Start && x.Second.End >= x.First.End)
                    {
                        return true;
                    }

                    return false;
                })
            .Count;

        var puzzleTwoAnswer = GetParsedInput()
            .Filter(
                x =>
                {
                    if (x.First.Start <= x.Second.Start && x.Second.Start <= x.First.End)
                    {
                        return true;
                    }

                    if (x.Second.Start <= x.First.Start && x.First.Start <= x.Second.End)
                    {
                        return true;
                    }

                    return false;
                })
            .Count;

        return new[] { new PuzzleResult(puzzleOneAnswer), new PuzzleResult(puzzleTwoAnswer) };
    }

    private static Seq<Pair> GetParsedInput()
    {
        return FileProvider
            .GetAllLines()
            .Map(
                x =>
                {
                    var ranges = x.Split(",");
                    var firstRange = CreateRange(ranges[0]);
                    var secondRange = CreateRange(ranges[1]);
                    return new Pair(firstRange, secondRange);
                });
    }

    private static Range CreateRange(string rangeString)
    {
        var rangeSplit = rangeString.Split("-");
        return new Range(int.Parse(rangeSplit[0]), int.Parse(rangeSplit[1]));
    }

    private readonly record struct Pair(Range First, Range Second);

    private readonly record struct Range(int Start, int End);
}