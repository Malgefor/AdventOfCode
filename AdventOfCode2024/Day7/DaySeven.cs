using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2024.Day7;

public class DaySeven : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider
            .GetAllLines()
            .Map(x => x.Split(": ").ToSeq())
            .Map(
                x => new Equation(
                    long.Parse(x.Head),
                    x.Tail.Head
                        .Split(" ")
                        .Map(long.Parse)
                        .Rev()
                        .ToSeq()
                        .Strict()));

        var puzzleOneResult = Solve(parsedData, ['*', '+']);
        yield return new PuzzleResult(puzzleOneResult);

        var puzzleTwoResult = Solve(parsedData, ['*', '+', '|']);
        yield return new PuzzleResult(puzzleTwoResult);
    }

    private long Solve(Seq<Equation> parsedData, char[] operatorKeys)
    {
        return parsedData
            .Map(
                equation =>
                {
                    var amountOfOperators = equation.Numbers.Count - 1;
                    var stack = new Stack<long>();

                    return GetPermutations(amountOfOperators, operatorKeys)
                        .Map(
                            ops =>
                            {
                                foreach (var number in equation.Numbers)
                                {
                                    stack.Push(number);
                                }

                                var value = ops.Fold(
                                    stack,
                                    (nums, func) =>
                                    {
                                        var result = this.operators[func](nums.Pop(), nums.Pop());
                                        nums.Push(result);

                                        return nums;
                                    });
                                return value.Pop();
                            })
                        .Find(result => result == equation.TestValue);
                })
            .Somes()
            .Sum();
    }

    private readonly Dictionary<char, Func<long, long, long>> operators =
        new()
        {
            { '*', (d1, d2) => d1 * d2 },
            { '+', (d1, d2) => d1 + d2 },
            { '|', (d1, d2) => d1 * (long)Math.Pow(10, (int)Math.Floor(Math.Log10(d2)) + 1) + d2 }
        };

    private record Equation(long TestValue, Seq<long> Numbers);

    private static IEnumerable<List<char>> GetPermutations(int maxLength, char[] letters)
    {
        var queue = new Queue<List<char>>();
        queue.Enqueue([]);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var letter in letters)
            {
                var next = new List<char>(current) { letter };

                if (next.Count == maxLength)
                {
                    yield return next;
                }
                else
                {
                    queue.Enqueue(next);
                }
            }
        }
    }
}