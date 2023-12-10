using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2023.Day2;

public class DayTwo : IPuzzleDay
{
    public int DayNumber => 2;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var maxCounts = new[]
        {
            new CubeCount(12, "red"),
            new CubeCount(13, "green"),
            new CubeCount(14, "blue")
        };

        var parsedInput = GetParsedInput();
        var resultPuzzleOne = parsedInput
            .Filter(
                game => game.Cubes
                    .ForAll(y => y.Count <= maxCounts.First(z => z.Colour == y.Colour).Count))
            .Sum(x => x.Id);

        yield return new PuzzleResult(resultPuzzleOne);

        var resultPuzzleTwo = parsedInput
            .Map(
                game => game.Cubes
                    .Fold(
                        new[] { new CubeCount(0, "red"), new(0, "green"), new(0, "blue") }.ToDictionary(x => x.Colour),
                        (accumulate, current) =>
                        {
                            var currentMaxForColour = accumulate[current.Colour];
                            if (current.Count > currentMaxForColour.Count)
                            {
                                accumulate[current.Colour] = current;
                            }

                            return accumulate;
                        }))
            .Sum(x => x.Map(y => y.Value.Count).Reduce((acc, current) => acc * current));

        yield return new PuzzleResult(resultPuzzleTwo);
    }

    private record Game(int Id, Seq<CubeCount> Cubes);

    private record CubeCount(int Count, string Colour);

    private static Seq<Game> GetParsedInput() => FileProvider
        .GetAllLines()
        .Map(
            game =>
            {
                var split = game.Split(':');
                var cubeCounts = split[1]
                    .Split(';')
                    .Bind(
                        cubeSet => cubeSet
                            .Split(',')
                            .Map(
                                showing =>
                                {
                                    var trim = showing.Trim().Split(" ");
                                    return new CubeCount(int.Parse(trim[0]), trim[1]);
                                }))
                    .ToSeq();

                return new Game(int.Parse(split[0].Split(" ")[1]), cubeCounts);
            });
}