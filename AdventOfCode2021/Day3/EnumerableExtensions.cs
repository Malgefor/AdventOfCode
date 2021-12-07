using LanguageExt;

namespace AdventOfCode2021.Day3;

public static class EnumerableExtensions
{
    public static long TransformBinaryToDecimal(this Seq<int> binaryNumber) => binaryNumber
        .Aggregate(
            string.Empty,
            (binaryNumberString, binaryDigit) => binaryNumberString + binaryDigit,
            binaryNumberString => Convert.ToInt64(binaryNumberString, 2));
}