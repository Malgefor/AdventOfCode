using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2021.Day7;

public class DaySeven : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var horizontalCrabPositions = GetParsedInput();

        yield return GetPuzzleOneResult(horizontalCrabPositions);
        yield return GetPuzzleTwoResult(horizontalCrabPositions);
    }

    private static PuzzleResult GetPuzzleOneResult(Seq<int> horizontalCrabPositions)
    {
        var median = horizontalCrabPositions
            .OrderBy(position => position)
            .Skip(
                horizontalCrabPositions
                    .Count() / 2)
            .First();

        var totalFuelCost = horizontalCrabPositions
            .Sum(position => Math.Abs(median - position));

        return new PuzzleResult(totalFuelCost);
    }

    private static PuzzleResult GetPuzzleTwoResult(Seq<int> horizontalCrabPositions)
    {
        var positionAverage = horizontalCrabPositions
            .Average(i => i);

        var averageCeiling = (int)Math.Ceiling(
            positionAverage);

        var averageFloor = (int)Math.Floor(
            positionAverage);

        var sumOfDistancesToBestPosition = Enumerable
            .Range(averageFloor - 5, averageCeiling - averageFloor + 5)
            .Map(
                canidatePosition =>
                {
                    return horizontalCrabPositions
                        .Sum(
                            position =>
                            {
                                var steps = Math.Abs(canidatePosition - position);
                                return (steps * steps + steps) / 2;
                            });
                })
            .Min();

        return new PuzzleResult(sumOfDistancesToBestPosition);
    }

    private static Seq<int> GetParsedInput()
    {
        return FileProvider
            .GetAllLines(",")
            .Map(int.Parse)
            .ToSeq();
    }
}