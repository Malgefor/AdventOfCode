namespace AdventOfCode2021;

public interface IPuzzleDay
{
    int DayNumber { get; }

    IEnumerable<PuzzleResult> PuzzleResults();
}