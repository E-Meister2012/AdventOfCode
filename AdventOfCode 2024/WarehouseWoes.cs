using AdventOfCodeBase;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;


namespace AdventOfCode_2024
{
    internal class WarehouseWoes
    {
        static char[,] map, map2;
        static int width, length;
        static bool isSecond;
        static Point currentLocation;
        static Dictionary<char, Point> directionDictionary;
        public static int GetInput()
        {
            int count = 0;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.Combine(basePath, @"..\..\..");
            string filePath = Path.Combine(projectRoot, $@"Inputs\15 - WarehouseWoesOuput.txt");
            StreamWriter writer = new StreamWriter(filePath);
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Warehouse Woes");
            Queue<String> fileQueue = InputGatherer.GetInputs("15 - WarehouseWoes");
            Queue<char> directions = new();
            directionDictionary = new Dictionary<char, Point>()
            {
                { '^', new Point(0,-1) },
                { '>', new Point(1,0 ) },
                { 'v', new Point(0,1 ) },
                { '<', new Point(-1,0) }
            };

            Stopwatch watch = new Stopwatch();
            watch.Restart();
            length = fileQueue.Peek().Length;
            width = length + (isSecond ? length : 0);
            map = new char[width, length];
            for(int y = 0; y < length; y++)
            {
                string input = fileQueue.Dequeue();
                for (int x = 0; x < input.Length; x++)
                {
                    if(isSecond)
                        switch (input[x])
                        {
                            case '.':
                                map[x * 2, y] = '.';
                                map[x * 2 + 1, y] = '.';
                                break;
                            case '#':
                                map[x * 2, y] = '#';
                                map[x * 2 + 1, y] = '#';
                                break;
                            case 'O':
                                map[x * 2, y] = '[';
                                map[x * 2 + 1, y] = ']';
                                break;
                            case '@':
                                map[x * 2, y] = '@';
                                map[x * 2 + 1, y] = '.';
                                break;
                        }

                    else
                        map[x, y] = input[x];
                    if (input[x] =='@')
                        currentLocation = new Point(x + (isSecond ? x : 0), y);

                }
            }
            while(fileQueue.Count > 0)
            {
                string input = fileQueue.Dequeue();
                foreach(char c in input)
                    directions.Enqueue(c);
            }

            Console.WriteLine("Starting Position");
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(map[x, y]);
                    if (map[x, y] == 'O')
                        result += x + 100 * y;
                }
                Console.WriteLine();
            }
            foreach (char c in directions)
            {
                if ((c == '^' || c == 'v') && map[currentLocation.X + directionDictionary[c].X, currentLocation.Y + directionDictionary[c].Y] != '.'
                    && map[currentLocation.X + directionDictionary[c].X, currentLocation.Y + directionDictionary[c].Y] != '#')
                { }
                Move(c, currentLocation.X, currentLocation.Y, '@');
                Console.WriteLine();
                Console.WriteLine(c);
                for (int y = 0; y < length; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Console.Write(map[x, y]);
                        if (map[x, y] == 'O')
                            result += x + 100 * y;
                    }
                    Console.WriteLine();
                }

            }
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(map[x, y]);
                    if ((map[x, y] == 'O' && !isSecond) || (map[x,y] == '[' && isSecond))
                        result += x + 100 * y;
                }
                Console.WriteLine();
            }

            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static void Move(char dir,  int x, int y, char currentObject)
        {
            bool moveBigBoxes = false;
            Point direction = directionDictionary[dir];
            Point newLocation = new Point(x + direction.X, y + direction.Y);
            switch(map[newLocation.X,newLocation.Y])
            {
                case '.':
                    map[x, y] = '.';
                    map[newLocation.X, newLocation.Y] = currentObject; break;
                case 'O':
                    Move(dir, newLocation.X, newLocation.Y, 'O');
                    if (map[newLocation.X, newLocation.Y] == '.')
                        Move(dir, x, y, currentObject); 
                    else
                        newLocation = new Point(x, y); break;
                case '#':
                    newLocation = new Point(x,y);
                    break;
                case '[':
                    if (CanMoveBigBoxes(dir, newLocation.X, newLocation.Y, '['))
                        MoveBigBoxes(dir, x, y, '@');
                    else
                        newLocation = new Point(x, y);
                    break;
                case ']':
                    if (CanMoveBigBoxes(dir, newLocation.X, newLocation.Y, ']'))
                        MoveBigBoxes(dir, x, y, '@');
                    else
                        newLocation = new Point(x, y);
                    break;
            }
            if (currentObject == '@')
                currentLocation = new Point(newLocation.X, newLocation.Y);
        }

        static void MoveBigBoxes(char dir, int x, int y, char currentObject, bool adjacentChecked = false)
        {
            Point direction = directionDictionary[dir];
            if(direction.Y != 0 && !adjacentChecked)
            {
                if (currentObject == '[')
                    MoveBigBoxes(dir, x + 1, y, ']', true);
                else if (currentObject == ']')
                    MoveBigBoxes(dir, x - 1, y, '[', true);
            }
            Point newPosition = new Point(x + direction.X, y + direction.Y);
            if (map[newPosition.X, newPosition.Y] == '[')
            MoveBigBoxes(dir, newPosition.X, newPosition.Y, '[');
            if (map[newPosition.X, newPosition.Y] == ']')
                MoveBigBoxes(dir, newPosition.X, newPosition.Y, ']');
            map[newPosition.X, newPosition.Y] = currentObject;
                map[x,y] = '.';

        }

        static bool CanMoveBigBoxes(char dir, int x, int y, char currentObject, bool adjacentChecked = false)
        {
            bool result = true;
            Point direction = directionDictionary[dir];
            Point newLocation = new Point(x + direction.X, y + direction.Y);
            if (direction.Y != 0 && !adjacentChecked)
            {
                if (currentObject == '[')
                    result &= CanMoveBigBoxes(dir, x + 1, y, ']', true);
                else if (currentObject == ']')
                    result &= CanMoveBigBoxes(dir, x - 1, y, '[', true);
            }
            if(result)
            switch (map[newLocation.X, newLocation.Y])
            {
                case '.':
                    break;
                case '#':
                    result = false;
                    break;
                case '[':
                        result &= CanMoveBigBoxes(dir, newLocation.X, newLocation.Y, '[');
                    break;
                case ']':
                        result &= CanMoveBigBoxes(dir, newLocation.X, newLocation.Y, '[');
                    break;
            }
            return result;
        }
    }
}
