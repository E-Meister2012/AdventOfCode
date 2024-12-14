﻿using AdventOfCodeBase;
using System.Drawing;

namespace AdventOfCode_2024
{
    internal class GardenGroups
    {
        static List<KeyValuePair<int, int>> visitedPlots = new List<KeyValuePair<int, int>>();
        static char[,] map;
        static int length, width;
        static bool isSecond;
        public static int GetInput()
        {
            string input;
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Garden Groups");
            Queue<String> fileQueue = InputGatherer.GetInputs("12 - GardenGroups");
            length = fileQueue.Count;
            width = fileQueue.Peek().Length - 1;
            map = new char[width, length];
            for (int y = 0; y < length; y++)
            {
                input = fileQueue.Dequeue();
                for (int x = 0; x < width; x++)
                    map[x, y] = input[x];
            }
            for (int y = 0; y < length; y++)
                for (int x = 0; x < width; x++)
                {
                    if (!visitedPlots.Contains(new KeyValuePair<int, int>(x, y)))
                    {
                        int perimiter = GetPerimiter(x, y, map[x, y], out int amount, out int sides);
                        Console.WriteLine($"Letter: {map[x, y]}    Perimiter: {perimiter}   Amount: {amount}    Sides: {sides}");
                        result += isSecond ? sides * amount : perimiter * amount;
                    }
                }
            return result;
        }
        internal static int GetPerimiter(int x, int y, char c, out int amount, out int sideCount)
        {
            List<(Point, int, int)> sides = new();
            sideCount = 0;
            amount = 0;
            List<Point> directions = new() { new Point(0, 1), new Point(1, 0), new Point(0, -1), new Point(-1, 0) };
            int perimiter = 0;
            Queue<(int, int)> queue = new();
            Point direction = new Point(0, 1);
            queue.Enqueue((x, y));
            visitedPlots.Add(new KeyValuePair<int, int>(x, y));
            CheckCorners(x, y, c);
            while (queue.Count > 0)
            {
                (x, y) = queue.Dequeue();
                foreach (Point dir in directions)
                {
                    int newX = x + dir.X;
                    int newY = y + dir.Y;
                    if (!InputGatherer.IsInBounds(newX, newY, length, width) || map[newX, newY] != c)
                        perimiter++;
                    else if (!visitedPlots.Contains(new KeyValuePair<int, int>(newX, newY)))
                    {
                        visitedPlots.Add(new KeyValuePair<int, int>(newX, newY));
                        queue.Enqueue((newX, newY));
                        CheckCorners(newX, newY, c);
                    }
                    direction = new Point(-direction.Y, direction.X);
                }
                amount++;
            }
            sideCount = sides.Count();
            return perimiter;
            void CheckCorners(int x, int y, char c)
            {
                bool containsCurrentPoint;
                foreach (Point dir in directions)
                {
                    containsCurrentPoint = false;
                    if (InputGatherer.IsInBounds(x + dir.X, y + dir.Y, length, width) && map[x + dir.X, y + dir.Y] == c)
                        continue;
                    if (dir.X != 0)
                    {
                        for(int s = 0; s < sides.Count; s++)
                        {
                            if (sides[s].Item1 == dir)
                            {
                                if (sides[s].Item2 == x + dir.X)
                                {
                                    if (Math.Abs(sides[s].Item3 - y) <= 1)
                                    {
                                        containsCurrentPoint = true;
                                        sides[s] = (dir, x + dir.X, y );
                                    }
                                }
                            }
                        }
                        if (!containsCurrentPoint)
                            sides.Add((dir, x + dir.X, y));
                    }
                    if (dir.Y != 0)
                    {
                        for (int s = 0; s < sides.Count; s++)
                        {
                            if (sides[s].Item1 == dir)
                            {
                                if (sides[s].Item2 == y + dir.Y)
                                {
                                    if (Math.Abs(sides[s].Item3 - x) <= 1)
                                    {
                                        containsCurrentPoint = true;
                                        sides[s] = (dir, y + dir.Y, x);
                                    }
                                }
                            }
                        }
                        if (!containsCurrentPoint)
                            sides.Add((dir, y + dir.Y, x));
                    }
                }
            }
        }
    }
}