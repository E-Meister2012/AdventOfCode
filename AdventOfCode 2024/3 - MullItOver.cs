using AdventOfCodeBase;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode_2024
{
    internal class MullItOver
    {
        public static int GetInput()
        {
            string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
            string doPattern = @"do\(\)";
            string dontPattern = @"don\'t\(\)";
            int result = 0;
            string input = "";
            Queue<string> fileQueue = InputGatherer.GetInputs("3 - MullItOver");
            bool isSecond = InputGatherer.GetUserInput("Mull it Over");
            while (fileQueue.Count > 0)
                input += fileQueue.Dequeue();
            MatchCollection matches = Regex.Matches(input, pattern);
            MatchCollection dos = Regex.Matches(input, doPattern);
            MatchCollection donts = Regex.Matches(input, dontPattern);

            foreach (Match match in matches)
            {
                if (!isSecond || IsValid(match, dos, donts))
                result += GetValue(match);
            }
        return result;
    }
        static int GetValue(Match match)
        {
            int value1 = int.Parse(match.Groups[1].Value);
            int value2 = int.Parse(match.Groups[2].Value);
            Console.WriteLine($"{value1}  *  {value2}");
            return value1 * value2;
        }
        static bool IsValid(Match currentMatch, MatchCollection dos, MatchCollection donts)
        {
            var dontMatch = donts.Cast<Match>().Where(m => m.Index < currentMatch.Index).LastOrDefault();
            var doMatch = dos.Cast<Match>().Where(m => m.Index < currentMatch.Index).LastOrDefault();
            if(doMatch is null)
                return dontMatch is null;
            else
                return dontMatch is null || doMatch.Index > dontMatch.Index;
        }
    }
}