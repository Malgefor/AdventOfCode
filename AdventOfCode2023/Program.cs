// @formatter:off

using System.Diagnostics;

using AdventOfCode.Generic;

using LanguageExt;

(from puzzleDay in AllPuzzleDays()
 let timer = Stopwatch.StartNew()
 from answer in puzzleDay.PuzzleResults()
 select LogResult(puzzleDay.GetType().Name, answer, timer.Elapsed))
.Sequence()
.RunUnit();

Console.ReadKey();

IEnumerable<IPuzzleDay> AllPuzzleDays()
{
    return typeof(Program)
        .Assembly
        .GetTypes()
        .Where(type => typeof(IPuzzleDay).IsAssignableFrom(type) && !type.IsAbstract)
        .Select(type => Activator.CreateInstance(type) as IPuzzleDay)
        .OrderBy(day => day!.DayNumber);
}

Eff<Unit> LogResult(string puzzleDayName, PuzzleResult puzzleResult, TimeSpan elapsedMilliseconds)
{
    Console.WriteLine($"---{puzzleDayName}---");
    Console.WriteLine($"Answer to puzzle {puzzleResult.PuzzleNumber}: {puzzleResult.PuzzleAnswer}; Calculated in {elapsedMilliseconds:G}");
    Console.WriteLine();

    return Eff<Unit>.Success(Unit.Default);
}

// @formatter:on