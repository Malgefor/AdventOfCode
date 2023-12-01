using AdventOfCode.Generic;

namespace AdventOfCode2021.Day1;

public class DayOne : IPuzzleDay
{
    public int DayNumber => 1;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();

        var tripleZips = parsedInput
            .Zip(parsedInput.Skip(1))
            .Zip(parsedInput.Skip(2))
            .Select(tuple => tuple.Item1.Item1 + tuple.Item1.Item2 + tuple.Item2)
            .ToList();

        yield return new PuzzleResult(1, CountGreaterThanPrevious(parsedInput));
        yield return new PuzzleResult(2, CountGreaterThanPrevious(tripleZips));
    }

    private static int CountGreaterThanPrevious(IList<int> inputs)
    {
        return inputs
            .Zip(inputs.Skip(1))
            .Select(tuple => tuple.Item2 > tuple.Item1)
            .Count(boolValue => boolValue);
    }

    private static List<int> GetParsedInput()
    {
        return FileProvider
            .GetAllLines("Day1.input.txt")
            .Select(int.Parse)
            .ToList();
    }
}