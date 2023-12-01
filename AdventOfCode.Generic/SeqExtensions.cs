using LanguageExt;

namespace AdventOfCode.Generic;

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