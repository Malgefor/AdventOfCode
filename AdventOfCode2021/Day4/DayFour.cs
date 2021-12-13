using LanguageExt;

namespace AdventOfCode2021.Day4;

public class DayFour : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var (drawnNumbers, bingoCards) = GetParsedInput();

        yield return GetPuzzleOneResult(drawnNumbers, bingoCards);
        yield return GetPuzzleTwoResult(drawnNumbers, bingoCards);
    }

    private static PuzzleResult GetPuzzleTwoResult(Seq<int> drawnNumbers, Seq<BingoCard> bingoCards)
    {
        var allBoardsInWinningState = drawnNumbers.CrossOffDrawnNumbersUntil(
            bingoCards,
            cards => cards.All(card => card.HasCompleteColumnOrRow()));

        var lastToWinBingoCard = drawnNumbers
            .Join(
                allBoardsInWinningState,
                drawnNumber => drawnNumber,
                card => card.LastCheckedNumber,
                (drawnNumber, card) => (drawnNumber, card))
            .Last()
            .card;

        return new PuzzleResult(2, GetSumOfUnchecked(lastToWinBingoCard) * lastToWinBingoCard.LastCheckedNumber);
    }

    private static PuzzleResult GetPuzzleOneResult(Seq<int> drawnNumbers, Seq<BingoCard> bingoCards)
    {
        var winningBingoCard = drawnNumbers.CrossOffDrawnNumbersUntil(
                bingoCards,
                cards => cards.Any(card => card.HasCompleteColumnOrRow()))
            .First(card => card.HasCompleteColumnOrRow());

        return new PuzzleResult(1, GetSumOfUnchecked(winningBingoCard) * winningBingoCard.LastCheckedNumber);
    }

    private static int GetSumOfUnchecked(BingoCard winningBingoCard)
    {
        return winningBingoCard
            .NumbersOnBoard
            .Filter(number => !number.IsChecked)
            .Sum(number => number.Number);
    }

    private static (Seq<int> DrawnNumbers, Seq<BingoCard> BingoCards) GetParsedInput()
    {
        var allLines = FileProvider
            .GetAllLines("Day4.input.txt");

        var drawnNumbers = allLines
            .Head
            .Split(',')
            .Map(int.Parse)
            .ToSeq();

        var bingoCards = allLines
            .Skip(1)
            .Filter(input => !string.IsNullOrWhiteSpace(input))
            .Chunk(5)
            .Map(
                bingoCardStrings =>
                {
                    var numbersOnCard = bingoCardStrings.Map(
                            (columnIndex, bingoCardRow) =>
                            {
                                return bingoCardRow
                                    .Split(" ")
                                    .Filter(rowValue => !string.IsNullOrWhiteSpace(rowValue))
                                    .Map(
                                        (rowIndex, bingoCardNumber) => new BingoCardNumber(
                                            columnIndex,
                                            rowIndex,
                                            int.Parse(bingoCardNumber)));
                            })
                        .Bind(bingoCardNumbers => bingoCardNumbers)
                        .ToSeq();

                    return new BingoCard(numbersOnCard);
                })
            .ToSeq();

        return (drawnNumbers, bingoCards);
    }
}