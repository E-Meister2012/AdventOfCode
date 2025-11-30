using AdventOfCodeBase;
using System.Drawing;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode_2024
{
    internal class GuardGallivant
    {
        static HashSet<Point> visited = new();
        static char[,] map;
        static Point currentPos, startingPos;
        static Point walkingDir;
        static int width;
        static int length;
        static bool isSecond;
        static int obstructions = 0;

        public static int GetInput()
        {
            isSecond = InputGatherer.GetUserInput("Guard Gallivant");
            Queue<string> fileQueue = InputGatherer.GetInputs("06 - GuardGallivant");
            string input;
            length = fileQueue.Count;
            width = fileQueue.Peek().Length - 1;
            map = new char[width, length];

            for (int y = 0; y < length; y++)
            {
                input = fileQueue.Dequeue();
                for (int x = 0; x < width; x++)
                {
                    if (input[x] == '^')
                    {
                        startingPos = new Point(x, y);
                        currentPos = startingPos;
                        walkingDir = new Point(0, -1);
                    }
                    map[x, y] = input[x];
                }
            }

            //Get Guard pathing
            while (NextGuardMove())
                continue;

            //Return the correct value based on the part 
            return isSecond ? obstructions : visited.Count;
        }

        static bool NextGuardMove()
        {
            if(!visited.Contains(currentPos))
            {
                visited.Add(currentPos);
                if (currentPos != startingPos && Loops( currentPos, walkingDir))
                    obstructions++;
            }
            Point nextLocation = new Point(currentPos.X + walkingDir.X, currentPos.Y + walkingDir.Y);

            if (!InputGatherer.IsInBounds(nextLocation.X, nextLocation.Y, length, width))
                return false;

            if(map[nextLocation.X, nextLocation.Y] == '#')
            {
                walkingDir = new Point(-walkingDir.Y, walkingDir.X);
                nextLocation = currentPos;
            }

            if (!InputGatherer.IsInBounds(nextLocation.X, nextLocation.Y, length, width))
                return false;

            currentPos = nextLocation;

            return true;
        }
        
        static bool Loops(Point startingPos, Point lookingDir)
        {
            HashSet<(Point point, Point direction)> obstructionVisited = new();

            Point currentPosition = new Point(startingPos.X - lookingDir.X, startingPos.Y - lookingDir.Y);
            Point lookingDirection = new Point(-lookingDir.Y, lookingDir.X);

            while(!obstructionVisited.Contains((currentPosition, lookingDirection)))
            {
                obstructionVisited.Add((currentPosition, lookingDirection));

                Point nextPosition = new Point(currentPosition.X + lookingDirection.X, 
                    currentPosition.Y + lookingDirection.Y);
                if (!InputGatherer.IsInBounds(nextPosition.X, nextPosition.Y, length, width))
                    return false;

                if (map[nextPosition.X, nextPosition.Y] == '#'||
                    (nextPosition.X == startingPos.X && nextPosition.Y == startingPos.Y))
                {
                    lookingDirection = new Point(-lookingDirection.Y, lookingDirection.X);
                    nextPosition = currentPosition;
                }
                currentPosition = nextPosition;
            }
            return true;
        }
    }
}