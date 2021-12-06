namespace AdventOfCode2021;

public record PuzzleResult(int PuzzleNumber, string PuzzleAnswer)
{
    public PuzzleResult(int puzzleNumber, int puzzleAnswer)
        : this(puzzleNumber, puzzleAnswer.ToString())
    {
    }
}