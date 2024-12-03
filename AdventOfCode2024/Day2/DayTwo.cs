using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2024.Day2;

public class DayTwo : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider
            .GetAllLines()
            .Map(input => input.Split(" ").ToSeq())
            .Map(x => x.Map(int.Parse));

        var puzzleOneResult = parsedData
            .Filter(SolveRow)
            .Count;

        yield return new PuzzleResult(puzzleOneResult);

        var puzzleTwoResult = parsedData
            .Filter(
                x => Prelude
                    .Range(0, x.Count)
                    .Map(
                        i =>
                        {
                            var toCompute = x
                                .Index()
                                .Filter(tuple => tuple.Index != i)
                                .Map(y => y.Item)
                                .ToSeq();

                            return SolveRow(toCompute);
                        })
                    .Find(isMatch => isMatch)
                    .IsSome)
            .Count;

        yield return new PuzzleResult(puzzleTwoResult);
    }

    private static bool SolveRow(Seq<int> input)
    {
        var sign = Math.Sign(input.Head - input.Tail.Head);
        if (sign == 0)
        {
            return false;
        }

        return input.Tail.FoldWhile(
                (Previous: input.Head, Sign: sign, IsValid: true),
                (acc, current) => (
                    current,
                    acc.Sign,
                    Math.Max(current, acc.Previous) - Math.Min(current, acc.Previous) <= 3
                    && Math.Sign(acc.Previous - current) == acc.Sign),
                acc => acc.IsValid)
            .Item3;
    }
}