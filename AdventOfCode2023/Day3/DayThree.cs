using System.Text.RegularExpressions;

using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2023.Day3;

public class DayThree : IPuzzleDay
{
    public int DayNumber => 3;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();

        var resultPuzzleOne = parsedInput
            .Map(
                (index, line) =>
                {
                    var partNumbers = line
                        .Where(schematicOption => schematicOption is not Dot && schematicOption is not Symbol)
                        .Cast<PartNumber>()
                        .ToArr();

                    var validParts = new List<PartNumber>();

                    if (index != 0)
                    {
                        var previousLine = parsedInput[index - 1];
                        var symbolsPreviousLine = GetSymbols(previousLine);

                        validParts.AddRange(
                            partNumbers
                                .Where(
                                    partNumber => partNumber.Indexes.Any(
                                        partNumberIndex => GetValidParts(
                                            symbolsPreviousLine,
                                            partNumberIndex))));
                    }

                    if (index != parsedInput.Length - 1)
                    {
                        var nextLine = parsedInput[index + 1];
                        var symbolsNextLine = GetSymbols(nextLine);

                        validParts.AddRange(
                            partNumbers
                                .Where(
                                    partNumber => partNumber.Indexes.Any(
                                        partNumberIndex => GetValidParts(symbolsNextLine, partNumberIndex))));
                    }

                    var symbolsCurrentLine = GetSymbols(line);
                    validParts.AddRange(
                        partNumbers
                            .Where(
                                partNumber => partNumber.Indexes.Any(
                                    partNumberIndex => GetValidParts(symbolsCurrentLine, partNumberIndex))));

                    return validParts.Distinct();
                })
            .Flatten()
            .Sum(x => x.Number);

        yield return new PuzzleResult(resultPuzzleOne);

        var puzzleTwoResult = parsedInput
            .Map(
                (index, line) =>
                {
                    var partsInThaHood = parsedInput
                        .Flatten()
                        .Where(x => x is PartNumber)
                        .Cast<PartNumber>()
                        .Where(
                            partnumber => partnumber.RowIndex == index || partnumber.RowIndex == index - 1
                                                                       || partnumber.RowIndex == index + 1)
                        .ToSeq();

                    return line
                        .Where(schematicOption => schematicOption is Symbol { IsGear: true })
                        .Cast<Symbol>()
                        .Map(
                            gear => partsInThaHood
                                .Where(
                                    part => part.Indexes.Any(
                                        partIndex =>
                                            partIndex == gear.Index || partIndex == gear.Index + 1
                                                                    || partIndex == gear.Index - 1)))
                        .Where(neighbours => neighbours.Count == 2)
                        .Map(neighbours => neighbours.Fold(1, (product, partNumber) => product * partNumber.Number));
                })
            .ToSeq()
            .Flatten()
            .Sum();

        yield return new PuzzleResult(puzzleTwoResult);
    }

    private static Seq<Symbol> GetSymbols(Seq<ISchematicOption> line)
    {
        return line
            .Where(schematicOption => schematicOption is Symbol)
            .Cast<Symbol>()
            .ToSeq();
    }

    private static bool GetValidParts(Seq<Symbol> symbols, int partNumberIndex) => symbols.Any(
        symbol => symbol.Index == partNumberIndex
                  || (partNumberIndex != 0 && partNumberIndex - 1 == symbol.Index)
                  || (partNumberIndex != 140 && partNumberIndex + 1 == symbol.Index));

    private static Seq<Seq<ISchematicOption>> GetParsedInput() => FileProvider
        .GetAllLines()
        .Map(
            (index, line) =>
            {
                var current = new List<ISchematicOption>();
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] == '.')
                    {
                        current.Add(ISchematicOptionCon.Dot(i));
                        continue;
                    }

                    if (Regex.IsMatch(line[i].ToString(), "[^0-9\\.]"))
                    {
                        var isGear = line[i] == '*';
                        current.Add(ISchematicOptionCon.Symbol(isGear, i));
                        continue;
                    }

                    var digit = new List<(char, int)> { (line[i], i) };
                    while (i + 1 < line.Length && char.IsDigit(line[i + 1]))
                    {
                        i++;
                        digit.Add((line[i], i));
                    }

                    var partNumber = digit.Map(x => x.Item1).Fold(string.Empty, (acc, cur) => acc + cur);
                    var indexes = digit.Map(x => x.Item2).ToArray();
                    current.Add(ISchematicOptionCon.PartNumber(int.Parse(partNumber), indexes, index));
                }

                return current.ToSeq();
            })
        .ToSeq();
}