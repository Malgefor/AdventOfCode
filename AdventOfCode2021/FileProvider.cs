using LanguageExt;

using Microsoft.Extensions.FileProviders;

namespace AdventOfCode2021;

public class FileProvider
{
    //TODO: use Eff<IEnumerable<string>>
    public static Seq<string> GetAllLines(string filename, string splitOn = "\r\n")
    {
        var fileProvider = new EmbeddedFileProvider(typeof(FileProvider).Assembly);
        var inputStream = fileProvider
            .GetFileInfo(filename)
            .CreateReadStream();

        using var reader = new StreamReader(inputStream);

        return reader
            .ReadToEnd()
            .Split(splitOn)
            .ToSeq();
    }
}