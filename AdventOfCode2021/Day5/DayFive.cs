using LanguageExt;

namespace AdventOfCode2021.Day5;

public class DayFive : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var allLines = GetParsedInput()
            .Map(startAndEndPosition => Line.CreateLineFromStartAndEnd(startAndEndPosition.Item1, startAndEndPosition.Item2));

        yield return new PuzzleResult(
            1,
            CountPositionsWithOccurrenceMoreThanOnce(
                allLines.Filter(line => line.IsVerticalOrHorizontal())));

        yield return new PuzzleResult(
            2,
            CountPositionsWithOccurrenceMoreThanOnce(
                allLines));
    }

    private static int CountPositionsWithOccurrenceMoreThanOnce(Seq<Line> lines) => lines
        .Bind(line => line.IntermediatePositions)
        .GroupBy(position => position)
        .Count(grouping => grouping.Count() > 1);

    private static Seq<(Position, Position)> GetParsedInput() => FileProvider
        .GetAllLines("Day5.input.txt")
        .Map(
            lineString =>
            {
                var positions = lineString
                    .Split("->")
                    .Map(
                        positionString =>
                        {
                            var points = positionString
                                .Trim()
                                .Split(',')
                                .Map(int.Parse)
                                .ToSeq();

                            return new Position(points.First(), points.Last);
                        })
                    .ToSeq();

                return (positions.First(), positions.Last);
            });

    private record Line(Position Start, Position End, Seq<Position> IntermediatePositions)
    {
        public static Line CreateLineFromStartAndEnd(Position start, Position end)
        {
            return new Line(
                start,
                end,
                GetIntermediatePositions(start.X, end.X)
                    .Zip(GetIntermediatePositions(start.Y, end.Y))
                    .Map(tuple => new Position(tuple.Left, tuple.Right))
                    .ToSeq());
        }

        public bool IsVerticalOrHorizontal() => this.Start.X == this.End.X || this.Start.Y == this.End.Y;

        private static Seq<int> GetIntermediatePositions(int start, int end)
        {
            if (start == end)
            {
                return Seq.repeat(start, int.MaxValue);
            }

            return start < end
                ? Enumerable.Range(start, Math.Abs(start - end) + 1).ToSeq()
                : Enumerable.Range(end, Math.Abs(start - end) + 1).Reverse().ToSeq();
        }
    }

    private record Position(int X, int Y);
}