// using System.Text.RegularExpressions;
//
// using AdventOfCode.Generic;
//
// using LanguageExt;
//
// namespace AdventOfCode2023.Day5;
//
// public class DayFive : IPuzzleDay
// {
//     public int DayNumber => 5;
//
//     public IEnumerable<PuzzleResult> PuzzleResults()
//     {
//         var parsedInput = GetParsedInput();
//         
//         var puzzleOneResult = parsedInput
//             .Seeds
//             .Map(
//                 seed => parsedInput.AlmanacEntries
//                     .Fold(
//                         seed,
//                         (acc, ranges) => ranges
//                             .Find(
//                                 range => acc >= range.SourceRangeStart
//                                          && acc < range.SourceRangeStart + range.RangeLength)
//                             .Match(
//                                 range => range.DestinationRangeStart + Math.Abs(acc - range.SourceRangeStart),
//                                 () => acc)))
//             .Min();
//         
//         yield return new PuzzleResult(1, puzzleOneResult);
//
//         // var seedRanges = parsedInput
//         //     .Seeds
//         //     .Chunk(2)
//         //     .Map(seedRange => Prelude.Seq(seedRange[0], seedRange[0] + seedRange[1]))
//         //     .ToSeq();
//         //
//         // var minSeedRange = seedRanges
//         //     .Map(
//         //         seedRange =>
//         //         {
//         //             return parsedInput.AlmanacEntries
//         //                 .Fold(
//         //                     seedRange,
//         //                     (acc, ranges) =>
//         //                     {
//         //                         return ranges
//         //                             .Find(
//         //                                 range => acc >= range.SourceRangeStart
//         //                                          && acc < range.SourceRangeStart + range.RangeLength)
//         //                             .Match(
//         //                                 range => range.DestinationRangeStart + Math.Abs(acc - range.SourceRangeStart),
//         //                                 () => acc);
//         //                     })
//         //         })
//         //
//         // yield return new PuzzleResult(2, actualMin);
//
//         var seeds = FileProvider.GetAllLines("Day5.input.txt")[0]
//             .Split(' ')
//             .Skip(1)
//             .Select(x => long.Parse(x))
//             .ToList();
//         var maps = new List<List<(long from, long to, long adjustment)>>();
//         List<(long from, long to, long adjustment)>? currmap = null;
//         foreach (var line in FileProvider.GetAllLines("Day5.input.txt").Skip(2))
//         {
//             if (line.EndsWith(':'))
//             {
//                 currmap = new List<(long from, long to, long adjustment)>();
//                 continue;
//             }
//
//             if (line.Length == 0 && currmap != null)
//             {
//                 maps.Add(currmap!);
//                 currmap = null;
//                 continue;
//             }
//
//             var nums = line.Split(' ').Select(x => long.Parse(x)).ToArray();
//             currmap!.Add((nums[1], nums[1] + nums[2] - 1, nums[0] - nums[1]));
//         }
//
//         if (currmap != null)
//         {
//             maps.Add(currmap);
//         }
//
// // Part 2
//         var ranges = new List<(long from, long to)>();
//         for (var i = 0; i < seeds.Count; i += 2)
//             ranges.Add((from: seeds[i], to: seeds[i] + seeds[i + 1] - 1));
//
//         foreach (var map in maps)
//         {
//             var orderedmap = map.OrderBy(x => x.from).ToList();
//
//             var newranges = new List<(long from, long to)>();
//             foreach (var r in ranges)
//             {
//                 var range = r;
//                 foreach (var mapping in orderedmap)
//                 {
//                     if (range.from < mapping.from)
//                     {
//                         newranges.Add((range.from, Math.Min(range.to, mapping.from - 1)));
//                         range.from = mapping.from;
//                         if (range.from > range.to)
//                         {
//                             break;
//                         }
//                     }
//
//                     if (range.from <= mapping.to)
//                     {
//                         newranges.Add(
//                             (range.from + mapping.adjustment, Math.Min(range.to, mapping.to) + mapping.adjustment));
//                         range.from = mapping.to + 1;
//                         if (range.from > range.to)
//                         {
//                             break;
//                         }
//                     }
//                 }
//
//                 if (range.from <= range.to)
//                 {
//                     newranges.Add(range);
//                 }
//             }
//
//             ranges = newranges;
//         }
//
//         yield return new PuzzleResult(2, ranges.Min(r => r.from));
//     }
//
//     private record AlmanacRangeStarts(double DestinationRangeStart, double SourceRangeStart, double RangeLength);
//
//     private record AlmanacRange(Arr<double> DestinationRange, Arr<double> SourceRange);
//
//     private static (Seq<double> Seeds, Seq<Seq<AlmanacRangeStarts>> AlmanacEntries) GetParsedInput()
//     {
//         const string pattern = @"\b\d+\b";
//
//         var allLines = FileProvider.GetAllLines("Day5.input.txt", "\r\n\r\n");
//         var seeds = Regex.Matches(allLines.Head, pattern).ToSeq().Map(x => double.Parse(x.Value));
//
//         return (seeds, allLines.Tail.Map(ParseAlmanacEntries));
//     }
//
//     private static Seq<AlmanacRangeStarts> ParseAlmanacEntries(string groupOfLines) => groupOfLines
//         .Split("\r\n")
//         .Tail()
//         .Map(line => line.Split(' '))
//         .Map(
//             splitLine => new AlmanacRangeStarts(
//                 double.Parse(splitLine[0]),
//                 double.Parse(splitLine[1]),
//                 double.Parse(splitLine[2])))
//         .OrderBy(range => range.DestinationRangeStart)
//         .ToSeq();
// }

