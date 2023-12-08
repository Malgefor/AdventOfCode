using System.Numerics;

namespace AdventOfCode2023.Day6;

public static class DaySixExtentions
{
    public static TNumber CalculateWinningOptions<TNumber>(
        this IEnumerable<TNumber> source,
        TNumber gameLength,
        TNumber gameRecord)
        where TNumber : INumber<TNumber>
    {
        return source
            .Fold(
                default(TNumber),
                (acc, holdingMiliseconds) =>
                    (gameLength - holdingMiliseconds) * holdingMiliseconds > gameRecord
                        ? acc + TNumber.MultiplicativeIdentity
                        : acc);
    }
}