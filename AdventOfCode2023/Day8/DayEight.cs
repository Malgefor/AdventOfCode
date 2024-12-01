using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2023.Day8;

public class DayEight : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();

        var puzzleAnswerOne = Prelude
            .Range(0, int.MaxValue)
            .FoldWhile(
                (Index: -1, Steps: 0, Instruction: parsedInput.First),
                (acc, _) =>
                {
                    var instructionIndex = acc.Index == parsedInput.Directions.Count - 1
                        ? 0
                        : acc.Index + 1;

                    return parsedInput.Directions[instructionIndex] == 'L'
                        ? (instructionIndex, acc.Steps += 1, parsedInput.Instructions[acc.Instruction.Left])
                        : (instructionIndex, acc.Steps += 1, parsedInput.Instructions[acc.Instruction.Right]);
                },
                acc => acc.Instruction.Key != "ZZZ");

        yield return new PuzzleResult(puzzleAnswerOne.Steps);
    }

    private record Instruction(string Key, string Left, string Right);

    private static (Arr<char> Directions, Instruction First, Map<string, Instruction> Instructions) GetParsedInput()
    {
        var allLines = FileProvider.GetAllLines("\r\n\r\n");
        var instructions = allLines[1]
            .Split("\r\n")
            .Map(
                line =>
                {
                    var split = line.Split('=');
                    var key = split[0].Trim();
                    var values = split[1]
                        .Replace("(", string.Empty)
                        .Replace(")", string.Empty)
                        .Split(',');

                    return new Instruction(key, values[0].Trim(), values[1].Trim());
                })
            .ToSeq();

        return (
            allLines[0].Trim().ToArr(),
            instructions.First(x => x.Key == "AAA"),
            instructions.Map(instruction => (instruction.Key, instruction)).ToMap());
    }
}