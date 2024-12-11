using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class DiskFragmenter
    {
        static int currentLength;
        static bool isSecond;
        public static long GetInput()
        {
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Disk Fragmenter");
            Queue<String> fileQueue = InputGatherer.GetInputs("9 - DiskFragmenter");
            string input = fileQueue.Dequeue();
            List<int> inputLengths = new();
            List<int> emptyLengths = new();
            List<int> order = new();
            order.AddRange(Enumerable.Range(0, input.Length / 2 + 1));
            int firstID = 0, lastID = input.Length / 2;
            currentLength = -1;
            for(int i = 0; i <= input.Length / 2; i++)
            {
                inputLengths.Add(int.Parse(input[i * 2].ToString()));
                if(i * 2 + 1 < input.Length)
                    emptyLengths.Add(int.Parse(input[i * 2 + 1].ToString()));
            }
            if (isSecond)
                return CalculateSecondChecksum(inputLengths, emptyLengths, order);
            else
                while (input.Length > 0)
                {
                    result += GetFrontChecksum(firstID, input, out int length);
                    firstID++;
                    if(input.Length > 2)
                    {
                        input = input[2..];
                        result += GetBackChecksum(lastID, length, input, out input, out lastID);
                    }
                    else
                        input = string.Empty;
                }
            return result;
        }
        static long GetFrontChecksum(int id, string input, out int length)
        {
            long result = 0;
            length = int.Parse(input[0].ToString());
            int strength = length + currentLength;
            while(strength > currentLength)
            {
                result += strength * id;
                strength--;
            }
            currentLength += length;
            if (input.Length > 1)
                length = int.Parse(input[1].ToString());
            else
                length = -1;
            return result;
        }
        static long GetBackChecksum(int id, int length, string input, out string output, out int newId)
        {
            long result = 0;
            int backLength;
            int strength;
            newId = id;
            output = string.Empty;
            while(length > (backLength = int.Parse(input[input.Length - 1].ToString())))
            {
                strength = backLength + currentLength;
                while (strength > currentLength)
                {
                    result += strength * newId;
                    strength--;
                }
                if (input.Length > 2)
                {
                    input = input[..(input.Length - 2)];
                    newId--;
                    length -= backLength;
                    currentLength += backLength;
                }
                else
                    return result;
            }
            strength = length + currentLength;
            while (strength > currentLength)
            {
                result += strength * newId;
                strength--;
            }
            currentLength += length;

            output = input[..(input.Length - 1)] + (int.Parse(input[input.Length - 1].ToString()) - length).ToString();
            return result;
        }
        static long CalculateSecondChecksum(List<int> input, List<int> empty, List<int> order)
        {
            List<int> usedNumbers = new();
            long total = 0;
            for(int i = input.Count - 1; i >= 0; i--)
            {
                if (usedNumbers.Contains(order[i]))
                    continue;
                usedNumbers.Add(order[i]);
                for(int j = 0; j < empty.Count; j++)
                {
                    if (input[i] <= empty[j] && j < i)
                    {
                        empty[j] -= input[i];
                        empty.Insert(j, 0);
                        empty[i] += input[i];
                        if (i < input.Count - 1)
                        {
                            empty[i] += empty[i + 1];
                            empty.RemoveAt(i + 1);
                        }
                        order.Insert(j + 1, order[i]);
                        order.RemoveAt(i + 1);
                        input.Insert(j + 1, input[i]);
                        input.RemoveAt(i + 1);
                        i++;
                        break;
                    }
                }
            }
            int count = 0;
            for(int i = 0; i < input.Count; i++)
            {
                for(int j = 0; j < input[i]; j++)
                {
                    total += order[i] * count;
                    Console.Write(order[i]);
                    count++;
                }
                if(i < empty.Count())
                {
                    count += empty[i];
                    for (int j = 0; j < empty[i]; j++)
                        Console.Write('.');
                }
            }
            Console.WriteLine();
            return total;
        }

    }
}
