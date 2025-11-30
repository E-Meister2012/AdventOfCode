using AdventOfCodeBase;
using System.Diagnostics;

namespace AdventOfCode_2024
{
    internal class MoneyMarket
    {
        static bool isSecond;
        static int changes;
        static readonly Dictionary<(long, (long,long,long,long)), long> dictionary = new();
        static readonly Dictionary<(long, long, long, long), long> sumValue = new();
        public static long GetInput()
        {
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Money Market");
            Queue<string> fileQueue = InputGatherer.GetInputs("22 - MoneyMarket");
            List<int> values = new();
            Stopwatch watch = new();
            while (changes == 0)
            {
                Console.Write("How many times do the prices change?  ");
                int.TryParse(Console.ReadLine(), out changes);
            }

            watch.Restart();
            while(fileQueue.Count > 0)
                values.Add(int.Parse(fileQueue.Dequeue()));

            for(int i = 0; i < values.Count; i++)
            {
                long newNumber = CalculateNewNumber(values[i], changes, i, new List<long>());
                //Console.WriteLine($"{i}: {newNumber}");
                result += newNumber;
            }
            var bestKVP = sumValue.MaxBy(kvp => kvp.Value);
            long bestValue = bestKVP.Value;
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return isSecond ? bestValue : result;
        }
        static long CalculateNewNumber(long previous, int runs, int number, List<long> previousChanges)
        {
            long startingValue = previous;
            if(runs == 0)
                return previous;
            //First Step
            long result = previous * 64;
            result = Prune(Mix(previous, result));
            previous = result;
            //Second step
            result = previous / 32;
            result = Prune(Mix(previous, result));
            previous = result;
            //Third step
            result = previous * 2048;
            result = Prune(Mix(previous, result));

            runs--;
            previousChanges = InsertValue(result % 10 - startingValue % 10, previousChanges.ToList());
            AddToDict(previousChanges.ToList(), number, result % 10);
            return CalculateNewNumber(result, runs, number, previousChanges.ToList());
        }

        static List<long> InsertValue(long value, List<long> toInsert)
        {
            if(toInsert.Count == 4)
                toInsert.RemoveAt(0);
            toInsert.Add(value); 
            return toInsert;
        }

        static long Mix(long previous, long result)
        {
            return previous ^ result;
        }
        static long Prune(long number)
        {
            return number % 16777216;
        }
        static void AddToDict(List<long> previous, long number, long input)
        {
            if (previous.Count < 4)
                return;
            var tuple = (previous[0], previous[1], previous[2], previous[3]);
           if(dictionary.ContainsKey((number, tuple)))
           {
                long currentValue = dictionary[(number, tuple)];
                if(currentValue > input)
                {
                    dictionary[(number, tuple)] = input;
                }
           }
           else
            {
                dictionary[(number, tuple)] = input;
                if (sumValue.ContainsKey(tuple))
                {
                    sumValue[tuple] += input;
                }
                else
                    sumValue[tuple] = input;
            }
        }
    }
}