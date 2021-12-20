using LanguageExt;

namespace AdventOfCode2021.Day9;

public class DayNine : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var input = GetParsedInput()
            .Filter(position => position.Value != 9);

        var lowPoints = input.Filter(
            position => FlattenOptionsToSequence(
                    FindLeftOfPosition(input, position),
                    FindRightOfPosition(input, position),
                    FindAboveOfPosition(input, position),
                    FindBelowOfPosition(input, position))
                .Memo()
                .All(adjacentPosition => adjacentPosition.Value > position.Value));

        var riskScore = lowPoints.Sum(position => position.Value + 1);
        yield return new PuzzleResult(1, riskScore);

        var productOfLargestBasins = lowPoints
            .Aggregate<Position, (Seq<Position>, Seq<int>), Seq<int>>(
                (input, Seq.empty<int>()),
                ((Seq<Position> input, Seq<int> basinScores) accumulatedValues, Position lowPoint) =>
                {
                    var basinResult = Dfs(lowPoint, accumulatedValues.input);
                    return (basinResult.Input, accumulatedValues.basinScores.Add(basinResult.Value));
                },
                result => result.Item2)
            .OrderByDescending(basinScore => basinScore)
            .Take(3)
            .Reduce((reducer, nextValue) => reducer * nextValue);

        yield return new PuzzleResult(2, productOfLargestBasins);
    }

    private static Option<Position> FindBelowOfPosition(Seq<Position> input, Position position) => input.Find(
        inputPosition => inputPosition.XPosition == position.XPosition + 1
                         && inputPosition.YPosition == position.YPosition);

    private static Option<Position> FindAboveOfPosition(Seq<Position> input, Position position) => input.Find(
        inputPosition => inputPosition.XPosition == position.XPosition - 1
                         && inputPosition.YPosition == position.YPosition);

    private static Option<Position> FindRightOfPosition(Seq<Position> input, Position position) => input.Find(
        inputPosition => inputPosition.YPosition == position.YPosition + 1
                         && inputPosition.XPosition == position.XPosition);

    private static Option<Position> FindLeftOfPosition(Seq<Position> input, Position position) => input.Find(
        inputPosition => inputPosition.YPosition == position.YPosition - 1
                         && inputPosition.XPosition == position.XPosition);

    private static (Position CurrentPosition, int Value, Seq<Position> Input) Dfs(
        Position currentPosition,
        Seq<Position> input)
    {
        var leftoverPositionsWithoutCurrent = input.Except(Prelude.Seq1(currentPosition)).ToSeq();

        var leftOfPosition = TryExplore(
            tobeExpoloredPositions => FindLeftOfPosition(tobeExpoloredPositions, currentPosition),
            leftoverPositionsWithoutCurrent);

        var setOfUncheckedPositionsAfterLeft = leftOfPosition
            .Match(
                tuple => tuple.Input,
                () => leftoverPositionsWithoutCurrent);

        var rightOfPosition = TryExplore(
            tobeExpoloredPositions => FindRightOfPosition(tobeExpoloredPositions, currentPosition),
            setOfUncheckedPositionsAfterLeft);

        var setOfUncheckedPositionsAfterRight = rightOfPosition
            .Match(
                tuple => tuple.Input,
                () => setOfUncheckedPositionsAfterLeft);

        var aboveOfPosition = TryExplore(
            tobeExpoloredPositions => FindAboveOfPosition(tobeExpoloredPositions, currentPosition),
            setOfUncheckedPositionsAfterRight);

        var setOfUncheckedPositionsAfterAbove = aboveOfPosition
            .Match(
                tuple => tuple.Input,
                () => setOfUncheckedPositionsAfterRight);

        var belowOfPosition = TryExplore(
            tobeExpoloredPositions => FindBelowOfPosition(tobeExpoloredPositions, currentPosition),
            setOfUncheckedPositionsAfterAbove);

        var setOfUncheckedPositionsAfterBelow = belowOfPosition.Match(
            tuple => tuple.Input,
            () => setOfUncheckedPositionsAfterAbove);

        var currentPositionValue = FlattenOptionsToSequence(
                leftOfPosition,
                rightOfPosition,
                belowOfPosition,
                aboveOfPosition)
            .Sum(tuple => tuple.Item2);

        return (currentPosition, currentPositionValue + 1, setOfUncheckedPositionsAfterBelow);
    }

    private static Seq<Position> GetParsedInput()
    {
        return FileProvider
            .GetAllLines("Day9.input.txt")
            .Map(
                (rowIndex, lineOfPosition) =>
                {
                    return lineOfPosition
                        .Map(
                            (columnIndex, valueOfPosition) => new Position(
                                rowIndex,
                                columnIndex,
                                int.Parse(valueOfPosition.ToString())))
                        .ToSeq();
                })
            .Bind(seq => seq)
            .ToSeq();
    }

    private static Option<(Position CurrentPosition, int Value, Seq<Position> Input)> TryExplore(
        Func<Seq<Position>, Option<Position>> possiblePosition,
        Seq<Position> toBeExploredPositions)
    {
        return possiblePosition(toBeExploredPositions)
            .Map(
                position =>
                {
                    var valueTuple = Dfs(position, toBeExploredPositions);
                    return (valueTuple.CurrentPosition, valueTuple.Value, valueTuple.Input);
                });
    }

    private record Position(int XPosition, int YPosition, int Value);

    private static Seq<T> FlattenOptionsToSequence<T>(params Option<T>[] values)
    {
        return Seq
            .create(values)
            .Bind(option => option)
            .ToSeq();
    }
}