// @formatter:off

using System.Diagnostics;

using AdventOfCode.Generic;

using LanguageExt;

(from puzzleDay in AllPuzzleDays()
 let timer = Stopwatch.StartNew()
 from answer in puzzleDay.PuzzleResults().Map((index, answer) => (PuzzleNumber: index + 1, answer))
 select LogResult(puzzleDay.GetType().Name, answer.PuzzleNumber, answer.answer, timer.Elapsed))
.Sequence()
.RunUnit();

Console.ReadKey();
return;

IEnumerable<IPuzzleDay> AllPuzzleDays()
{
    return typeof(Program)
        .Assembly
        .GetTypes()
        .Where(type => typeof(IPuzzleDay).IsAssignableFrom(type) && !type.IsAbstract)
        .Select(type => Activator.CreateInstance(type) as IPuzzleDay)
        .OrderBy(day => day!.GetType().Namespace);
}

Eff<Unit> LogResult(string puzzleDayName, int puzzleNumber, PuzzleResult puzzleResult, TimeSpan elapsed)
{
    Console.WriteLine($"---{puzzleDayName}---");
    Console.WriteLine($"Answer to puzzle {puzzleNumber}: {puzzleResult.PuzzleAnswer}; Calculated in {elapsed:c} ({elapsed.TotalMilliseconds}ms)");
    Console.WriteLine();

    return Eff<Unit>.Success(Unit.Default);
}

// @formatter:on