using System.Text.RegularExpressions;

using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2023.Day6;

public class DaySix : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();
        var puzzleOneAnswer = parsedInput
            .Map(
                game => Enumerable
                    .Range(0, game.Head)
                    .CalculateWinningOptions(game.Head, game.Tail.Head))
            .Reduce((n1, n2) => n1 * n2);

        yield return new PuzzleResult(puzzleOneAnswer);

        var theGame = parsedInput
            .Fold(
                (string.Empty, string.Empty),
                (strings, game) => (strings.Item1 + game.Head, strings.Item2 + game.Tail.Head));

        var gameLength = long.Parse(theGame.Item1);
        var gameRecord = long.Parse(theGame.Item2);

        var puzzleAnswerTwo = LongExtensions
            .RangeLong(0, gameLength)
            .CalculateWinningOptions(gameLength, gameRecord);

        yield return new PuzzleResult(puzzleAnswerTwo);
    }

    private static Seq<Seq<int>> GetParsedInput()
    {
        var numbers = FileProvider
            .GetAllLines()
            .Map(
                line =>
                {
                    var idSplit = line.Split(':');

                    const string pattern = @"\b\d+\b";
                    return Regex.Matches(idSplit[1], pattern).Map(x => int.Parse(x.Value)).ToSeq();
                });

        return numbers
            .Fold(
                new List<List<int>>(Enumerable.Range(0, numbers.Head.Count).Map(_ => new List<int>())),
                (acc, line) => { return line.Map((index, number) => acc[index].Append(number).ToList()).ToList(); })
            .Map(x => x.ToSeq())
            .ToSeq();
    }
}