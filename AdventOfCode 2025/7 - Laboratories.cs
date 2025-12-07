using AdventOfCodeBase;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode_2025
{
    internal class Laboratories
    {
        static bool isSecond;
        public static long GetInput()
        {
            Dictionary<Point, long> lasers = [];
            List<Point> splitters = [];
            List<int> usedRows = [];
            long splittersUsed = 0, result = 0;
            isSecond = InputGatherer.GetUserInput("Laboratories");
            Queue<string> fileQueue = InputGatherer.GetInputs("7 - Laboratories");
            int totalRows = fileQueue.Count;
            Stopwatch watch = new();
            watch.Restart();
            for(int y = 0; fileQueue.Count > 0 ; y++)
            {
                string input = fileQueue.Dequeue();
                for (int x = 0; x < input.Length; x++)
                    if (input[x] == '^')
                    {
                        splitters.Add(new Point(x, y));
                        if (!usedRows.Contains(y))
                            usedRows.Add(y);
                    }
                    else if (input[x] == 'S') lasers.Add(new Point(x, y), 1);
            }
            for(int y = 1; y < totalRows; y++)
            {
                Dictionary<Point, long> toAdd = [];
                foreach (Point p in lasers.Keys.Where(l => l.Y == y - 1))
                {
                    Point down = new(p.X, p.Y + 1);
                    if (splitters.Contains(down))
                    {
                        Point up = p;
                        long i = lasers[p];
                        Point left = new(p.X - 1, p.Y + 1);
                        Point right = new(p.X + 1, p.Y + 1);
                        if (!toAdd.TryAdd(left, i))
                            toAdd[left] += i;
                        if (!toAdd.TryAdd(right, i))
                            toAdd[right] += i;
                        splittersUsed++;
                    }
                    else
                    {
                        if (!toAdd.TryAdd(down, lasers[p]))
                            toAdd[down] += lasers[p];

                    }
                }
                foreach (var (p, i) in toAdd)
                    lasers.Add(p, i);
            }

            foreach (Point p in lasers.Keys.Where(l => l.Y == totalRows - 1))
                result += lasers[p];
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return isSecond ? result: splittersUsed;
        }

    }
}
