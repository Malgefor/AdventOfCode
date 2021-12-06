// @formatter:off

using AdventOfCode2021;

using LanguageExt;

(from puzzleDay in GetAllPuzzleDays()
 from answer in puzzleDay.PuzzleResults()
 select LogResult(puzzleDay.GetType().Name, answer))
.Sequence()
.RunUnit();

Console.ReadKey();

IEnumerable<IPuzzleDay?> GetAllPuzzleDays() => typeof(Program)
    .Assembly
    .GetTypes()
    .Where(type => typeof(IPuzzleDay).IsAssignableFrom(type) && !type.IsAbstract)
    .Select(type => Activator.CreateInstance(type) as IPuzzleDay);

Eff<Unit> LogResult(string puzzleDayName, PuzzleResult puzzleResult)
{
    Console.WriteLine($"---{puzzleDayName}---");
    Console.WriteLine($"Answer to puzzle {puzzleResult.PuzzleNumber}: {puzzleResult.PuzzleAnswer}");
    Console.WriteLine();

    return Eff<Unit>.Success(Unit.Default);
}