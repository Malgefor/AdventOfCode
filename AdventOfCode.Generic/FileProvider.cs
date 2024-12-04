using System.Diagnostics;
using System.Reflection;

using LanguageExt;

namespace AdventOfCode.Generic;

public class FileProvider
{
    public static Seq<string> GetAllLines(string splitOn = "\r\n")
    {
        var stackFrame = new StackFrame(1);
        var callingMethod = stackFrame.GetMethod();
        var callingClassType = callingMethod!.DeclaringType;
        var callingAssembly = Assembly.GetCallingAssembly();
        var inputFilePath = callingAssembly
            .GetManifestResourceNames()
            .First(x => x.EndsWith(callingClassType.Namespace + ".input.txt"));
        var resourceStream = callingAssembly.GetManifestResourceStream(inputFilePath);

        using var reader = new StreamReader(resourceStream);

        var allLines = reader
            .ReadToEnd()
            .Split(splitOn)
            .ToSeq();
        return allLines;
    }
}