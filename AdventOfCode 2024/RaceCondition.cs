using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class RaceCondition
    {
        static bool isSecond;
        static HashSet<Point> visited = new();
        static Dictionary<Point, int> values = new();
        static Dictionary<int, int> values2 = new();
        static int length, width;
        static Point startPos, endPos;
        static int allowedSteps, minScore;
        public static int GetInput()
        {
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Race Condition");
            Queue<String> fileQueue = InputGatherer.GetInputs("20 - RaceCondition");
            Stopwatch watch = new Stopwatch();
            length = fileQueue.Count;
            width = fileQueue.Peek().Length;
            char[,] map = new char[length, width];
            Console.Write("How many steps are allowed?   ");
            while(allowedSteps == 0)
                int.TryParse(Console.ReadLine(), out allowedSteps);
            Console.Write("How many steps would you like to skip?   ");
            while (minScore == 0)
                int.TryParse(Console.ReadLine(), out minScore);

            watch.Restart();
            for (int y = 0; y < length; y++)
            {
                string input = fileQueue.Dequeue();
                for (int x = 0; x < width; x++)
                {
                    if (y == 0 || x == 0 || y == length - 1 || x == width - 1)
                        map[x, y] = 'B';
                    else
                        map[x, y] = input[x];
                    if (input[x] == 'S')
                    {
                        startPos = new Point(x, y);
                        map[x, y] = '.';
                    }
                    if (input[x] == 'E')
                    {
                        endPos = new Point(x, y);
                        map[x, y] = '.';
                    }

                }
            }

            map = FindDirections(map);

            for(int y = 0; y < length; y++)
            {
                for(int x = 0;x < width; x++)
                    Console.Write(map[x, y]);
                Console.WriteLine();
            }

            foreach (Point p in values.Keys)
            {
                visited.Clear();
                result += BigCheat(map, p);
            }

            foreach (int i  in values2.Keys)
            {
                Console.WriteLine($"There are {values2[i]} cheats that save {i} picoseconds");
            }

            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static char[,] FindDirections(char[,] map)
        {
            Point currentPos = endPos;
            int count = 0;
            while (currentPos != startPos)
            {
                values[new Point(currentPos.X, currentPos.Y)] = count;
                for (int i = 0; i < 4; i++)
                {
                    Point direction = NiceToHave.directions[i];
                    Point newPosition = new Point(currentPos.X + direction.X, currentPos.Y + direction.Y);
                    if (map[newPosition.X, newPosition.Y] == '.' && !values.ContainsKey(newPosition))
                    {
                        currentPos = newPosition;
                        count++;
                        break;
                    }
                }
            }
            values[new Point(startPos.X, startPos.Y)] = count;
            return map;
        }
        static int BigCheat(char[,] map, Point origin)
        {
            int result = 0;
            Queue<(Point, int, bool)> queue = new();
            queue.Enqueue((origin, 0, false));
            while (queue.TryDequeue(out var data))
            {
                char current = map[data.Item1.X, data.Item1.Y];
                if (current == 'B' || visited.Contains(data.Item1))
                    continue;
                visited.Add(data.Item1);
                if (current == '.' && data.Item2 > 0&&
                    values[origin] > values[data.Item1] && data.Item3)
                {
                    int diff = (values[startPos] - values[origin] + (values[data.Item1] + data.Item2));
                    diff = values[startPos] - diff;
                    if (diff >= minScore)
                    {
                        result++;
                        if (!values2.ContainsKey(diff))
                            values2[diff] = 1;
                        else values2[diff]++;
                    }

                }
                if (current == '#')
                    data.Item3 = true;
                if (data.Item2 >= allowedSteps)
                    continue;
                Array.ForEach(NiceToHave.directions, d => queue.Enqueue((new Point(data.Item1.X + d.X, data.Item1.Y + d.Y), data.Item2 + 1, data.Item3)));
            }
            return result;
        }
    }
}
