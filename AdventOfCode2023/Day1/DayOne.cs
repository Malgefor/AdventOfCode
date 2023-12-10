using AdventOfCode.Generic;

using Humanizer;

using LanguageExt;

namespace AdventOfCode2023.Day1;

public class DayOne : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var resultPuzzleOne = GetParsedInput()
            .Map(input => $"{input.First(char.IsNumber)}{input.Last(char.IsNumber)}")
            .Map(int.Parse)
            .Sum();

        yield return new PuzzleResult(resultPuzzleOne);

        var parsedInput = GetParsedInput();

        var resultPuzzleTwo = parsedInput
            .Map(
                input => (input
                        .Fold(
                            string.Empty,
                            (accumulate, currentChar) => accumulate.Any(char.IsNumber)
                                ? accumulate
                                : NumbersAsStrings
                                    .Fold(
                                        accumulate + currentChar,
                                        (current, number) => current.Contains(number.Item1)
                                            ? current.Replace(number.Item1, number.Item2.ToString())
                                            : current)),
                    input
                        .ReverseString() // 7ine
                        .Fold(
                            string.Empty,
                            (accumulate, currentChar) =>
                                accumulate.Any(char.IsNumber)
                                    ? accumulate
                                    : NumbersAsStrings
                                        .Fold(
                                            accumulate + currentChar,
                                            (current, number) => current.ReverseString().Contains(number.Item1)
                                                ? current.ReverseString()
                                                    .Replace(number.Item1, number.Item2.ToString())
                                                    .ReverseString()
                                                : current))))
            .Map(input => $"{input.Item1.First(char.IsNumber)}{input.Item2.First(char.IsNumber)}")
            .Map(int.Parse)
            .Sum();

        yield return new PuzzleResult(resultPuzzleTwo);
    }

    private static readonly (string, int)[] NumbersAsStrings = Enumerable
        .Range(0, 10)
        .Map(number => (number.ToWords(false).Replace(" ", string.Empty), number))
        .ToArray();

    private static Seq<string> GetParsedInput() => FileProvider
        .GetAllLines();
}