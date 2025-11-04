using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class CeresSearch
    {
        static int length, width;
        static char[,] map;
        public static int GetInput()
        {
            bool isSecond = InputGatherer.GetUserInput("Ceres Search");
            Queue<string> fileQueue = InputGatherer.GetInputs("4 - CeresSearch");
            length = fileQueue.Count;
            width = fileQueue.Peek().Length - 1;
            map = new char[width, length];
            int count = 0;
            string input;
            while (fileQueue.Count > 0)
            {
                input = fileQueue.Dequeue();
                for (int x = 0; x < width; x++)
                    map[x, length - fileQueue.Count - 1] = input[x];
            }
            for (int x = 0; x < width; x++)
                for (int y = 0; y < length; y++)
                {
                    if (isSecond)
                            count += map[x, y] == 'A' && IsRealXMas(x, y) ? 1 : 0;
                    else if (map[x, y] == 'X')
                        count += XmasCount(x, y);
                }
            return count;
        }

        static bool IsRealXMas(int x, int y)
        {
            string legalChars = "MS";
            if (x < 1 || x >= width - 1 || y < 1 || y >= length - 1)
                return false;
            if (map[x - 1, y - 1] == map[x - 1, y + 1] &&
                legalChars.Contains(map[x - 1, y - 1]))
            {
                return map[x + 1, y + 1] == map[x + 1, y - 1] &&
                legalChars.Contains(map[x + 1, y + 1]) &&
                map[x+1, y+1] != map[x-1,y-1];
            }
            else if (map[x - 1, y - 1] == map[x + 1, y - 1] &&
                legalChars.Contains(map[x - 1, y - 1]))
            {
                return map[x + 1, y + 1] == map[x - 1, y + 1] &&
                legalChars.Contains(map[x + 1, y + 1]) &&
                map[x + 1, y + 1] != map[x - 1, y - 1];
            }

            return false;
        }

        static int XmasCount(int currentX, int currentY)
        {
            int result = 0;
            for(int x = -1 ;x <= 1 ; x++)
                for (int y = -1; y <= 1; y++)
                    if(IsXmas(currentX, currentY, x, y))
                        result++;
            return result;
        }
        static bool IsXmas(int currentX, int currentY, int xDir, int yDir)
        {
            string xmas = "XMAS";
            string potential = "";
            potential += map[currentX, currentY];
            while (xmas.Contains(potential))
            {
                currentX += xDir;
                currentY += yDir;
                try
                {
                    potential += map[currentX, currentY];
                }
                catch (Exception e)
                {
                    return false;
                }
                if (potential == xmas)
                    return true;
            }
            return false;

        }
    }
}
