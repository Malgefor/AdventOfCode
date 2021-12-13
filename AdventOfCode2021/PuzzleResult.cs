using System.Globalization;

namespace AdventOfCode2021;

public record PuzzleResult(int PuzzleNumber, string PuzzleAnswer)
{
    public PuzzleResult(int puzzleNumber, int puzzleAnswer)
        : this(puzzleNumber, puzzleAnswer.ToString())
    {
    }

    public PuzzleResult(int puzzleNumber, double puzzleAnswer)
        : this(puzzleNumber, puzzleAnswer.ToString(CultureInfo.InvariantCulture))
    {
    }

    public PuzzleResult(int puzzleNumber, long puzzleAnswer)
        : this(puzzleNumber, puzzleAnswer.ToString(CultureInfo.InvariantCulture))
    {
    }
}