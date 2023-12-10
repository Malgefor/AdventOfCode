using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2021.Day6;

public class DaySix : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var initialLanternFishes = GetParsedInput();

        yield return new PuzzleResult(GetTotalFishesAfterDays(initialLanternFishes, 80));
        yield return new PuzzleResult(GetTotalFishesAfterDays(initialLanternFishes, 256));
    }

    private static long GetTotalFishesAfterDays(Seq<(int DueInDays, long AmountOfFishes)> initialCountsOfFish, int days)
    {
        return Enumerable
            .Repeat(0, days)
            .Fold(
                initialCountsOfFish,
                (countsOfFish, _) => countsOfFish
                    .Map(
                        tuple => tuple.DueInDays == 0
                            ? (6, tuple.AmountOfFishes)
                            : (tuple.DueInDays - 1, tuple.AmountOfFishes))
                    .Append(
                        countsOfFish
                            .Filter(fish => fish.DueInDays == 0)
                            .Match(
                                Seq.empty<(int, long)>,
                                (head, _) => Seq.create((8, head.AmountOfFishes)))))
            .Sum(tuple => tuple.AmountOfFishes);
    }

    private static Seq<(int, long)> GetParsedInput()
    {
        return FileProvider
            .GetAllLines(",")
            .Map(int.Parse)
            .GroupBy(fish => fish)
            .Map(fishes => (fishes.Key, fishes.LongCount()))
            .ToSeq();
    }
}