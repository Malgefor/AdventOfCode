using System.Text.RegularExpressions;

using AdventOfCode.Generic;

namespace AdventOfCode2022.Day5;

public class DayFive : IPuzzleDay
{
    public int DayNumber => 5;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var input = GetParsedInput();

        var (initialStacks, instructions) = input;

        var orderedStacks = instructions
            .Fold(
                initialStacks,
                (stacks, instr) =>
                {
                    var fromStack = initialStacks[instr.From - 1];
                    var toStack = initialStacks[instr.To - 1];

                    Enumerable
                        .Range(1, instr.Amount)
                        .Iter(
                            _ =>
                            {
                                var popped = fromStack.Pop();
                                toStack.Push(popped);
                            });
                    return stacks;
                })
            .ToList();

        var puzzleOneAnswer = GetAnswer(orderedStacks);

        var orderedStacksPuzzleTwo = instructions
            .Fold(
                initialStacks,
                (stacks, instr) =>
                {
                    var fromStack = initialStacks[instr.From - 1];
                    var toStack = initialStacks[instr.To - 1];
                    var inter = new List<char>();
                    Enumerable
                        .Range(1, instr.Amount)
                        .Iter(
                            _ =>
                            {
                                var popped = fromStack.Pop();
                                inter.Add(popped);
                            });
                    inter.Rev().Iter(x => toStack.Push(x));

                    return stacks;
                })
            .ToList();

        var puzzleTwoAnswer = GetAnswer(orderedStacksPuzzleTwo);

        return new[] { new PuzzleResult(1, puzzleOneAnswer), new PuzzleResult(2, puzzleTwoAnswer) };
    }

    private static string GetAnswer(List<Stack<char>> orderedStacks)
    {
        return orderedStacks
            .Map(x => x.Pop())
            .Fold(string.Empty, (result, currentChar) => result + currentChar);
    }

    private static (Stack<char>[] initialStacks, List<(int Amount, int From, int To)> instructions) GetParsedInput()
    {
        var input = FileProvider.GetAllLines("Day5.input.txt", "\r\n\r\n");
        var initialStacks = ParseStacks(input[0].Split("\r\n"));

        var instructionRegex = new Regex("move\\s([0-9]+)\\sfrom\\s([0-9]+)\\sto\\s([0-9]+)", RegexOptions.Compiled);
        var instructions = input[1]
            .Split("\r\n")
            .Map(
                x =>
                {
                    var result = instructionRegex.Match(x);
                    return (int.Parse(result.Groups[1].Value), int.Parse(result.Groups[2].Value),
                        int.Parse(result.Groups[3].Value));
                })
            .ToList();

        return (initialStacks, instructions);
    }

    private static Stack<char>[] ParseStacks(string[] initialStacks)
    {
        var stacks = new Stack<char>[9];
        foreach (var initialStack in initialStacks.Take(initialStacks.Length - 1))
        {
            var charArray = initialStack.ToCharArray();
            var column = 0;
            for (var i = 0; i < charArray.Length; i++)
            {
                if (charArray[i] == '[' || charArray[i] == ']')
                {
                    continue;
                }

                if (charArray[i] == ' ')
                {
                    var nextIndex = i + 1;
                    if (charArray.Length >= nextIndex && charArray[nextIndex] == ' ')
                    {
                        i += 4;
                        column += 1;
                    }

                    continue;
                }

                var stack = stacks.ElementAtOrDefault(column);
                if (stack == null)
                {
                    stack = new Stack<char>();
                    stacks[column] = stack;
                }

                stack.Push(charArray[i]);
                column += 1;
            }
        }

        return stacks
            .Map(x => new Stack<char>(x))
            .ToArray();
    }
}