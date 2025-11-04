using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode_2024
{
    internal class ReindeerMaze
    {
        static char[,] map;
        static int length, width;
        static bool isSecond;
        static Point exitPosition;

        public static int GetInput()
        {
            List<Point> visited = new();
            Point startingPosition = new Point();
            int result = 0;
            isSecond = InputGatherer.GetUserInput("ReindeerMaze");
            Queue<string> fileQueue = InputGatherer.GetInputs("16 - ReindeerMaze");
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            length = fileQueue.Count;
            width = fileQueue.Peek().Length;
            map = new char[width, length];
            for (int y = 0; y < length; y++)
            {
                string input = fileQueue.Dequeue();
                for (int x = 0; x < input.Length; x++)
                {
                    map[x, y] = input[x];
                    if (input[x] == 'S')
                        startingPosition = new Point(x, y);
                    if (input[x] == 'E')
                        exitPosition = new Point(x, y);
                }
            }

            result = AStar(startingPosition);
            watch.Stop();

            // Output the map (optional)
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }

            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        // A* pathfinding algorithm
        static int AStar(Point start)
        {
            // Priority queue for A* (stores (f, g, x, y))
            var openSet = new SortedSet<PriorityQueueNode>(Comparer<PriorityQueueNode>.Create(
                (a, b) => a.F == b.F ? a.G.CompareTo(b.G) : a.F.CompareTo(b.F)));
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, int>();  // Cost from start to each point
            var fScore = new Dictionary<Point, int>();  // Estimated total cost

            openSet.Add(new PriorityQueueNode(0, 0, start));  // f = g + h, where g = 0 initially
            gScore[start] = 0;
            fScore[start] = Heuristic(start, exitPosition);

            while (openSet.Count > 0)
            {
                // Get the point with the lowest fScore
                var current = openSet.Min;
                openSet.Remove(current);
                Point currentPos = current.Position;

                if (currentPos == exitPosition)
                {
                    return gScore[currentPos];  // Return the cost to reach the exit
                }

                foreach (var neighbor in GetNeighbors(currentPos))
                {
                    if (map[neighbor.X, neighbor.Y] == '#') continue;  // Ignore walls

                    int tentativeGScore = gScore.GetValueOrDefault(currentPos, int.MaxValue) + 1;

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = currentPos;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Heuristic(neighbor, exitPosition);

                        openSet.Add(new PriorityQueueNode(fScore[neighbor], tentativeGScore, neighbor));
                    }
                }
            }

            return int.MaxValue;  // If no path exists
        }

        static int Heuristic(Point a, Point b)
        {
            // Manhattan distance as heuristic
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        static List<Point> GetNeighbors(Point pos)
        {
            var directions = new List<Point>
            {
                new Point(0, 1),   // Down
                new Point(1, 0),   // Right
                new Point(0, -1),  // Up
                new Point(-1, 0)   // Left
            };

            var neighbors = new List<Point>();
            foreach (var dir in directions)
            {
                var newPos = new Point(pos.X + dir.X, pos.Y + dir.Y);
                // Check if the new position is within bounds and walkable
                if (newPos.X >= 0 && newPos.X < width && newPos.Y >= 0 && newPos.Y < length)
                {
                    neighbors.Add(newPos);
                }
            }

            return neighbors;
        }

        // Custom class to store the f, g values and Position
        public class PriorityQueueNode
        {
            public int F { get; }
            public int G { get; }
            public Point Position { get; }

            public PriorityQueueNode(int f, int g, Point position)
            {
                F = f;
                G = g;
                Position = position;
            }
        }
    }
}
