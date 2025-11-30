using System.Diagnostics;
using System.Xml.XPath;
using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class ChronospatialComputer
    {
        static bool isSecond;
        static long registerA, registerB, registerC;
        static string input;
        static long best = long.MaxValue;
        static string expectedResult;
        public static string GetInput()
        {
            string result = "";
            isSecond = InputGatherer.GetUserInput("ChronospatialComputer");
            Queue<String> fileQueue = InputGatherer.GetInputs("17 - ChronospatialComputer");
            int[] program;
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            registerA = long.Parse(fileQueue.Dequeue().Split(':', StringSplitOptions.TrimEntries)[1]);
            registerB = long.Parse(fileQueue.Dequeue().Split(':', StringSplitOptions.TrimEntries)[1]);
            registerC = long.Parse(fileQueue.Dequeue().Split(':', StringSplitOptions.TrimEntries)[1]);
            input = fileQueue.Dequeue().Split(':', StringSplitOptions.TrimEntries)[1];
            program = input.Split(",").Select(int.Parse).ToArray();
            expectedResult = input;
            if (isSecond)
            {
                FindCopy(ref program, 0, expectedResult.Length / 2);
                result = best.ToString();
            }
            else
                result = RunProgram(ref program);

            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static long GetComboOperand(int i)
        {
            switch (i)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                case 4:
                    return registerA;
                case 5:
                    return registerB;
                case 6:
                    return registerC;
            }
            return -1;
        }
        static string RunProgram(ref int[] program, long potentialA = 0)
        {
            string result = "";
            int pointer = 0;
            if(potentialA > 0) 
                registerA = potentialA;
            while (pointer + 1 < program.Length)
            {
                int opCode = program[pointer];
                int operand = program[pointer + 1];
                switch (opCode)
                {
                    case 0:
                        registerA = (long)(registerA / (long)Math.Pow(2, GetComboOperand(operand)));
                        break;
                    case 1:
                        registerB ^= operand;
                        break;
                    case 2:
                        registerB = GetComboOperand(operand) % 8;
                        break;
                    case 3:
                        if (registerA != 0)
                        {
                            pointer = operand;
                            pointer -= 2;
                        }
                        break;
                    case 4:
                        registerB ^= registerC;
                        break;
                    case 5:
                        result += (GetComboOperand(operand) % 8).ToString();
                        break;
                    case 6:
                        registerB = (long)(registerA / (long)Math.Pow(2, GetComboOperand(operand)));
                        break;
                    case 7:
                        registerC = (long)(registerA / (long)Math.Pow(2, GetComboOperand(operand)));
                        break;
                }
                pointer += 2;
            }
            return GetCorrectOutput(result);
        }
        static string GetCorrectOutput(string s)
        {
            return string.Join(",", s.ToCharArray());
        }

        //Part 2 copied from someone else
        static void FindCopy(ref int[] program, long currentA, int index)
        {
            if(index == -1)
            {
                best = Math.Min(best, currentA);
                return;
            }

            int next = expectedResult[index];
            for(int i = 0; i < 8; i++)
            {
                long nextA = currentA * 8 + i;
                string result = RunProgram(ref program, nextA);
                if(expectedResult.EndsWith(result))
                    FindCopy(ref program, nextA, index - 1);
            }
        }
    }
}