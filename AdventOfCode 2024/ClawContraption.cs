using AdventOfCodeBase;
using System.Diagnostics;

namespace AdventOfCode_2024
{
    internal class ClawContraption
    {
        static bool isSecond;
        public static long GetInput()
        {
            string input;
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Claw Contraption");
            Queue<string> fileQueue = InputGatherer.GetInputs("13 - ClawContraption");
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            long runs = 0;
            while(fileQueue.Count > 0)
            {
                runs++;
                input = fileQueue.Dequeue();
                string[] buttonsA = input.Split(':', StringSplitOptions.TrimEntries)[1].Split(',', StringSplitOptions.TrimEntries);
                input = fileQueue.Dequeue();
                string[] buttonsB = input.Split(':', StringSplitOptions.TrimEntries)[1].Split(',', StringSplitOptions.TrimEntries);
                input = fileQueue.Dequeue();
                string[] results = input.Split(':', StringSplitOptions.TrimEntries)[1].Split(',', StringSplitOptions.TrimEntries);
                long[,] buttons = { { long.Parse(buttonsA[0].Split('+')[1]), long.Parse(buttonsB[0].Split('+')[1]) }, { long.Parse(buttonsA[1].Split('+')[1]), long.Parse(buttonsB[1].Split('+')[1]) } };
                long[] resultArray = { long.Parse(results[0].Split('=')[1]), long.Parse(results[1].Split('=')[1]) };
                if (isSecond)
                {
                    resultArray[0] += 10000000000000;
                    resultArray[1] += 10000000000000;

                }

                long det = GetDeterminant(buttons);

                if(det == 0)
                {
                    Console.WriteLine("No valid solution found");
                    continue;
                }

                long[,] buttonsX = {
                { resultArray[0], buttons[0, 1] },
                { resultArray[1], buttons[1, 1] }
                };

                long[,] buttonsY = {
                { buttons[0,0], resultArray[0] },
                { buttons[1,0], resultArray[1] }
                };
                
                long detX = GetDeterminant(buttonsX);
                long detY = GetDeterminant(buttonsY);
                long x = detX / det;
                long y = detY / det;

                if (x * buttons[0, 0] + y * buttons[0, 1] != resultArray[0] ||
                    x * buttons[1, 0] + y * buttons[1, 1] != resultArray[1])
                    continue;

                result += 3 * x + y;
            }

            watch.Stop();
            Console.WriteLine($"Runs: {runs}");
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static long GetDeterminant(long[,] input) => (input[0, 0] * input[1, 1]) - (input[0, 1] * input[1, 0]);
    }
}
