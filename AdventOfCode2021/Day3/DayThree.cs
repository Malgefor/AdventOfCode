using AdventOfCode.Generic;

using LanguageExt;

using static LanguageExt.Prelude;

using Seq = LanguageExt.Seq;

namespace AdventOfCode2021.Day3;

public class DayThree : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();

        yield return GetPuzzleOneResult(parsedInput);
        yield return GetPuzzleTwoResult(parsedInput);
    }

    private static PuzzleResult GetPuzzleOneResult(Seq<Seq<int>> parsedInput)
    {
        var sumsPerColumn = parsedInput
            .Fold(
                Seq.repeat(0, parsedInput.First().Count),
                (oneCounts, input) => oneCounts
                    .Zip(input, (currentOneCount, binaryBit) => currentOneCount + binaryBit));

        var halfOfTotalValueCount = parsedInput.Count / 2;
        var epsilon = GetDecimalRepresentationOfBinaryNumber(
            sumsPerColumn,
            sumValue => sumValue >= halfOfTotalValueCount);
        var gamma = GetDecimalRepresentationOfBinaryNumber(
            sumsPerColumn,
            sumValue => sumValue <= halfOfTotalValueCount);

        return new PuzzleResult(epsilon * gamma);
    }

    private static PuzzleResult GetPuzzleTwoResult(Seq<Seq<int>> parsedInput)
    {
        var numberWithTheMostCommonValues = GetBinaryNumberInDecimal(
            parsedInput,
            (groupOfInts, groupOfInts2) => groupOfInts > groupOfInts2,
            1);

        var numberWithTheLeastCommonValues = GetBinaryNumberInDecimal(
            parsedInput,
            (groupOfInts, groupOfInts2) => groupOfInts < groupOfInts2,
            0);

        return new PuzzleResult(numberWithTheMostCommonValues * numberWithTheLeastCommonValues);
    }

    private static long GetBinaryNumberInDecimal(
        Seq<Seq<int>> parsedInput,
        Func<int, int, bool> compareFunc,
        int defaultIfEqual)
    {
        return Enumerable
            .Range(0, 11)
            .Aggregate(
                parsedInput,
                (current, position) =>
                {
                    return current.Match(
                        () => LanguageExt.Seq<Seq<int>>.Empty,
                        Seq1,
                        (firstItem, restOfItems) =>
                        {
                            var binaryWithPositions = restOfItems
                                .Add(firstItem)
                                .Map(binaryNumber => (binaryNumber, binaryNumber[position]));

                            var onesCount = binaryWithPositions.Count(tuple => tuple.Item2 == 1);
                            var zeroesCount = binaryWithPositions.Count(tuple => tuple.Item2 == 0);

                            if (onesCount == zeroesCount)
                            {
                                return binaryWithPositions
                                    .Filter(tuple => tuple.Item2 == defaultIfEqual)
                                    .Map(tuple => tuple.binaryNumber);
                            }

                            return compareFunc(onesCount, zeroesCount)
                                ? binaryWithPositions.Filter(tuple => tuple.Item2 == 1).Map(tuple => tuple.binaryNumber)
                                : binaryWithPositions.Filter(tuple => tuple.Item2 == 0)
                                    .Map(tuple => tuple.binaryNumber);
                        });
                },
                finalValues => finalValues
                    .First()
                    .TransformBinaryToDecimal());
    }

    private static long GetDecimalRepresentationOfBinaryNumber(
        Seq<int> sumsPerColumn,
        Func<int, bool> compareFunc)
    {
        return sumsPerColumn
            .Map(sumOfColumn => compareFunc(sumOfColumn) ? 1 : 0)
            .TransformBinaryToDecimal();
    }

    private static Seq<Seq<int>> GetParsedInput()
    {
        return FileProvider
            .GetAllLines()
            .Map(value => value.Map(c => int.Parse(c.ToString())).ToSeq());
    }
}