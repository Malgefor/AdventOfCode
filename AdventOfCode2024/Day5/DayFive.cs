using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2024.Day5;

public class DayFive : IPuzzleDay
{
    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedData = FileProvider.GetAllLines("\r\n\r\n");
        var orderingRules = parsedData.Head
            .Split("\r\n")
            .Map(x => x.Split("|"))
            .Map(x => new OrderingRule(int.Parse(x[0]), int.Parse(x[1])))
            .ToSeq();

        var updates = parsedData.Tail[0]
            .Split("\r\n")
            .Map(y => y.Split(',').Map(int.Parse).ToArray())
            .ToSeq();

        var correctUpdates =
            from update in updates
            let relevantOrderingRules = GetRelevantOrderingRules(orderingRules, update)
            where IsUpdateCorrect(relevantOrderingRules, update)
            select update;

        yield return new PuzzleResult(correctUpdates.Map(GetMiddlePage).Sum());

        var incorrectUpdates = updates.Filter(update => !correctUpdates.Contains(update)).ToSeq();
        foreach (var incorrectUpdate in incorrectUpdates)
        {
            var relevantOrderingRules = GetRelevantOrderingRules(orderingRules, incorrectUpdate);

            var isCorrect = false;
            while (!isCorrect)
            {
                foreach (var page in incorrectUpdate)
                {
                    var orderingRulesForPage = relevantOrderingRules.Filter(x => x.First == page);
                    foreach (var orderingRule in orderingRulesForPage)
                    {
                        var firstIndex = Array.IndexOf(incorrectUpdate, orderingRule.First);
                        var secondIndex = Array.IndexOf(incorrectUpdate, orderingRule.Second);
                        if (firstIndex > secondIndex)
                        {
                            (incorrectUpdate[firstIndex], incorrectUpdate[secondIndex]) = (incorrectUpdate[secondIndex],
                                incorrectUpdate[firstIndex]);
                        }
                    }
                }

                isCorrect = IsUpdateCorrect(relevantOrderingRules, incorrectUpdate);
            }
        }

        yield return new PuzzleResult(incorrectUpdates.Map(GetMiddlePage).Sum());
    }

    private static int GetMiddlePage(int[] update)
    {
        return update[update.Length / 2];
    }

    private static bool IsUpdateCorrect(Seq<OrderingRule> relevantOrderingRules, int[] update)
    {
        return relevantOrderingRules.All(x => Array.IndexOf(update, x.First) < Array.IndexOf(update, x.Second));
    }

    private static Seq<OrderingRule> GetRelevantOrderingRules(Seq<OrderingRule> orderingRules, int[] update)
    {
        return orderingRules.Filter(x => update.Contains(x.First) && update.Contains(x.Second));
    }

    private record OrderingRule(int First, int Second);
}