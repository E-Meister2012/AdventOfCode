using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode_2023
{
    internal class BuildPlan
    {
        static char[,] map;
        static int length, width;
        public static int GetInput()
        {
            int sum = 0;
            bool isSecondPart = InputGatherer.GetUserInput("Build Plan");
            Queue<String> fileQueue = InputGatherer.GetInputs("3 - Build Plan");
            length = fileQueue.Count;
            width = fileQueue.Peek().Length - 1;
            map = new char[width, length];
            string input;
            while(fileQueue.Count > 0)
            {
                input = fileQueue.Dequeue();
                for (int x = 0; x < width; x++)
                    map[x, length - fileQueue.Count - 1] = input[x];
            }
            for (int x = 0;x <width;x++)
                for(int y = 0; y < length; y++)
                {
                    if (char.IsDigit(map[x, y]) || map[x, y] == '.')
                        continue;
                    else
                    {
                        sum += (isSecondPart ? GetGearRatio(x, y) : LookAround(x, y));
                    }
                }
            return sum;
        }
        static int LookAround(int x, int y)
        {
            int sum = 0;
            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i + x < 0 || i + x >= width || j + y < 0 || j + y >= length)
                        continue;
                    if (char.IsDigit(map[i + x, j + y]) && 
                        (i + x == 0 || i == -1 || !char.IsDigit(map[i + x - 1, j + y])))
                        sum += GetNumber(i + x, j + y);
                }
            }
            return sum;
        }
        static int GetNumber(int x, int y)
        {
            string result = "";
            int start = x;
            while (x > -1 && char.IsDigit(map[x, y]))
            {
                result = map[x, y] + result;
                x--;
            }
            x = start + 1;
            while (x < length && char.IsDigit(map[x, y]))
            { 
                result += map[x, y];
                x = x + 1;
            }
            return int.Parse(result);
        }

        static int GetGearRatio(int x, int y)
        {
            List<int> temp = new List<int>();
            if (map[x, y] != '*')
                return 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i + x < 0 || i + x >= width || j + y < 0 || j + y >= length)
                        continue;
                    if (char.IsDigit(map[i + x, j + y]) && 
                        (i + x == 0 || i == -1 || !char.IsDigit(map[i + x - 1, j + y])))
                        temp.Add(GetNumber(i + x, j + y));
                }
            }
            if (temp.Count == 2)
                return temp[0] * temp[1];
            return 0;
        }

    }

}
