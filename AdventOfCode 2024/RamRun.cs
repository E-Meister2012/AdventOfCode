using AdventOfCodeBase;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode_2024
{
    internal class RamRun
    {
        static bool isSecond;
        static bool[,] ram;
        static Point[] directions;
        static int length;
        static Dictionary<Point, int> steps = new();
        public static string GetInput()
        {
            string result = "";
            isSecond = InputGatherer.GetUserInput("RamRun");
            Queue<string> fileQueue = InputGatherer.GetInputs("18 - RamRun");
            Console.Write("What is the length (and width) of the ram?  ");
            length = int.Parse(Console.ReadLine()) + 1;
            ram = new bool[length, length];
            directions = new Point[4] 
            {
                new Point(-1, 0),
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, -1),
            };
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            if(isSecond)
            {
                while(fileQueue.Count > 0)
                {
                    int[] input = fileQueue.Dequeue().Split(',').Select(int.Parse).ToArray();
                    ram[input[0], input[1]] = true; //True = block there
                    if (!FindShortestPath(out int count))
                    {
                        result = $"{input[0]},{input[1]}";
                        break;
                    }
                }
            }
            else
            {
                Console.Write("How many bytes?  ");
                int bytes = int.Parse(Console.ReadLine());
                for (int i = 0; i < bytes; i++)
                {
                    int[] input = fileQueue.Dequeue().Split(',').Select(int.Parse).ToArray();
                    ram[input[0], input[1]] = true; //True = block there
                }
                FindShortestPath(out int steps);
                result = steps.ToString();
            }
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static bool FindShortestPath(out int stepCount)
        {
            steps.Clear();
            Point endPoint = new Point(length - 1, length - 1);
            Point newPosition;
            Queue<Block> queue = new();
            queue.Enqueue(new Block(new Point(0, 0), new Point(0, 0), 0));
            while(queue.Count > 0)
            {
                Block current = queue.Dequeue();
                for(int i = 0; i < directions.Length; i++)
                {
                    newPosition = new Point(current.position.X + directions[i].X, current.position.Y + directions[i].Y);
                    if (directions[i] != current.backDirection && InputGatherer.IsInBounds(newPosition.X, newPosition.Y, length, length) &&
                        !ram[newPosition.X, newPosition.Y] && CheckValue(newPosition, current.steps + 1))
                        queue.Enqueue(new(newPosition, directions[(i + 2) % 4], current.steps + 1));
                }
            }
            if(steps.ContainsKey(endPoint))
            {
                stepCount = steps[endPoint];
                return true;
            }
            stepCount = -1; return false;
        }
        static bool CheckValue(Point position, int newSteps)
        {
            if(steps.ContainsKey(position) && steps[position] <= newSteps)
                return false;
            steps[position] = newSteps;
            return true;

        }
        record struct Block (Point position, Point backDirection, int steps);
    }
}
