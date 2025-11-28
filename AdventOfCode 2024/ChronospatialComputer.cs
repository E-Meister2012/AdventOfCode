using System.Diagnostics;
using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class ChronospatialComputer
    {
        static bool isSecond;
        static long registerA, registerB, registerC;
        static string input;
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
            if (isSecond)
                result = FindCopy(ref program).ToString();
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
        static string RunProgram(ref int[] program)
        {
            string result = "";
            int pointer = 0;
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
        static long FindCopy(ref int[] program)
        {
            long startA = 1;
            for(startA = 1; startA < (long)Math.Pow(8, program.Length); startA++)
            {
                registerA = startA;
                string output = RunProgram(ref program);
                int[] subProgram = program[(program.Length - output.Length)..];
                bool matchesDigits = subProgram
                    .Select((x, i) => int.Parse(output[i].ToString()) == x).All(x => x);
                if (matchesDigits)
                {
                    if (output.Length == program.Length) break;
                    startA = (startA* 8) - 1;
                }
            }
            return startA;
        }
    }
}
