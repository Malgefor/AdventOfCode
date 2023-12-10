using System.Text.RegularExpressions;

using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2023.Day5;

public class DayFive : IPuzzleDay
{
    public int DayNumber => 5;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();
        yield return new PuzzleResult(1);
    }

    private record AlmanacRangesRanges(int DestinationRangeStart, int SourceRangeStart, int RangeLength);

    private static (Seq<int> Seeds, Seq<Seq<AlmanacRangesRanges>> AlmanacEntries) GetParsedInput()
    {
        const string pattern = @"\b\d+\b";

        var allLines = FileProvider.GetAllLines("\r\n\r\n");
        var seeds = Regex.Matches(allLines.Head, pattern).ToSeq().Map(x => int.Parse(x.Value));

        return (seeds, allLines.Tail.Map(ParseAlmanacEntries));
    }

    private static Seq<AlmanacRangesRanges> ParseAlmanacEntries(string groupOfLines) => groupOfLines
        .Split("\r\n")
        .Tail()
        .Map(line => line.Split(' '))
        .Map(
            splitLine => new AlmanacRangesRanges(
                int.Parse(splitLine[0]),
                int.Parse(splitLine[1]),
                int.Parse(splitLine[2])))
        .ToSeq();
}