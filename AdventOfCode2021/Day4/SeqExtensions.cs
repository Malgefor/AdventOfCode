using LanguageExt;

namespace AdventOfCode2021.Day4;

public static class SeqExtensions
{
    public static Seq<BingoCard> CrossOffDrawnNumbersUntil(
        this Seq<int> drawnNumbers,
        Seq<BingoCard> bingoCards,
        Func<Seq<BingoCard>, bool> untilFunc)
    {
        return drawnNumbers
            .FoldUntil(
                bingoCards,
                (cards, bingoNumber) => cards
                    .Map(
                        card =>
                        {
                            if (card.NumbersOnBoard.Any(numberOnCard => numberOnCard.Number == bingoNumber)
                                && !card.HasCompleteColumnOrRow())
                            {
                                return card with
                                {
                                    NumbersOnBoard = card.NumbersOnBoard
                                        .Filter(numberOnCard => numberOnCard.Number == bingoNumber)
                                        .Map(number => number with { IsChecked = true })
                                        .Concat(
                                            card.NumbersOnBoard.Filter(
                                                numberOnCard => numberOnCard.Number != bingoNumber)),
                                    LastCheckedNumber = bingoNumber
                                };
                            }

                            return card;
                        }),
                untilFunc);
    }
}