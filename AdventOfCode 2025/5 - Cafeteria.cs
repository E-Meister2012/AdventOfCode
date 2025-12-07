using AdventOfCodeBase;
using System;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2025
{
    internal class Cafeteria
    {
        static bool isSecond;
        static List<(long start, long end)> goodFood = [];
        public static long GetInput()
        {
            string input;
            List<long> food = [];
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Cafeteria");
            Queue<string> fileQueue = InputGatherer.GetInputs("5 - Cafeteria");
            Stopwatch watch = new();
            watch.Restart();
            while((input = fileQueue.Dequeue()).Contains('-'))
            {
                string[] splitInput = input.Split('-');
                goodFood.Add((long.Parse(splitInput[0]), long.Parse(splitInput[1])));
            }
            food.Add(long.Parse(input));
            while(fileQueue.Count > 0)
            {
                input = fileQueue.Dequeue();
                food.Add(long.Parse(input));
            }

            if(!isSecond)
                foreach (long ingredient in food)
                {
                    if (IsFresh(ingredient))
                        result++;
                }
            else
            {
                goodFood = [.. goodFood.OrderBy(i => i.start)];
                List<(long start, long end)> merged = MergeIngredients();
                while(goodFood.Count != merged.Count)
                {
                    goodFood = merged;
                    merged = MergeIngredients();
                }
                result = merged.Sum(r => r.end - r.start + 1);
            }

            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        static bool IsFresh(long ingredient)
        {
            foreach (var (start, end) in goodFood)
                if (start <= ingredient && ingredient <= end)
                    return true;
            return false;
        }
        static List<(long, long)> MergeIngredients()
        {
            List<(long start, long end)> merged = [goodFood[0]];

            foreach (var range in goodFood[1..])
            {
                var (start, end) = merged[^1];
                if (range.start <= end && range.end >= start)
                    merged[^1] = (start, Math.Max(end, range.end));
                else
                    merged.Add(range);
            }
            return merged;
        }

    }
}
