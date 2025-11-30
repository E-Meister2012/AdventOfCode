using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class HoofIt
    {
        static List<KeyValuePair<int, int>> visitedNines = new();
        static int[,] map;
        static int length, width;
        static bool isSecond;
        public static int GetInput()
        {
            string input;
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Hoof It");
            Queue<string> fileQueue = InputGatherer.GetInputs("10 - HoofIt");
            length = fileQueue.Count;
            width = fileQueue.Peek().Length;
            map = new int[width, length];
            for(int y = 0; y < length; y++)
            {
                input = fileQueue.Dequeue();
                for (int x = 0; x < width; x++)
                    map[x, y] = int.Parse(input[x].ToString());
            }
            for (int y = 0; y < length; y++)
                for (int x = 0; x < width; x++)
                {
                    visitedNines.Clear();
                    result += GetPathTotals(x, y);
                }
            return result;
        }
        static int GetPathTotals(int x, int y, int start = 0)
        {
            int total = 0;
            if (!InputGatherer.IsInBounds(x, y, length, width))
                return 0;
            if (map[x,y] != start)
                return 0;
            if (start == 9 && !visitedNines.Contains(new KeyValuePair<int, int>(x, y)))
            {
                if(!isSecond)
                    visitedNines.Add(new KeyValuePair<int, int>(x, y));
                return 1;
            }
            else if (start == 9)
                return 0;
            total += GetPathTotals(x - 1, y, start + 1);
            total += GetPathTotals(x, y - 1, start + 1);
            total += GetPathTotals(x + 1, y, start + 1);
            total += GetPathTotals(x, y + 1, start + 1);
            return total;
        }
    }
}