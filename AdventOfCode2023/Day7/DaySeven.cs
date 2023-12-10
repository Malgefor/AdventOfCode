using AdventOfCode.Generic;

using LanguageExt;

namespace AdventOfCode2023.Day7;

public class DaySeven : IPuzzleDay
{
    public int DayNumber => 7;

    public IEnumerable<PuzzleResult> PuzzleResults()
    {
        var parsedInput = GetParsedInput();
        var puzzleAnswerOne = parsedInput
            .Order(new HandComparer(GetCardRankPuzzleOne))
            .Map((index, hand) => hand.Bid * (index + 1))
            .Sum();

        yield return new PuzzleResult(puzzleAnswerOne);

        var puzzleAnswerTwo = parsedInput
            .Order(new HandComparer(GetCardRankPuzzleTwo, true))
            .Map((index, hand) => hand.Bid * (index + 1))
            .Sum();

        yield return new PuzzleResult(puzzleAnswerTwo);
    }

    private record HandWithBid(string Cards, int Bid)
    {
        private int rank;
        private int rankWithJokers;

        public int Rank(bool useJoker = false)
        {
            if (useJoker && this.Cards.Contains('J'))
            {
                return this.rankWithJokers != 0
                    ? this.rankWithJokers
                    : this.rankWithJokers = AllHandsRanked()
                        .FoldWhile(
                            0,
                            (acc, hand) =>
                            {
                                var isMatch = hand.Check(this.Cards);
                                if (isMatch)
                                {
                                    return hand.Score;
                                }

                                var mostCommonChar =
                                    this.Cards
                                        .GroupBy(x => x)
                                        .OrderByDescending(x => x.Count())
                                        .First(x => x.Key != 'J')
                                        .Key;

                                var replacedHand = this.Cards.Replace('J', mostCommonChar);

                                return hand.Check(replacedHand)
                                    ? hand.Score
                                    : acc;
                            },
                            acc => acc == 0);
            }

            return this.rank != 0
                ? this.rank
                : this.rank = AllHandsRanked()
                    .First(hand => hand.Check(this.Cards))
                    .Score;
        }

        private static Seq<(Func<string, bool> Check, int Score)> AllHandsRanked()
        {
            return Prelude.Seq<(Func<string, bool> Check, int Score)>(
                (IsFiveOfAKind, 7),
                (IsFourOfAKind, 6),
                (IsFullHouse, 5),
                (IsThreeOfAKind, 4),
                (IsTwoPair, 3),
                (IsOnePair, 2),
                (_ => true, 1));
        }

        private static bool IsTwoPair(string input) => input.GroupBy(c => c).Count(group => group.Count() == 2) == 2;

        private static bool IsOnePair(string input) => ContainsGroupingOfXAmount(input, 2);

        private static bool IsThreeOfAKind(string input) => ContainsGroupingOfXAmount(input, 3);

        private static bool IsFourOfAKind(string input) => ContainsGroupingOfXAmount(input, 4);

        private static bool IsFiveOfAKind(string input) => ContainsGroupingOfXAmount(input, 5);

        private static bool IsFullHouse(string input) => IsThreeOfAKind(input) && IsOnePair(input);

        private static bool ContainsGroupingOfXAmount(string input, int expectedAmount) =>
            input.GroupBy(c => c).Any(group => group.Count() == expectedAmount);
    }

    private static int GetCardRankPuzzleOne(char card) => card switch
    {
        'A' => 12,
        'K' => 11,
        'Q' => 10,
        'J' => 9,
        'T' => 8,
        '9' => 7,
        '8' => 6,
        '7' => 5,
        '6' => 4,
        '5' => 3,
        '4' => 2,
        '3' => 1,
        '2' => 0
    };

    private static int GetCardRankPuzzleTwo(char card) => card switch
    {
        'A' => 12,
        'K' => 11,
        'Q' => 10,
        'T' => 9,
        '9' => 8,
        '8' => 7,
        '7' => 6,
        '6' => 5,
        '5' => 4,
        '4' => 3,
        '3' => 2,
        '2' => 1,
        'J' => 0
    };

    private static Seq<HandWithBid> GetParsedInput() => FileProvider
        .GetAllLines()
        .Map(
            line =>
            {
                var split = line.Split(' ');
                return new HandWithBid(split[0], int.Parse(split[1]));
            });

    private class HandComparer : IComparer<HandWithBid>
    {
        private readonly Func<char, int> cardRankComparer;
        private readonly bool useJoker;

        public HandComparer(Func<char, int> cardRankComparer, bool useJoker = false)
        {
            this.cardRankComparer = cardRankComparer;
            this.useJoker = useJoker;
        }

        public int Compare(HandWithBid hand1, HandWithBid hand2)
        {
            var hand1Score = hand1.Rank(this.useJoker);
            var hand2Score = hand2.Rank(this.useJoker);

            if (hand1Score > hand2Score)
            {
                return 1;
            }

            if (hand2Score > hand1Score)
            {
                return -1;
            }

            return hand1.Cards
                .Zip(hand2.Cards)
                .FoldWhile(
                    0,
                    (_, cards) =>
                    {
                        var cardRank1 = this.cardRankComparer(cards.Item1);
                        var cardRank2 = this.cardRankComparer(cards.Item2);
                        return cardRank1 > cardRank2
                            ? 1
                            : cardRank2 > cardRank1
                                ? -1
                                : 0;
                    },
                    compareResult => compareResult == 0);
        }
    }
}