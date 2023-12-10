using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2021.Day11;

public class DayEleven : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var octopuses = GetParsedInput();

        var resultPuzzle1 = Seq
            .createRange(Enumerable.Range(1, 100))
            .Fold(
                (octopuses, totalFlashes: 0, step: 0),
                ProcessDay);

        yield return new PuzzleResult(resultPuzzle1.totalFlashes);

        var resultPuzzle2 = Seq
            .createRange(Enumerable.Range(1, int.MaxValue))
            .FoldUntil(
                (octopuses, totalFlashes: 0, step: 0),
                ProcessDay,
                tuple => tuple.octopuses.All(octopus => octopus.EnergyLevel == 0));

        yield return new PuzzleResult(resultPuzzle2.step);
    }

    private static (Seq<Octopus> octopuses, int totalFlashes, int step) ProcessDay(
        (Seq<Octopus> octopuses, int totalFlashes, int step) tuple,
        int step)
    {
        var (currentStateOfOctopuses, totalFlashes, _) = tuple;
        var increasedOctopuses = IncrementEnergyOfOctopuses(currentStateOfOctopuses);

        var roundResult = Seq
            .createRange(Enumerable.Range(1, int.MaxValue))
            .FoldUntil(
                increasedOctopuses,
                (tuple2, _) => ProcessRound(tuple2),
                allOctopuses => allOctopuses.All(octopus => octopus.HasFlashed || octopus.EnergyLevel <= 9));

        var flashedOctopuses = roundResult.Filter(octopus => octopus.HasFlashed);
        var nonFlashedOctopuses = roundResult.Filter(octopus => !octopus.HasFlashed);

        var newTotalOctopuses = flashedOctopuses
            .Map(octopus => octopus with { HasFlashed = false, EnergyLevel = 0 })
            .Concat(nonFlashedOctopuses);

        var totalFlashCountAfterRound = totalFlashes + flashedOctopuses.Count;

        return (newTotalOctopuses, totalFlashCountAfterRound, step);
    }

    private static Seq<Octopus> ProcessRound(Seq<Octopus> octopuses)
    {
        var toFlashOctopuses = octopuses
            .Filter(octopus => !octopus.HasFlashed && octopus.EnergyLevel > 9);

        var octopusesAjacentToFlashedOctopuses = octopuses
            .Except(toFlashOctopuses)
            .Filter(octopus => !octopus.HasFlashed && toFlashOctopuses.Any(octopus.IsAdjacentTo))
            .ToSeq();

        var otherOctopuses = octopuses
            .Except(octopusesAjacentToFlashedOctopuses)
            .Except(toFlashOctopuses)
            .ToSeq();

        return toFlashOctopuses
            .Map(octopus => octopus with { HasFlashed = true })
            .Concat(
                octopusesAjacentToFlashedOctopuses.Map(
                    octopus =>
                    {
                        var increaseBy = toFlashOctopuses.Count(octopus.IsAdjacentTo);
                        return octopus with { EnergyLevel = octopus.EnergyLevel + increaseBy };
                    }))
            .Concat(otherOctopuses);
    }

    private static Seq<Octopus> IncrementEnergyOfOctopuses(Seq<Octopus> currentStateOfOctopuses)
    {
        return currentStateOfOctopuses.Map(octopus => octopus with { EnergyLevel = octopus.EnergyLevel + 1 });
    }

    private static Seq<Octopus> GetParsedInput()
    {
        return FileProvider
            .GetAllLines()
            .Map(
                (verticalIndex, line) =>
                {
                    return line
                        .Map(
                            (horizontalIndex, octopusStartingValue) => new Octopus(
                                horizontalIndex,
                                verticalIndex,
                                int.Parse(octopusStartingValue.ToString())));
                })
            .Bind(values => values)
            .ToSeq();
    }

    private record Octopus(int XPos, int YPos, int EnergyLevel, bool HasFlashed = false)
    {
        public bool IsAdjacentTo(Octopus other)
        {
            return IsLeftOf(other)
                   || IsRightOf(other)
                   || IsAboveOf(other)
                   || IsBelowOf(other)
                   || IsOnTopLeftDiagonalOf(other)
                   || IsOnTopRightDiagonalOf(other)
                   || IsOnBottomLeftDiagonalOf(other)
                   || IsOnBottomRightDiagonalOf(other);
        }

        private bool IsLeftOf(Octopus other)
        {
            return this.YPos == other.YPos && this.XPos - 1 == other.XPos;
        }

        private bool IsRightOf(Octopus other)
        {
            return this.YPos == other.YPos && this.XPos + 1 == other.XPos;
        }

        private bool IsAboveOf(Octopus other)
        {
            return this.YPos - 1 == other.YPos && this.XPos == other.XPos;
        }

        private bool IsBelowOf(Octopus other)
        {
            return this.YPos + 1 == other.YPos && this.XPos == other.XPos;
        }

        private bool IsOnTopLeftDiagonalOf(Octopus other)
        {
            return this.YPos - 1 == other.YPos && this.XPos - 1 == other.XPos;
        }

        private bool IsOnTopRightDiagonalOf(Octopus other)
        {
            return this.YPos - 1 == other.YPos && this.XPos + 1 == other.XPos;
        }

        private bool IsOnBottomLeftDiagonalOf(Octopus other)
        {
            return this.YPos + 1 == other.YPos && this.XPos - 1 == other.XPos;
        }

        private bool IsOnBottomRightDiagonalOf(Octopus other)
        {
            return this.YPos + 1 == other.YPos && this.XPos + 1 == other.XPos;
        }
    }
}