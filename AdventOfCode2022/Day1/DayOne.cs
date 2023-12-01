using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2022.Day1;

public class DayOne : IPuzzleDay
{
    public int DayNumber => 1;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var summedInput = GetParsedInput().Map(x => x.Sum());

        var puzzleOneAnswer = summedInput.Max();
        var puzzleTwoAnswer = summedInput.OrderByDescending(x => x).Take(3).Sum();

        return new[] { new PuzzleResult(1, puzzleOneAnswer), new PuzzleResult(2, puzzleTwoAnswer) };
    }

    private static Seq<Seq<int>> GetParsedInput()
    {
        var allLines = FileProvider
            .GetAllLines("Day1.input.txt", "\r\n\r\n");
        return allLines
            .Map(x => x.Split("\r\n").Map(int.Parse).ToSeq());
    }
}