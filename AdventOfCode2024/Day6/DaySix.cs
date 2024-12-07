using AdventOfCode.Generic;

namespace AdventOfCode2024.Day6;

public class DaySix : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider.GetAllLines();

        var mapArea = parsedData.Map(x => x.ToCharArray()).ToArray();
        var guardPosition = mapArea
            .Map((yIndex, y) => new GuardPosition(Array.IndexOf(y, '^'), yIndex, FacingDirection.North))
            .First(x => x.X != -1);

        var visitedPositions = new List<GuardPosition> { guardPosition };
        while (true)
        {
            var nextPosition = GetNextPosition(guardPosition);
            if (IsOutOfBounds(nextPosition, mapArea))
            {
                break;
            }

            var mapSquare = mapArea[nextPosition.Y][nextPosition.X];
            if (mapSquare == '#')
            {
                guardPosition = guardPosition with { FacingDirection = Turn(guardPosition.FacingDirection) };
                nextPosition = GetNextPosition(guardPosition);
            }

            guardPosition = guardPosition with { X = nextPosition.X, Y = nextPosition.Y };

            if (!visitedPositions.Any(x => x.X == guardPosition.X && x.Y == guardPosition.Y))
            {
                visitedPositions.Add(guardPosition);
            }
        }

        yield return new PuzzleResult(visitedPositions.Count);
    }

    private static bool IsOutOfBounds(GuardPosition position, char[][] map)
    {
        return position.Y < 0 || position.Y == map.Length || position.X < 0 || position.X == map[0].Length;
    }

    private static GuardPosition GetNextPosition(GuardPosition guardPositionPosition)
    {
        return guardPositionPosition.FacingDirection switch
        {
            FacingDirection.North => guardPositionPosition with { Y = guardPositionPosition.Y - 1 },
            FacingDirection.East => guardPositionPosition with { X = guardPositionPosition.X + 1 },
            FacingDirection.South => guardPositionPosition with { Y = guardPositionPosition.Y + 1 },
            FacingDirection.West => guardPositionPosition with { X = guardPositionPosition.X - 1 }
        };
    }

    private static FacingDirection Turn(FacingDirection facingDirection)
    {
        return facingDirection switch
        {
            FacingDirection.North => FacingDirection.East,
            FacingDirection.East => FacingDirection.South,
            FacingDirection.South => FacingDirection.West,
            FacingDirection.West => FacingDirection.North
        };
    }

    private record GuardPosition(int X, int Y, FacingDirection FacingDirection);

    private enum FacingDirection
    {
        North,
        East,
        South,
        West
    }
}