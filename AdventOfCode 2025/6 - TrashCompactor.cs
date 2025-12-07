using AdventOfCodeBase;
using System.Buffers;
using System.Diagnostics;
using System.Xml.XPath;

namespace AdventOfCode_2025
{
    internal class TrashCompactor
    {
        static bool isSecond;
        static List<List<int>> numbers = [];
        static List<char> operation = [];


        public static long GetInput()
        {
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Trash Compactor");
            Queue<string> fileQueue = InputGatherer.GetInputs("6 - TrashCompactor");
            Stopwatch watch = new();
            watch.Restart();
            while (fileQueue.Count > 0 && !isSecond)
            {
                string[] input = fileQueue.Dequeue().Split(' ', StringSplitOptions.TrimEntries |
                    StringSplitOptions.RemoveEmptyEntries);
                if (char.IsDigit(input[0][0]))
                {
                    numbers.Add([]);
                    foreach (string s in input)
                        numbers[^1].Add(int.Parse(s));
                }

                else
                    foreach (string s in input)
                        operation.Add(s[0]);

            }
            if (isSecond)
                ParseInputs(fileQueue);

            result += Solve();
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static long Solve()
        {
            long result = 0;
            long tempResult = 0;
            for(int i = 0; i < operation.Count; i++)
            {
                tempResult = 0;
                List<int> currentNums = [];
                foreach (List<int> list in numbers)
                    currentNums.Add(list[i]);
                if (operation[i] == '*')
                {
                    tempResult = 1;
                    foreach(int s in currentNums)
                        tempResult *= s;
                }
                else if(operation[i] == '+')
                    foreach (int s in currentNums)
                        tempResult += s;
                result += tempResult;
            }
            return result;
        }

        static void ParseInputs(Queue<string> fileQueue)
        {
            string[] inputs = [.. fileQueue];
            List<string> numberedInputs = [];
            string[] tempInput = inputs[^1].Split(' ', StringSplitOptions.TrimEntries |
                    StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in tempInput)
                operation.Add(s[0]);
            while (inputs[0].Length > 0)
            {
                numberedInputs.Clear();
                int nextSpace = FindSpace(inputs);
                for(int i = 0; i < nextSpace; i++)
                {
                    string number = "";
                    for(int j = 0; j < inputs.Length - 1; j++)
                        number += inputs[j][i];
                    number = number.Trim();
                    numberedInputs.Add(number);
                }
                for(int n = 0; n < 4; n++)
                {
                    if (numbers.Count <= n)
                        numbers.Add([]);
                    if (n < numberedInputs.Count)
                        numbers[n].Add(int.Parse(numberedInputs[n]));
                    else
                    {
                        int adder = operation[numbers[n].Count] == '+' ? 0 : 1;
                        numbers[n].Add(adder);
                    }
                }
                Console.WriteLine();
                for (int i = 0; i < inputs.Length; i++)
                    if (inputs[i].Length > nextSpace + 1)
                        inputs[i] = inputs[i][(nextSpace + 1)..];
                    else inputs[i] = "";
            }
        }
        static int FindSpace(string[] input)
        {
            int returner = -1;
            for(int j = 0; j < input.Length - 1; j++)
            {
                bool foundDigit = false;
                for (int i = 0; i < input[j].Length; i++)
                {
                    char c = input[j][i];
                    if (!char.IsDigit(c) && foundDigit)
                    {
                        returner = Math.Max(i, returner);
                        break;
                    }
                    else if(char.IsDigit(c))
                        foundDigit = true;
                    if(i == input[j].Length - 1)
                        return input[j].Length;
                }

            }

            return returner;
        }


    }
}
