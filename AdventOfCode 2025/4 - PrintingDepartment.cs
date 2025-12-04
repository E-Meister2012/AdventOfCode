using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2025
{
    internal class PrintingDepartment
    {
        static char[,] map;
        static bool isSecond;
        static int length, width;
        public static int GetInput()
        {
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Printing Department");
            Queue<string> fileQueue = InputGatherer.GetInputs("4 - PrintingDepartment");
            Stopwatch watch = new();
            length = fileQueue.Count;
            List<(int, int)> locations = [];
            width = length;
            map = new char[width, length];
            watch.Restart();
            for (int y = 0; y < length; y++)
            {
                string input = fileQueue.Dequeue();
                for (int x = 0; x < input.Length; x++)
                    map[x, y] = input[x];
            }
            int tempResult = 1;
            while (tempResult > 0 && isSecond)
            {
                tempResult = 0;
                for (int y = 0; y < width; y++)
                    for (int x = 0; x < length; x++)
                        if (map[x, y] == '@' && SearchAround(x, y) == 1)
                        {
                            tempResult++;
                            locations.Add((x,y));
                        }
                result += tempResult;
                foreach(var (x,y)  in locations)
                    map[x, y] = '.';
                Console.WriteLine(tempResult);
            }
            if(!isSecond)
            {
                for (int y = 0; y < width; y++)
                    for (int x = 0; x < length; x++)
                        if (map[x, y] == '@')
                            result += SearchAround(x, y);

            }
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        static int SearchAround(int baseX, int baseY)
        {
            int around = 0;
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (!InputGatherer.IsInBounds(baseX + x, baseY + y, length, width))
                        continue;
                    if (map[baseX + x, baseY + y] == '@' && !(x == 0 && y == 0))
                        around++;
                }
            return around < 4 ? 1 : 0;
        }
    }
}
