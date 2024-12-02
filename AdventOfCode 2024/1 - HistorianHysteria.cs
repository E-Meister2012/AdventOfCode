using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class HistorianHysteria
    {
        public static int GetInput()
        {
            Queue<string> fileQueue = InputGatherer.GetInputs("1 - Historian Hysteria");
            bool isSecond = InputGatherer.GetUserInput("Historian Hysteria");
            List<int> firstList = new List<int>();
            List<int> secondList = new List<int>();
            while (fileQueue.Count > 0)
            {
                int[] input = fileQueue.Dequeue().Split(' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse).ToArray();
                firstList = Insert(firstList, input[0]);
                secondList = Insert(secondList, input[1]);
            }
            return isSecond ? CalcuateSimilarity(firstList, secondList) : CalculateTotalDiff(firstList, secondList);
        }
        static List<int> Insert(List<int> result, int insert)
        {
            for(int i = result.Count - 1; i >= 0; i--)
            {
                if (insert < result[i])
                    continue;
                result.Insert(i + 1, insert);
                return result;
            }
            result.Insert(0, insert);
            return result;
        }

        static int CalculateTotalDiff(List<int> firstList, List<int> secondList)
        {
            int totalDiff = 0;
            for (int i = 0; i < firstList.Count; i++)
                totalDiff += Math.Abs(firstList[i] - secondList[i]);
            return totalDiff;

        }

        static int CalcuateSimilarity(List<int> firstList, List<int> secondList)
        {
            int totalSimm = 0;
            int lastInt = -1;
            int lastResult = -1;
            foreach(int i in firstList)
            {
                if (i == lastInt)
                    totalSimm += lastResult;
                else
                {
                    lastInt = i;
                    lastResult = secondList.FindAll(n => n == i).Count * i;
                    totalSimm += lastResult;
                }
            }
            return totalSimm;
        }

    }
}
