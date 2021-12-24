using LanguageExt;

namespace AdventOfCode2021.Day8;

public class DayEight : IPuzzleDay
{
    public int DayNumber => 8;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var inputLines = GetParsedInput();

        var result = inputLines
            .Bind(
                line => line
                    .OutputValues
                    .Filter(
                        outputValue => IsOne(outputValue) || IsFour(outputValue) || IsSeven(outputValue) || IsEight(outputValue)))
            .Count;

        yield return new PuzzleResult(1, result);

        var sumOfOutputValues = inputLines.Map(
            line =>
            {
                var oneValue = line.UniqueCombinations.First(IsOne).ToSeq();
                var fourValue = line.UniqueCombinations.First(IsFour).ToSeq();
                var sevenValue = line.UniqueCombinations.First(IsSeven).ToSeq();
                var eightValue = line.UniqueCombinations.First(IsEight).ToSeq();

                var sixValue = line.UniqueCombinations
                    .Filter(combination => combination.Length == 6)
                    .Filter(combination => combination.Intersect(oneValue).ToSeq().Length == 1)
                    .First();

                var topValue = sevenValue.Except(oneValue).First();
                var bottomRight = sixValue.Intersect(oneValue).First();
                var topRight = oneValue.Except(Prelude.Seq1(bottomRight)).First();

                var threeValue = line.UniqueCombinations
                    .Filter(value => value.Length == 5)
                    .Filter(value => value.Contains(topValue) && value.Contains(bottomRight) && value.Contains(topRight))
                    .First();

                var topLeft = fourValue.Except(threeValue.ToSeq()).First();
                var middleValue = fourValue.Except(Seq.create(topLeft, topRight, bottomRight)).First();
                var bottomValue = threeValue.Except(Seq.create(topValue, topRight, middleValue, bottomRight)).First();
                var bottomLeft =
                    eightValue
                        .Except(Seq.create(topValue, topLeft, topRight, middleValue, bottomRight, bottomValue))
                        .First();

                var displayNumber = new DisplayNumber(
                    topValue,
                    middleValue,
                    bottomValue,
                    topLeft,
                    topRight,
                    bottomLeft,
                    bottomRight);

                return line.OutputValues
                    .Map(value => displayNumber.CreateDigit(value))
                    .Reduce(ConcatIntegers);
            });

        yield return new PuzzleResult(2, sumOfOutputValues.Sum(number => number));
    }

    private static bool IsOne(string value) => value.Length == 2;

    private static bool IsFour(string value) => value.Length == 4;

    private static bool IsSeven(string value) => value.Length == 3;

    private static bool IsEight(string value) => value.Length == 7;

    private static Seq<InputLine> GetParsedInput() => FileProvider
        .GetAllLines("Day8.input.txt")
        .Map(
            inputString =>
            {
                var splitSegments = inputString.Split('|');
                var uniqueCombinations = splitSegments.First().Trim().Split(" ").ToSeq();
                var outputValues = splitSegments.Last().Trim().Split(" ").ToSeq();

                return new InputLine(uniqueCombinations, outputValues);
            })
        .ToSeq();

    private record InputLine(Seq<string> UniqueCombinations, Seq<string> OutputValues);

    private record DisplayNumber(
        char Top,
        char Middle,
        char Bottom,
        char TopLeft,
        char TopRight,
        char BottomLeft,
        char BottomRight)
    {
        public int CreateDigit(string input)
        {
            if (IsOne(input))
            {
                return 1;
            }

            if (IsFour(input))
            {
                return 4;
            }

            if (IsSeven(input))
            {
                return 7;
            }

            if (IsEight(input))
            {
                return 8;
            }

            if (input.Contains(this.Top)
                && input.Contains(this.TopLeft)
                && input.Contains(this.TopRight)
                && input.Contains(this.BottomRight)
                && input.Contains(this.BottomLeft)
                && input.Contains(this.Bottom))
            {
                return 0;
            }

            if (input.Contains(this.Top)
                && input.Contains(this.TopLeft)
                && input.Contains(this.Middle)
                && input.Contains(this.BottomRight)
                && input.Contains(this.BottomLeft)
                && input.Contains(this.Bottom))
            {
                return 6;
            }

            if (input.Contains(this.Top)
                && input.Contains(this.TopLeft)
                && input.Contains(this.TopRight)
                && input.Contains(this.BottomRight)
                && input.Contains(this.Middle)
                && input.Contains(this.Bottom))
            {
                return 9;
            }

            if (input.Contains(this.Top)
                && input.Contains(this.TopRight)
                && input.Contains(this.Middle)
                && input.Contains(this.BottomLeft)
                && input.Contains(this.Bottom))
            {
                return 2;
            }

            if (input.Contains(this.Top)
                && input.Contains(this.TopRight)
                && input.Contains(this.Middle)
                && input.Contains(this.BottomRight)
                && input.Contains(this.Bottom))
            {
                return 3;
            }

            return 5;
        }
    }

    private static int ConcatIntegers(int a, int b)
    {
        return int.Parse(a.ToString() + b);
    }
}