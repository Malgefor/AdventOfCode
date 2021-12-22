using LanguageExt;

namespace AdventOfCode2021;

public static class SeqExtensions
{
    public static T GetMedian<T>(this Seq<T> source)
    {
        return source
            .OrderBy(Prelude.identity)
            .Skip(source.Count() / 2)
            .First();
    }
}