using AdventOfCodeBase;
using System.Diagnostics;

namespace AdventOfCode_2024
{
    internal class PlutonianPebbles
    {
        public static long GetInput() 
        {
            long result = 0;
            Console.Write("Welcome to the Plutonian Pebbles problem, how many blinks are there?  ");
            long blinks = long.Parse(Console.ReadLine());
            
            //Handle Inputs
            Queue<string> fileQueue = InputGatherer.GetInputs("11 - PlutonianPebbles");
            Stopwatch watch = new();
            watch.Restart();
            watch.Start();
            List<long> pebbles = fileQueue.Dequeue().Split(' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(long.Parse).ToList();
            Dictionary<long, long> pebbleAmounts = new();
            foreach (long pebble in pebbles)
                AddValue(pebbleAmounts, pebble, 1);

            //Implement Puzzle
            for (int i = 0; i < blinks; i++)
            {
                pebbleAmounts = GetNewPebbles(pebbleAmounts);
                if((i + 1 )% 10000 == 0)
                    Console.WriteLine($"It takes {watch.ElapsedMilliseconds}ms to go through {i + 1} blinks");
            }
                foreach (long key in pebbleAmounts.Keys)
                result += pebbleAmounts[key];
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static Dictionary<long, long> GetNewPebbles(Dictionary<long, long> previousPebbles)
        {
            Dictionary<long, long> result = new();
            foreach(long key in previousPebbles.Keys)
            {
                if (key == 0)
                    AddValue(result, 1, previousPebbles[key]);
                else if (key.ToString().Length % 2 == 0)
                {
                    string firstHalf = key.ToString();
                    string secondHalf = firstHalf.Substring(firstHalf.Length / 2);
                    firstHalf = firstHalf[0..(firstHalf.Length / 2)];
                    AddValue(result, long.Parse(firstHalf), previousPebbles[key]);
                    AddValue(result, long.Parse(secondHalf), previousPebbles[key]);
                }
                else
                    AddValue(result, key * 2024, previousPebbles[key]);
            }
            return result;
        }
        static Dictionary<long, long> AddValue(Dictionary<long, long> dictionary, long key, long value)
        {
            if(dictionary.ContainsKey(key))
                dictionary[key] += value;
            else
                dictionary.Add(key, value); 
            return dictionary;
        }
    }
}