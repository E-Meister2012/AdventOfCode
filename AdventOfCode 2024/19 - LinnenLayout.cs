using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2024
{
    internal class LinnenLayout
    {
        static bool isSecond;
        static Dictionary<string, (bool, long)> cache = new();
        static string[] towels;
        public static long GetInput()
        {
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Linnen Layout");
            Queue<string> fileQueue = InputGatherer.GetInputs("19 - LinnenLayout");
            Stopwatch watch = new Stopwatch();
            List<string> patterns = new();
            watch.Restart();
            towels = fileQueue.Dequeue().Split(',', StringSplitOptions.TrimEntries).ToArray();
            while(fileQueue.Count > 0)
            {
                patterns.Add(fileQueue.Dequeue());
            }

            foreach(string pattern in patterns)
            {
                var canMake = CanMakeTowel(pattern);
                if(canMake.Item1)
                    result += isSecond ? canMake.Item2 : 1;
            }
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static (bool, long) CanMakeTowel(string pattern)
        {
            if(cache.ContainsKey(pattern))
                return cache[pattern];
            else
                cache[pattern] = (false, 0);
            bool result = false;
            if (towels.Contains(pattern))
            {
                long count = cache[pattern].Item2 + 1;
                cache[pattern] = (true, count);
            }
            foreach (string towel in towels.Where(t => t.Length < pattern.Length && pattern.Substring(0,t.Length) == t))
            {
                string remainder = pattern.Substring(towel.Length);
                var canMake = CanMakeTowel(remainder);
                if (canMake.Item1)
                {
                    
                    cache[pattern] = (true, cache[pattern].Item2 + canMake.Item2);
                }
            }
            return cache[pattern];
        }
    }
}
