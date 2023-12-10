using System.Globalization;

namespace AdventOfCode.Generic;

public record PuzzleResult(string PuzzleAnswer)
{
    public PuzzleResult(int puzzleAnswer)
        : this(puzzleAnswer.ToString())
    {
    }

    public PuzzleResult(double puzzleAnswer)
        : this(puzzleAnswer.ToString(CultureInfo.InvariantCulture))
    {
    }

    public PuzzleResult(long puzzleAnswer)
        : this(puzzleAnswer.ToString(CultureInfo.InvariantCulture))
    {
    }
}