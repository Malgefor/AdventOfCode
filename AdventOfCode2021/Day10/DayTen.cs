using LanguageExt;
using LanguageExt.Parsec;

using static LanguageExt.Parsec.Prim;
using static LanguageExt.Parsec.Char;

namespace AdventOfCode2021.Day10;

public class DayTen : IPuzzleDay
{
    private static readonly Dictionary<string, int> PuzzleOneScoreMapping = new()
    {
        { ")", 3 },
        { "]", 57 },
        { "}", 1197 },
        { ">", 25137 }
    };

    private static readonly Dictionary<string, int> PuzzleTwoScoreMapping = new()
    {
        { ")", 1 },
        { "]", 2 },
        { "}", 3 },
        { ">", 4 }
    };

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parser = many(GetCharacterParsingOptions());

        var faultedLines = GetParsedInput()
            .Map(line => (line, parserResult: parse(parser, line)))
            .Filter(parserResult => parserResult.Item2.IsFaulted)
            .Map(lineAndResult => (lineAndResult, puzzleScore: TryGetPuzzleOneScore(lineAndResult.Item2.Reply.Error.Msg)));

        yield return new PuzzleResult(1, faultedLines.Bind(tuple => tuple.puzzleScore).Sum());

        var medianScoreOfIncompleteLines = faultedLines
            .Filter(tuple => tuple.puzzleScore.IsNone)
            .Map(
                tuple =>
                {
                    return Seq
                        .repeat(0, int.MaxValue)
                        .FoldUntil(
                            (score: (double)0, tuple.lineAndResult),
                            (values, _) =>
                            {
                                var missingChar = GetMissingCharFromParseResult(values.lineAndResult.parserResult);
                                var score = CalculateNewScore(values.Item1, missingChar);
                                var newLineState = values.lineAndResult.line + missingChar;
                                return (score, (newLineState, parse(parser, newLineState)));
                            },
                            valueTuple => !valueTuple.lineAndResult.parserResult.IsFaulted)
                        .Map(valueTuple => valueTuple.Item1);
                })
            .GetMedian();

        yield return new PuzzleResult(2, medianScoreOfIncompleteLines);
    }

    private static Parser<char> GetCharacterParsingOptions() => choice(
        GetCharacterSetParser('(', ')'),
        GetCharacterSetParser('{', '}'),
        GetCharacterSetParser('[', ']'),
        GetCharacterSetParser('<', '>'));

    private static Parser<char> GetCharacterSetParser(char startChar, char endChar) =>
        from start in ch(startChar)
        from intermediate in many(GetCharacterParsingOptions())
        from end in either(ch(endChar), endOfLine)
        select end;

    private static string GetMissingCharFromParseResult(ParserResult<Seq<char>> parseResult) =>
        parseResult.Reply.Error.Expected[4].Replace("\'", string.Empty);

    private static double CalculateNewScore(double score, string missingChar) =>
        score * 5 + TryGetPuzzleTwoScore(missingChar).Match(value => value, () => 0);

    private static Option<int> TryGetPuzzleOneScore(string input) => TryGetPuzzleScore(input, PuzzleOneScoreMapping);

    private static Option<int> TryGetPuzzleTwoScore(string input) => TryGetPuzzleScore(input, PuzzleTwoScoreMapping);

    private static Option<int> TryGetPuzzleScore(string input, Dictionary<string, int> mapping)
    {
        var (key, value) = mapping.FirstOrDefault(pair => input.Contains(pair.Key));
        return !string.IsNullOrWhiteSpace(key)
            ? Prelude.Some(value)
            : Prelude.None;
    }

    private static Seq<string> GetParsedInput() => FileProvider.GetAllLines("Day10.input.txt");
}