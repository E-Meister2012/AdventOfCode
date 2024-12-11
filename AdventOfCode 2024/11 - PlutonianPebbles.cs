using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class PlutonianPebbles
    {
        public static long GetInput() 
        {
            long result = 0;
            bool isSecond = InputGatherer.GetUserInput("Plutonian Pebbles");
            Queue<String> fileQueue = InputGatherer.GetInputs("11 - PlutonianPebbles");
            List<long> pebbles = fileQueue.Dequeue().Split(' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(long.Parse).ToList();
            Dictionary<long, long> pebbleAmounts = new();
            foreach (long pebble in pebbles)
            {
                AddValue(pebbleAmounts, pebble, 1);
            }
            Console.Write("How many blinks?  ");
            long blinks = long.Parse(Console.ReadLine());
            for (int i = 0; i < blinks; i++)
                pebbleAmounts = GetNewPebbles(pebbleAmounts);
            foreach (long key in pebbleAmounts.Keys)
                result += pebbleAmounts[key];
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