using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class ResonantCollinearity
    {
        static char[,] map;
        static int length, width;
        static bool isSecond;
        public static int GetInput()
        {
            isSecond = InputGatherer.GetUserInput("Resonant Collinearity");
            Queue<string> fileQueue = InputGatherer.GetInputs("8 - ResonantCollinearity");
            length = fileQueue.Count;
            width = fileQueue.Peek().Length - 1;
            map = new char[width, length];
            char[,] output;
            char[] usedChars = new char[62]; //Max amount of chars, 2x26 for the letters + 10 for digits
            int charIndex = 0;
            string input;
            while (fileQueue.Count > 0)
            {
                input = fileQueue.Dequeue();
                for (int x = 0; x < width; x++)
                {
                    map[x, length - fileQueue.Count - 1] = input[x];
                    if (char.IsLetterOrDigit(input[x]) && !usedChars.Contains(input[x]))
                    {
                        usedChars[charIndex] = input[x];
                        charIndex++;
                    }
                }
            }
            output = map.Clone() as char[,];
            foreach (char c in usedChars)
                GetOutput(output, c);
            return CountAntinodes(output);
        }
        static List<KeyValuePair<int, int>> GetAntennaLocations(char c)
        {

            List<KeyValuePair<int, int>> output = new();
            if (c == 0)
                return output;
            for (int y = 0; y < length; y++)
                for (int x = 0; x < width; x++)
                    if (map[x, y] == c)
                        output.Add(new KeyValuePair<int, int>(x, y));
            return output;
        }
        static int CountAntinodes(char[,] output)
        {
            int finalCount = 0;
            foreach (char c in output)
                if (c == '#' || (isSecond && char.IsLetterOrDigit(c)))
                    finalCount++;
            return finalCount;
        }
        static char[,] GetOutput(char[,] output, char c)
        {
            int antinodeX, antinodeY;
            List<KeyValuePair<int, int>> antennaLocations = GetAntennaLocations(c);
            for (int i = 0; i < antennaLocations.Count; i++)
                for (int j = i + 1; j < antennaLocations.Count; j++)
                {
                    int distanceX = antennaLocations[j].Key - antennaLocations[i].Key;
                    int distanceY = antennaLocations[j].Value - antennaLocations[i].Value;
                    GetAntinode(antennaLocations[i].Key, antennaLocations[i].Value, -distanceX, -distanceY,
                        out antinodeX, out antinodeY);
                    while (InputGatherer.IsInBounds(antinodeX, antinodeY, length, width))
                    {
                        output[antinodeX, antinodeY] = '#';
                        if (!isSecond)
                            break;
                        GetAntinode(antinodeX, antinodeY, -distanceX, -distanceY,
                        out antinodeX, out antinodeY);
                    }
                    GetAntinode(antennaLocations[j].Key, antennaLocations[j].Value, distanceX, distanceY,
                        out antinodeX, out antinodeY);
                    while (InputGatherer.IsInBounds(antinodeX, antinodeY, length, width))
                    {
                        output[antinodeX, antinodeY] = '#';
                        if (!isSecond)
                            break;
                        GetAntinode(antinodeX, antinodeY, distanceX, distanceY,
                        out antinodeX, out antinodeY);
                    }
                }
            return output;
        }
        static void GetAntinode(int startX, int startY, int diffX, int diffY, out int antinodeX, out int antinodeY)
        {
            antinodeX = startX;
            antinodeY = startY;
            antinodeX += diffX;
            antinodeY += diffY;
        }
    }
}
