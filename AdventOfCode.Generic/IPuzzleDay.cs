namespace AdventOfCode.Generic;

public interface IPuzzleDay
{
    int DayNumber { get; }

    IEnumerable<PuzzleResult> PuzzleResults();
}