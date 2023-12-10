using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2021.Day9;

public class DayNine : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var input = GetParsedInput();

        var lowPoints = input.Filter(
                position => FlattenOptionsToSequence(
                        FindPositionAt(input, position.Key with { XPosition = position.Key.YPosition - 1 }),
                        FindPositionAt(input, position.Key with { XPosition = position.Key.YPosition + 1 }),
                        FindPositionAt(input, position.Key with { XPosition = position.Key.XPosition - 1 }),
                        FindPositionAt(input, position.Key with { XPosition = position.Key.XPosition + 1 }))
                    .All(adjacentPosition => adjacentPosition.Value > position.Value.Value))
            .ToSeq();

        var riskScore = lowPoints.Sum(position => position.Value.Value + 1);
        yield return new PuzzleResult(riskScore);

        var productOfLargestBasins = lowPoints
            .Map(pair => Dfs(pair.Value, input))
            .OrderByDescending(basinScore => basinScore.Value)
            .Take(3)
            .Fold(1, (reducer, nextValue) => reducer * nextValue.Value);

        yield return new PuzzleResult(productOfLargestBasins);
    }

    private static Option<Position> FindPositionAt(
        IReadOnlyDictionary<Coordinate, Position> input,
        Coordinate toBeCheckedCoordinate)
    {
        return input.TryGetValue(toBeCheckedCoordinate, out var result)
            ? Prelude.Some(result)
            : Prelude.None;
    }

    private static (Position CurrentPosition, int Value, Dictionary<Coordinate, Position> Input) Dfs(
        Position currentPosition,
        Dictionary<Coordinate, Position> input)
    {
        input.Remove(currentPosition.Coordinate);

        var leftOfPosition = TryExplore(
            tobeExpoloredPositions => FindPositionAt(
                tobeExpoloredPositions,
                currentPosition.Coordinate with { XPosition = currentPosition.Coordinate.YPosition - 1 }),
            input);

        var setOfUncheckedPositionsAfterLeft = leftOfPosition
            .Match(
                tuple => tuple.Input,
                () => input);

        var rightOfPosition = TryExplore(
            tobeExpoloredPositions => FindPositionAt(
                tobeExpoloredPositions,
                currentPosition.Coordinate with { XPosition = currentPosition.Coordinate.YPosition + 1 }),
            setOfUncheckedPositionsAfterLeft);

        var setOfUncheckedPositionsAfterRight = rightOfPosition
            .Match(
                tuple => tuple.Input,
                () => setOfUncheckedPositionsAfterLeft);

        var aboveOfPosition = TryExplore(
            tobeExpoloredPositions => FindPositionAt(
                tobeExpoloredPositions,
                currentPosition.Coordinate with { XPosition = currentPosition.Coordinate.XPosition - 1 }),
            setOfUncheckedPositionsAfterRight);

        var setOfUncheckedPositionsAfterAbove = aboveOfPosition
            .Match(
                tuple => tuple.Input,
                () => setOfUncheckedPositionsAfterRight);

        var belowOfPosition = TryExplore(
            tobeExpoloredPositions => FindPositionAt(
                tobeExpoloredPositions,
                currentPosition.Coordinate with { XPosition = currentPosition.Coordinate.XPosition + 1 }),
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

    private static Dictionary<Coordinate, Position> GetParsedInput()
    {
        return FileProvider
            .GetAllLines()
            .Map(
                (rowIndex, lineOfPosition) =>
                {
                    return lineOfPosition
                        .Map(
                            (columnIndex, valueOfPosition) => new Position(
                                new Coordinate(
                                    rowIndex,
                                    columnIndex),
                                int.Parse(valueOfPosition.ToString())))
                        .ToDictionary(position => position.Coordinate, position2 => position2);
                })
            .Bind(seq => seq)
            .Filter(keyValuePair => keyValuePair.Value.Value != 9)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private static Option<(Position CurrentPosition, int Value, Dictionary<Coordinate, Position> Input)> TryExplore(
        Func<Dictionary<Coordinate, Position>, Option<Position>> possiblePosition,
        Dictionary<Coordinate, Position> toBeExploredPositions)
    {
        return possiblePosition(toBeExploredPositions)
            .Map(
                position =>
                {
                    var valueTuple = Dfs(position, toBeExploredPositions);
                    return (valueTuple.CurrentPosition, valueTuple.Value, valueTuple.Input);
                });
    }

    private record Position(Coordinate Coordinate, int Value);

    private record Coordinate(int XPosition, int YPosition);

    private static Seq<T> FlattenOptionsToSequence<T>(params Option<T>[] values)
    {
        return Seq
            .create(values)
            .Bind(option => option)
            .ToSeq();
    }
}