using LanguageExt;

namespace AdventOfCode2021.Day4;

public record BingoCard(Seq<BingoCardNumber> NumbersOnBoard, int LastCheckedNumber = -1)
{
    public bool HasCompleteColumnOrRow()
    {
        var hasFullColumn = this.NumbersOnBoard
            .GroupBy(number => number.Column)
            .Any(numbersInColumn => numbersInColumn.All(number => number.IsChecked));

        var hasFullRow = this.NumbersOnBoard
            .GroupBy(number => number.Row)
            .Any(numbersInRow => numbersInRow.All(number => number.IsChecked));

        return hasFullColumn || hasFullRow;
    }
}