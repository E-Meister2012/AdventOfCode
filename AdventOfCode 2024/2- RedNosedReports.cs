using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode_2024
{
    internal class RedNosedReports
    {
        static bool isSecond;
        internal static int GetInput()
        {
            int result = 0;
            Queue<string> fileQueue = InputGatherer.GetInputs("2- RedNosedReports");
            isSecond = InputGatherer.GetUserInput("Red Nosed Reports");
            while(fileQueue.Count > 0)
            {

                string input = fileQueue.Dequeue();
                List<int> levels = input.Split(' ').Select(int.Parse).ToList();
                bool isSafe = CheckSafety(levels, false);
                if (isSafe)
                    result++;
            }
            return result;
        }
        static bool CheckSafety(List<int> levels, bool removedInt)
        {
            List<int> newLevels1, newLevels2;
            bool isIncreasing = levels[0] < levels[1];
            for (int i = 1; i < levels.Count; i++)
            {
                int diff = Math.Abs(levels[i] - levels[i - 1]);
                if (diff > 3 || (isIncreasing && levels[i] <= levels[i - 1]) || (!isIncreasing && levels[i] >= levels[i - 1]))
                {
                    if (!removedInt && isSecond)
                    {
                        newLevels1 = levels.ToList();
                        newLevels2 = levels.ToList();
                        newLevels1.RemoveAt(i - 1);
                        newLevels2.RemoveAt(i);
                        levels.RemoveAt(0);
                        return CheckSafety(newLevels1, true) || CheckSafety(newLevels2, true) || CheckSafety(levels, true);
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
