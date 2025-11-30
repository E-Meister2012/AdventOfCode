using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode_2024
{
    internal class ReindeerMaze
    {
        static List<List<Point>> points;
        static char[,] map;
        static int length, width;
        static bool isSecond;
        static Point endPosition;
        static int[,,] hasVisited;
        static Queue<Visitations> visitations;
        static Point[] directions;

        public static int GetInput()
        {
            List<Point> uniquePoints = new();
            directions = new Point[]
            {
                new Point(1,0), //East
                new Point(0,1), //South
                new Point(-1,0), //West
                new Point(0,-1) //North
            };
            Point startingPosition = new();
            int result = int.MaxValue;
            isSecond = InputGatherer.GetUserInput("ReindeerMaze");
            Queue<String> fileQueue = InputGatherer.GetInputs("16 - ReindeerMaze");
            Stopwatch watch = new();
            watch.Restart();
            points = new();
            visitations = new Queue<Visitations>();

            length = fileQueue.Count;
            width = fileQueue.Peek().Length;
            hasVisited = new int[width, length, 4];
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
                        endPosition = new Point(x, y);

                }
            }
            visitations.Enqueue(new Visitations(startingPosition.X, startingPosition.Y, directions[0], 0, 0, new List<Point>()));
            while (visitations.Count > 0)
            {
                int bestRoute = GetBestRoute(visitations.Dequeue(), out Visitations visited);
                if (bestRoute > 0)
                {
                    if(bestRoute < result)
                    {
                        result = bestRoute;
                        points.Clear();
                        points.Add(visited.visited);
                    }
                    else if (bestRoute == result)
                        points.Add(visited.visited);
                }
            }
            watch.Stop();
            foreach(List<Point> list in points)
            {
                foreach (Point p in list)
                    if (!uniquePoints.Contains(p))
                        uniquePoints.Add(p);
                if(!uniquePoints.Contains(endPosition))
                uniquePoints.Add(endPosition);
            }
            return isSecond ? uniquePoints.Count() : result;
        }
        static int GetBestRoute(Visitations visitation, out Visitations visited)
        {
            visited = visitation;
            if (visitation.x == endPosition.X && visitation.y == endPosition.Y)
                return visitation.score;
            visitation.AddPoint();
            Point newPosition = visitation.Move();
            CheckRotations(visitation);
            while (InputGatherer.IsInBounds(newPosition.X, newPosition.Y, length, width) && map[newPosition.X, newPosition.Y] != '#')
            {
                if (hasVisited[newPosition.X, newPosition.Y, visitation.directionIndex] >= visitation.score
                    || hasVisited[newPosition.X, newPosition.Y, visitation.directionIndex] == 0)
                {
                    hasVisited[newPosition.X, newPosition.Y, visitation.directionIndex] = visitation.score;
                }
                visitation.x = newPosition.X; visitation.y = newPosition.Y; visitation.score++;
                CheckRotations(visitation);
                if (visitation.x == endPosition.X && visitation.y == endPosition.Y)
                    return visitation.score;
                newPosition = visitation.Move();
                visitation.AddPoint();
                visited = visitation;
            }
            visited = visitation;
            return -1;
        }
        static void CheckRotations(Visitations visitation)
        {
            int clockwiseRotation = (visitation.directionIndex + 1) % directions.Length;
            if ((hasVisited[visitation.x, visitation.y, clockwiseRotation] >= visitation.score||
                hasVisited[visitation.x, visitation.y, clockwiseRotation] == 0)
                && map[visitation.x + directions[clockwiseRotation].X, visitation.y + directions[clockwiseRotation].Y] != '#')
            {
                hasVisited[visitation.x, visitation.y, clockwiseRotation] = visitation.score;
                visitations.Enqueue(new Visitations(visitation.x, visitation.y, directions[clockwiseRotation], clockwiseRotation, visitation.score + 1000, visitation.visited.ToList()));
            }
            int counterClockwiseRotation = (visitation.directionIndex + (directions.Length - 1)) % directions.Length;
            if ((hasVisited[visitation.x, visitation.y, counterClockwiseRotation] >= visitation.score ||
                hasVisited[visitation.x, visitation.y, counterClockwiseRotation] == 0)
                && map[visitation.x + directions[counterClockwiseRotation].X, visitation.y + directions[counterClockwiseRotation].Y] != '#')
            {
                hasVisited[visitation.x, visitation.y, counterClockwiseRotation] = visitation.score;
                visitations.Enqueue(new Visitations(visitation.x, visitation.y, directions[counterClockwiseRotation], counterClockwiseRotation, visitation.score + 1000, visitation.visited.ToList()));
            }
        }
    }
    class Visitations
    {
        public int x, y, score, directionIndex;
        public Point direction;
        public List<Point> visited;

        public Visitations(int x, int y, Point direction, int directionIndex, int score, List<Point> visited)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
            this.score = score;
            this.directionIndex = directionIndex;
            this.visited = visited;
        }
        public Point Move()
        {
            return new Point(x + direction.X, y + direction.Y);
        }
        public void AddPoint()
        {
            visited.Add(new Point(x, y));
        }
    }
}
