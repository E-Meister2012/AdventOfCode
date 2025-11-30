using AdventOfCodeBase;
using System.Diagnostics;

namespace AdventOfCode_2024
{
    internal class CodeChronicle
    {
        static List<int[]> keys = new();
        static List<int[]> locks = new();

        static bool isSecond;
        public static int GetInput()
        {
            List<string> pins = new();
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Code Chronicle");
            Queue<string> fileQueue = InputGatherer.GetInputs("25 - CodeChronicle");
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            while (fileQueue.Count > 0)
            {
                pins.Clear();

                string input = fileQueue.Dequeue();
                for (int i = 0; i < 5; i++)
                    pins.Add(fileQueue.Dequeue());

                if (input.StartsWith('#'))
                    locks.Add(GetPinHeights(pins));
                else
                    keys.Add(GetPinHeights(pins));
                fileQueue.Dequeue();
            }

            foreach (int[] l in locks)
            {
                result += CheckKeyCompatability(l);
            }
            //Implement Puzzle
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        static int CheckKeyCompatability(int[] locks)
        {
            int result = 0;
            bool canUseKey = false;
            foreach (int[] key in keys)
            {
                canUseKey = true;
                for(int i = 0; i < key.Length; i++)
                    if (key[i] + locks[i] > 5)
                        canUseKey = false;
                if(canUseKey)
                    result++;
            }
            return result;
        }

        static int[] GetPinHeights(List<string> pins)
        {
            int[] result = new int[5] { 0,0,0,0,0};
            foreach (string pin in pins)
            {
                for (int p = 0; p < pin.Length; p++)
                {
                    if (pin[p] == '#')
                        result[p]++;
                }
            }
            return result;
        }

    }
}
