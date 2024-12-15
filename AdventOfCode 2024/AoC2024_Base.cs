using AdventOfCodeBase;
using System.Diagnostics;

namespace AdventOfCode_2024
{
    internal class AoC2024_Base
    {
        static bool isSecond;
        public static int GetInput()
        {
            int result = 0;
            isSecond = InputGatherer.GetUserInput("NAME");
            Queue<String> fileQueue = InputGatherer.GetInputs("DAY - NAME");
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            //Handle Inputs

            //Implement Puzzle
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
    }
}
