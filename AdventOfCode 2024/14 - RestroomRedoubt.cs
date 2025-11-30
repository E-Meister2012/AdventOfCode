using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2024
{
    internal class RestroomRedoubt
    {
        static int width, length, seconds;
        static int Q1 = 0, Q2 = 0, Q3 = 0, Q4 = 0;
        static bool isSecond;
        static char[,] map;
        public static int GetInput()
        {
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Restroom Redoubt");
            Queue<string> fileQueue = InputGatherer.GetInputs("14 - RestroomRedoubt");
            Stopwatch watch = new Stopwatch();
            List<Point> currentPositions = new();
            List<Point> velocities = new();
            watch.Restart();
            width = 101;
            length = 103;
            seconds = 100;
            while (fileQueue.Count > 0)
            {
                string input = fileQueue.Dequeue();
                string[] positions = input.Split(' ');
                string startingPosition = positions[0].Split('=')[1];
                string direction = positions[1].Split('=')[1];
                Point currentPosition = new Point(int.Parse(startingPosition.Split(',')[0]),
                    int.Parse(startingPosition.Split(',')[1]));
                Point velocity = new Point(int.Parse(direction.Split(',')[0]),
                    int.Parse(direction.Split(',')[1]));
                GetQuadrant(currentPosition, velocity);
                currentPositions.Add(currentPosition);
                velocities.Add(velocity);
            }
            if (isSecond)
            {
                seconds = 0;
                double standardDevX = double.MaxValue; 
                double standardDevY = double.MaxValue;
                map = new char[width, length];
                while(true)
                {
                    currentPositions = UpdatePositions(currentPositions, velocities);
                    seconds++;
                    int[] xPos = currentPositions.Select(position => position.X).ToArray();
                    int[] yPos = currentPositions.Select(position => position.Y).ToArray();
                    standardDevX = StandardDev(xPos);
                    standardDevY = StandardDev(yPos);
                    if (standardDevX < 25f && standardDevY < 25f)
                    {
                        for(int y = 0; y < length; y++)
                        {
                            for(int x = 0; x < width; x++)
                            {
                                if (currentPositions.Contains(new Point(x, y)))
                                    Console.Write('#');
                                else
                                    Console.Write('.');
                            }
                            Console.WriteLine();
                        }
                        break;
                    }
                }
            }
            result = isSecond ? seconds : Q1 * Q2 * Q3 * Q4;
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static void GetQuadrant(Point startingPos,  Point directionPos)
        {
            Point endPos = new Point((startingPos.X + directionPos.X * seconds) % width,
                (startingPos.Y + directionPos.Y * seconds) % length);
            if (endPos.X < 0)
                endPos.X += width;
            if (endPos.Y < 0)
                endPos.Y += length;
            Console.WriteLine(endPos.ToString());
            if (endPos.X < width / 2) //Is in Q1 or Q3
            {
                if (endPos.Y < length / 2)
                    Q1++;
                else if (endPos.Y >= length - length / 2)
                    Q3++;
            }
            else if (endPos.X >= width - width / 2) //Is in Q2 or Q4
            {
                if (endPos.Y < length / 2)
                    Q2++;
                else if (endPos.Y >= length - length / 2)
                    Q4++;
            }
        }
        static List<Point> UpdatePositions(List<Point> currentPositions, List<Point> velocities)
        {
            for(int i = 0; i < currentPositions.Count; i++)
            {
                currentPositions[i] = new Point((currentPositions[i].X + velocities[i].X) % width,
                    (currentPositions[i].Y + velocities[i].Y) % length);
                if (currentPositions[i].X < 0)
                    currentPositions[i] = new Point(currentPositions[i].X + width, currentPositions[i].Y );
                if (currentPositions[i].Y < 0)
                    currentPositions[i] = new Point(currentPositions[i].X, currentPositions[i].Y + length);
                map[currentPositions[i].X, currentPositions[i].Y] = '#';
            }
            return currentPositions;
        }
        static double StandardDev(int[] positions)
        {
            int sum = positions.Sum();
            int count = positions.Length;
            double mean = sum / count;
            double variance = positions
            .Select(value => Math.Pow(mean - value, 2))
            .Sum() / count;
            return Math.Sqrt(variance);
        }
    }
}
