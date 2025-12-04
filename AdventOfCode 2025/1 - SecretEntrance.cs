using AdventOfCodeBase;
using System.Diagnostics;


namespace AdventOfCode_2025
{
    internal class SecretEntrance
    {
        static bool isSecond;
        static List<int> inputs = [];
        static int result = 0;
        static int location = 50;

        public static int GetInput()
        {
            isSecond = InputGatherer.GetUserInput("Secret Entrance");
            Queue<string> fileQueue = InputGatherer.GetInputs("1 - SecretEntrance");
            Stopwatch watch = new();
            watch.Restart();
            while (fileQueue.Count > 0)
            {
                string input = fileQueue.Dequeue();
                inputs.Add(input[0] == 'L' ? -int.Parse(input[1..]) : int.Parse(input[1..]));
            }
            Solve();
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        static void Solve()
        {
            //No if statements if boolean is casted to 0/1 * ...
            for (int i = 0; i < inputs.Count; i++)
            {
                int prevLoc = location;
                location += inputs[i] % 100;
                int absInput = Math.Abs(inputs[i]);
                if (location == 0 && !isSecond)
                    result++;
                if (isSecond)
                {
                    if ((location <= 0 || location >= 100) && prevLoc != 0)
                        result++;
                    if (absInput > 99)
                        result += absInput / 100;
                }
                location = (location % 100 + 100) % 100;
            }
        }

    }
}
