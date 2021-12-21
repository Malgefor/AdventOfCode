using LanguageExt;
using LanguageExt.Parsec;

using static LanguageExt.Parsec.Prim;
using static LanguageExt.Parsec.Char;

namespace AdventOfCode2021.Day10;

public class DayTen : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var input = GetParsedInput();

        var options = GetOptions();
        var parser = many(options);
        var results = input
            .Map(line => parse(parser, line))
            .Filter(parserResult => parserResult.IsFaulted)
            .Sum(parserResult => { return GetScore(parserResult.Reply.Error.Msg); });

        yield return new PuzzleResult(1, results);
    }

    private static Parser<char> GetOptions()
    {
        return choice(GetParser2('(', ')'), GetParser2('{', '}'), GetParser2('[', ']'), GetParser2('<', '>'));
    }

    private static Parser<char> GetParser2(char startChar, char endChar)
    {
        return
            from start in ch(startChar)
            from intermediate in many(GetOptions())
            from end in either(ch(endChar), endOfLine)
            select end;
    }

    private static Seq<string> GetParsedInput()
    {
        return FileProvider
            .GetAllLines("Day10.input.txt");
    }

    private static int GetScore(string input)
    {
        if (input.Contains(")"))
        {
            return 3;
        }

        if (input.Contains("]"))
        {
            return 57;
        }

        if (input.Contains("}"))
        {
            return 1197;
        }

        if (input.Contains(">"))
        {
            return 25137;
        }

        if (input.Contains("end of stream"))
        {
            return 0;
        }

        throw new Exception();
    }
}