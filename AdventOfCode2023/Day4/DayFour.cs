using System.Text.RegularExpressions;

using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2023.Day4;

public class DayFour : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();

        var puzzleOneAnswer = parsedInput
            .Map(card => card.CardNumbers.Where(number => card.WinningNumbers.Contains(number)))
            .Filter(x => !x.IsEmpty)
            .Map(winningNumbersOnCard => Math.Pow(2, winningNumbersOnCard.Count - 1))
            .Sum();

        yield return new PuzzleResult(puzzleOneAnswer);

        var puzzleTwoAnswer = parsedInput
            .Fold(
                parsedInput.ToDictionary(x => x.Id, _ => 1),
                (acc, card) =>
                {
                    var amountOfWinningNumbers = card.CardNumbers.Count(number => card.WinningNumbers.Contains(number));

                    Enumerable
                        .Range(1, amountOfWinningNumbers)
                        .Filter(x => x < parsedInput.Count)
                        .Iter(offSet => acc[card.Id + offSet] += acc[card.Id]);

                    return acc;
                })
            .Sum(x => x.Value);

        yield return new PuzzleResult(puzzleTwoAnswer);
    }

    private record ScratchCard(int Id, Seq<int> WinningNumbers, Seq<int> CardNumbers);

    private static Seq<ScratchCard> GetParsedInput() => FileProvider
        .GetAllLines()
        .Map(
            line =>
            {
                var idSplit = line.Split(':');
                var numberSplit = idSplit[1].Split('|');

                const string pattern = @"\b\d+\b";
                return new ScratchCard(
                    int.Parse(idSplit[0][5..].Trim()),
                    Regex.Matches(numberSplit[0], pattern).ToSeq().Map(x => int.Parse(x.Value)),
                    Regex.Matches(numberSplit[1], pattern).ToSeq().Map(x => int.Parse(x.Value)));
            })
        .ToSeq();
}