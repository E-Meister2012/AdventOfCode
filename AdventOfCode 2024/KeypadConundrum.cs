using AdventOfCodeBase;
using System.Diagnostics;
using System.Drawing;
using System.IO.IsolatedStorage;

namespace AdventOfCode_2024
{
    internal class KeypadConundrum
    {
        static Dictionary<(char prevPos, char newPos), List<string>> shortestPaths = new();
        static Dictionary<(string code, int level), long> shortestSubPaths = new();
        static char[,] numPad, dirPad;
        static Point[] directions = new Point[4];
        static int robots = 0;
        public static long GetInput()
        {
            directions = new Point[4]
            {
                new Point(-1, 0),
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, -1),
            };
            while(robots == 0)
            {
                Console.Write("How many robots are controlling the direction pads?  ");
                int.TryParse(Console.ReadLine(), out robots);

            }

            long result = 0;
            Queue<string> fileQueue = InputGatherer.GetInputs("21 - KeypadConundrum");
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            numPad = new char[,] { {'7', '8', '9', },
                                         { '4', '5', '6', },
                                         { '1', '2', '3', },
                                         { ' ', '0', 'A' }};
            dirPad = new char[,] { {' ', '^', 'A', },
                                         { '<', 'v', '>' }};

            GetAllSequences(numPad);
            GetAllSequences(dirPad);
            List<string> codes = new();
            while(fileQueue.Count > 0)
                codes.Add(fileQueue.Dequeue());

            foreach(string code in codes)
            {
                int numeric = int.Parse(string.Join("" ,code.Where(char.IsDigit)));
                long length = GetShortestPath(code, 0);
                result += length * numeric;
            }
            return result;
        }

        static long GetShortestPath(string code, int runs)
        {
            if (shortestSubPaths.TryGetValue((code, runs), out long subPath))
                return subPath;

            if (runs == robots + 1)
            {
                shortestSubPaths[(code, runs)] = code.Length;
                return code.Length;
            }
            long result = 0;
            char previous = 'A';
            foreach(char current in code)
            {
                List<string> shortestSubPaths = shortestPaths[(previous, current)];
                long bestSol = long.MaxValue;
                foreach(string s in shortestSubPaths)
                {
                    long currentSol = GetShortestPath(s, runs + 1);
                    if (currentSol < bestSol)
                        bestSol = currentSol;
                }
                previous = current;
                result += bestSol;
            }
            shortestSubPaths[(code, runs)] = result;
            return result;
        }


        static void GetAllSequences(char[,] pad)
        {
            foreach(char c1 in pad)
            {
                foreach(char c2 in pad)
                {
                    shortestPaths[(c1, c2)] = new();
                    if (c1 == c2)
                    {
                        shortestPaths[(c1, c2)].Add("A");
                        continue;
                    }
                    GetSpecificSequence(c1, c2, pad);
                }
            }
        }
        static void GetSpecificSequence(char c1, char c2, char[,] pad)
        {
            Point from = FindCharacter(c1, pad);
            Point to = FindCharacter(c2, pad);
            int length = Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);

            Queue<(Point current, string walked)> queue = new();
            queue.Enqueue((from, ""));
            while(queue.Count > 0)
            {
                (Point current, string sequence) = queue.Dequeue();
                if(current == to)
                {
                    shortestPaths[(c1, c2)].Add(sequence + "A");
                    continue;
                }
                if (sequence.Length >= length)
                    continue;
                foreach(Point dir in directions)
                {
                    Point nextPoint = new Point(current.X + dir.X, current.Y + dir.Y);
                    if(InputGatherer.IsInBounds(nextPoint.X, nextPoint.Y, pad.GetLength(0), pad.GetLength(1))&&
                        pad[nextPoint.Y, nextPoint.X] != ' ')
                    {
                        char direction = ' ';
                        switch((dir.X, dir.Y))
                        {
                            case (0,-1):
                                direction = '^'; break;
                            case (1,0):
                                direction = '>'; break;
                            case (-1,0):
                                direction = '<'; break;
                            case (0,1):
                                direction = 'v'; break;
                        }
                        queue.Enqueue((nextPoint, sequence + direction));

                    }
                }
            }
        }
        static Point FindCharacter(char character, char[,] pad)
        {
            for(int y = 0; y < pad.GetLength(0); y++)
            {
                for(int x = 0;  x < pad.GetLength(1); x++)
                    if (pad[y,x] == character)
                        return new Point(x, y); 
            }
            return new Point();
        }
    }
}
