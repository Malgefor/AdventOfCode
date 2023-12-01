using AdventOfCode.Generic;

namespace AdventOfCode2021.Day2;

public class DayTwo : IPuzzleDay
{
    public int DayNumber => 2;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var instructions = GetParsedInput();

        yield return GetPuzzleOneResult(instructions);
        yield return GetPuzzleTwoResult(instructions);
    }

    private static PuzzleResult GetPuzzleOneResult(IList<IInstruction> instructions)
    {
        var (horizontalPosition, depth) = instructions.Aggregate(
            (0, 0),
            ((double HorizontalPosition, double Depth) position, IInstruction instruction) =>
                instruction.Match(
                    up => (position.HorizontalPosition, position.Depth - up.Change),
                    down => (position.HorizontalPosition, position.Depth + down.Change),
                    forward => (position.HorizontalPosition + forward.Change, position.Depth)));

        return new PuzzleResult(1, horizontalPosition * depth);
    }

    private static PuzzleResult GetPuzzleTwoResult(IList<IInstruction> instructions)
    {
        var (horizontalPosition, depth, _) = instructions.Aggregate(
            (0, 0, 0),
            ((double HorizontalPosition, double Depth, double Aim) position, IInstruction instruction) =>
                instruction.Match(
                    up => (position.HorizontalPosition, position.Depth, position.Aim - up.Change),
                    down => (position.HorizontalPosition, position.Depth, position.Aim + down.Change),
                    forward => (
                        position.HorizontalPosition + forward.Change,
                        position.Depth + forward.Change * position.Aim,
                        position.Aim
                    )));

        return new PuzzleResult(2, horizontalPosition * depth);
    }

    private static IList<IInstruction> GetParsedInput()
    {
        return FileProvider
            .GetAllLines("Day2.input.txt")
            .Select(
                stringValue =>
                {
                    var split = stringValue.Split(" ");
                    var direction = split[0];
                    var change = double.Parse(split[1]);

                    return direction switch
                    {
                        "forward" => IInstructionCon.Forward(change),
                        "down" => IInstructionCon.Down(change),
                        "up" => IInstructionCon.Up(change)
                    };
                })
            .ToList();
    }
}