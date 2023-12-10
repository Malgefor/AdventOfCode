using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2022.Day2;

public class DayTwo : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var input = GetParsedInput();

        var puzzleOneAnswer = input.Map(CalculateScore).Sum();

        var puzzleTwoAnswer = input.Map(ToExpectedHand).Map(CalculateScore).Sum();

        return new[] { new PuzzleResult(puzzleOneAnswer), new PuzzleResult(puzzleTwoAnswer) };
    }

    private static Prediction ToExpectedHand(Prediction x)
    {
        var yourHand = x switch
        {
            { Opponent: 'A', Response: 'X' } => 'Z',
            { Opponent: 'A', Response: 'Y' } => 'X',
            { Opponent: 'A', Response: 'Z' } => 'Y',
            { Opponent: 'B', Response: 'X' } => 'X',
            { Opponent: 'B', Response: 'Y' } => 'Y',
            { Opponent: 'B', Response: 'Z' } => 'Z',
            { Opponent: 'C', Response: 'X' } => 'Y',
            { Opponent: 'C', Response: 'Y' } => 'Z',
            { Opponent: 'C', Response: 'Z' } => 'X'
        };

        return new Prediction(x.Opponent, yourHand);
    }

    private static int CalculateScore(Prediction prediction)
    {
        var chosenHandScore = prediction.Response switch
        {
            'X' => 1,
            'Y' => 2,
            'Z' => 3
        };

        var resultScore = prediction switch
        {
            { Opponent: 'A', Response: 'Y' } => 6,
            { Opponent: 'A', Response: 'Z' } => 0,
            { Opponent: 'B', Response: 'X' } => 0,
            { Opponent: 'B', Response: 'Z' } => 6,
            { Opponent: 'C', Response: 'X' } => 6,
            { Opponent: 'C', Response: 'Y' } => 0,
            _ => 3
        };

        return chosenHandScore + resultScore;
    }

    private static Seq<Prediction> GetParsedInput()
    {
        return FileProvider
            .GetAllLines()
            .Map(x => x.Split(" ").Filter(y => !string.IsNullOrWhiteSpace(y)).Select(y => y.First()).ToSeq())
            .Map(x => new Prediction(x.First(), x.Last()));
    }

    private struct Prediction
    {
        public Prediction(char opponent, char response)
        {
            this.Opponent = opponent;
            this.Response = response;
        }

        public char Opponent { get; }

        public char Response { get; }
    }
}