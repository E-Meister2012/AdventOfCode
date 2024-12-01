using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCodeBase;

namespace AdventOfCode_2023
{
    public class Trebuchet
    {
        static string? input;
        static List<int> numbers = new List<int>();
        public static int GetInput()
        {
            int tempNumber = 0;
            int ones;
            Queue<string> fileQueue = InputGatherer.GetInputs("1 - Trebuchet");

            bool isSecond = InputGatherer.GetUserInput("Trebuchet");
            
            while (fileQueue.Count > 0)
            {
                input = fileQueue.Dequeue();
                for (int i = 0; i < input.Length; i++)
                    if ((tempNumber = isDigit(input[i].ToString(), isSecond, true, i) * 10)> -1)
                        break;
                for (int i = input.Length - 1; i >= 0; i--)
                    if (( ones = isDigit(input[i].ToString(), isSecond, true, i)) > -1)
                    {
                        numbers.Add(tempNumber + ones);
                        break;
                    }
            }
            return GetOutput();
        }

        static int GetOutput()
        {
            int result = 0;
            foreach (int i in numbers)
                result += i;
            return result;
        }

        static int isDigit(string current, bool isSecond, bool isStart, int location)
        {
            int writtenNumber;
            if (int.TryParse(current, out int digit))
                return digit;
            else if(isSecond && (writtenNumber = IsWrittenNumber(location, isStart)) > -1)
            {
                return writtenNumber;
            }
            return -1;

        }

        static int IsWrittenNumber(int i, bool isStart)
        {
            string tempString = "";
            char[] startingLetters = new char[] { 'z', 'o', 't', 'f', 's', 'e', 'n' };
            char[] endingLetters = new char[] { 'o', 'e', 'r', 'x', 'n', 't' };
            if ((!startingLetters.Contains(input[i]) && isStart) || (!endingLetters.Contains(input[i]) && !isStart))
                return -1;

            while (tempString.Length < 5 && i > -1 && i < input.Length)
            {
                tempString = isStart ? tempString + input[i] : input[i] + tempString;
                if (Enum.TryParse(tempString, out Digits result))
                {
                    return (int)result;
                }
                i += isStart ? 1 : -1;
            }
            return -1;
        }

        enum Digits
        {
            zero = 0,
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9
        }
    }
}
