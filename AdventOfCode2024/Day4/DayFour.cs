using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2024.Day4;

public class DayFour : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider
            .GetAllLines()
            .Map(x => x.ToCharArray());

        var puzzleOneResult = Prelude
            .Range(0, parsedData.Count)
            .Bind(
                y => Prelude
                    .Range(0, parsedData[y].Length)
                    .Map(x => parsedData[y][x] != 'X' ? 0 : SearchXMAS(x, y, parsedData).Somes().Count()))
            .Sum();

        yield return new PuzzleResult(puzzleOneResult);

        var puzzleTwoResult = Prelude
            .Range(0, parsedData.Count)
            .Bind(
                y => Prelude
                    .Range(0, parsedData[y].Length)
                    .Map(x => parsedData[y][x] != 'A' ? 0 : SearchMASInX(x, y, parsedData)))
            .Sum();

        yield return new PuzzleResult(puzzleTwoResult);
    }

    private static int SearchMASInX(int x, int y, Seq<char[]> parsedData)
    {
        var permutations = new List<char[]>
        {
            { ['M', 'M', 'S', 'S'] },
            { ['M', 'S', 'M', 'S'] },
            { ['S', 'M', 'S', 'M'] },
            { ['S', 'S', 'M', 'M'] }
        };

        var isTheMASinX = permutations
            .Select(charsToFind => MASinXHounds(x, y, parsedData, charsToFind))
            .Somes()
            .Any();
        return isTheMASinX
            ? 1
            : 0;
    }

    private static IEnumerable<Option<Finding>> SearchXMAS(int x, int y, Seq<char[]> parsedData)
    {
        return XMASHounds()
            .Select(
                hound => hound(x, y, parsedData, 'M')
                    .Bind(f => hound(f.X, f.Y, parsedData, 'A'))
                    .Bind(f => hound(f.X, f.Y, parsedData, 'S')));
    }

    private static IEnumerable<Func<int, int, Seq<char[]>, char, Option<Finding>>> XMASHounds()
    {
        return
        [
            SearchLeft,
            SearchRight,
            SearchUp,
            SearchDown,
            SearchUpLeft,
            SearchUpRight,
            SearchDownLeft,
            SearchDownRight
        ];
    }

    private static Option<Unit> MASinXHounds(int x, int y, Seq<char[]> parsedData, char[] charsToFind)
    {
        return
            from mUp in SearchUpLeft(x, y, parsedData, charsToFind[0])
            from mDown in SearchDownLeft(x, y, parsedData, charsToFind[1])
            from aUp in SearchUpRight(x, y, parsedData, charsToFind[2])
            from aDown in SearchDownRight(x, y, parsedData, charsToFind[3])
            let m = (x, y)
            select Prelude.unit;
    }

    private static Option<Finding> SearchLeft(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        var row = parsedData[y];
        if (x - 1 >= 0)
        {
            if (row[x - 1] == searchFor)
            {
                return Prelude.Some(new Finding(x - 1, y));
            }
        }

        return Prelude.None;
    }

    private static Option<Finding> SearchRight(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        var row = parsedData[y];
        if (x + 1 < row.Length)
        {
            if (row[x + 1] == searchFor)
            {
                return Prelude.Some(new Finding(x + 1, y));
            }
        }

        return Prelude.None;
    }

    private static Option<Finding> SearchUp(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        if (y - 1 >= 0)
        {
            if (parsedData[y - 1][x] == searchFor)
            {
                return Prelude.Some(new Finding(x, y - 1));
            }
        }

        return Prelude.None;
    }

    private static Option<Finding> SearchDown(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        if (y + 1 < parsedData.Count)
        {
            if (parsedData[y + 1][x] == searchFor)
            {
                return Prelude.Some(new Finding(x, y + 1));
            }
        }

        return Prelude.None;
    }

    private static Option<Finding> SearchUpLeft(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            if (parsedData[y - 1][x - 1] == searchFor)
            {
                return Prelude.Some(new Finding(x - 1, y - 1));
            }
        }

        return Prelude.None;
    }

    private static Option<Finding> SearchUpRight(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        var row = parsedData[y];
        if (x + 1 < row.Length && y - 1 >= 0)
        {
            if (parsedData[y - 1][x + 1] == searchFor)
            {
                return Prelude.Some(new Finding(x + 1, y - 1));
            }
        }

        return Prelude.None;
    }

    private static Option<Finding> SearchDownLeft(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        if (x - 1 >= 0 && y + 1 < parsedData.Count)
        {
            if (parsedData[y + 1][x - 1] == searchFor)
            {
                return Prelude.Some(new Finding(x - 1, y + 1));
            }
        }

        return Prelude.None;
    }

    private static Option<Finding> SearchDownRight(int x, int y, Seq<char[]> parsedData, char searchFor)
    {
        var row = parsedData[y];
        if (x + 1 < row.Length && y + 1 < parsedData.Count)
        {
            if (parsedData[y + 1][x + 1] == searchFor)
            {
                return Prelude.Some(new Finding(x + 1, y + 1));
            }
        }

        return Prelude.None;
    }

    private record Finding(int X, int Y);
}