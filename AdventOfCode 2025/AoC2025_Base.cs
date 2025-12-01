using AdventOfCodeBase;
using System.Diagnostics;

namespace AdventOfCode_2025
{
    internal class AoC2025_Base
    {
        static bool isSecond;
        public static int GetInput()
        {
            int result = 0;
            isSecond = InputGatherer.GetUserInput("NAME");
            Queue<string> fileQueue = InputGatherer.GetInputs("DAY - NAME");
            Stopwatch watch = new();
            watch.Restart();
            //Handle Inputs

            //Implement Puzzle
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
    }
}
