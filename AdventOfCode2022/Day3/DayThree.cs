using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2022.Day3;

public class DayThree : IPuzzleDay
{
    public int DayNumber => 3;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var charArrayInput = GetParsedInput()
            .Map(x => x.ToCharArray());

        var puzzleOneAnswer = charArrayInput
            .Map(x => x.Chunk(x.Length / 2).ToSeq())
            .Map(x => x.First().Intersect(x.Last()).First())
            .Map(GetValue)
            .Sum();

        var puzzleTwoAnswer = charArrayInput
            .Chunk(3)
            .Map(x => x[0].Intersect(x[1]).Intersect(x[2]).First())
            .Map(GetValue)
            .Sum();

        return new[] { new PuzzleResult(puzzleOneAnswer), new PuzzleResult(puzzleTwoAnswer) };
    }

    private static readonly List<char> Priorities = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();

    private static int GetValue(char value)
    {
        return Priorities.IndexOf(value) + 1;
    }

    private static Seq<string> GetParsedInput()
    {
        return FileProvider
            .GetAllLines();
    }
}