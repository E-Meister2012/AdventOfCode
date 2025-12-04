using AdventOfCodeBase;
using System.Diagnostics;


namespace AdventOfCode_2025
{
    internal class GiftShop
    {
        static bool isSecond;

        public static long GetInput()
        {
            Dictionary<long, long> inputs = [];
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Secret Entrance");
            Queue<string> fileQueue = InputGatherer.GetInputs("2 - GiftShop");
            Stopwatch watch = new();
            watch.Restart();
            while (fileQueue.Count > 0)
            {
                string input = fileQueue.Dequeue();
                foreach (string s in input.Split([','], StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] temp = s.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    inputs.Add(long.Parse(temp[0]), long.Parse(temp[1]));
                }
            }
            foreach (var (key, value) in inputs)
                result += Solve(key, value);
            //Implement Puzzle
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        static long Solve(long startingNumber, long endingNumber)
        {
            long result = 0;
            for (long i = startingNumber; i <= endingNumber; i++)
            {
                string s = i.ToString();
                long length = s.Length;
                long divider = (long)Math.Pow(10, length / 2);
                long firstHalf = i / divider;
                long secondHalf = i % divider;
                if (firstHalf == secondHalf)
                    result += i;
                else if (isSecond)
                    if (CheckAll(s, length / 2))
                        result += i;
            }
            return result;
        }
        static bool CheckAll(string s, long length)
        {
            //Check divisors
            string editedS;
            for (int i = 1; i <= length; i++)
            {
                editedS = s;
                string ending = s[^i..];
                while (editedS.EndsWith(ending))
                    editedS = editedS[..^ending.Length];
                if (editedS.Length == 0)
                    return true;
            }
            return false;
        }
    }
}