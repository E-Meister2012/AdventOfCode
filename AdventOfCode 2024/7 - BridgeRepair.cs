using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class BridgeRepair
    {
        static bool isSecond = false;
        public static long GetInput()
        {
            long result = 0;
            isSecond = InputGatherer.GetUserInput("Bridge Repair");
            Queue<string> fileQueue = InputGatherer.GetInputs("7 - BridgeRepair");
            string input = "";
            Dictionary<long, List<long>> inputs = new Dictionary<long, List<long>>();

            while (fileQueue.Count > 0)
            {
                input = fileQueue.Dequeue();
                long output = long.Parse(input.Split(':')[0]);
                List<long> outputs = input.Split(':')[1].Split(' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(long.Parse).ToList();
                inputs.Add(output, outputs);
            }
            foreach (KeyValuePair<long, List<long>> pair in inputs)
                result += GetResult(pair.Key, pair.Value, pair.Value[0]) ? pair.Key : 0;
            return result;
        }
        static bool GetResult(long result, List<long> inputs, long currentValue, int index = 1 )
        {
            if (index >= inputs.Count || currentValue > result)
                return false;

            long addedValue = currentValue + inputs[index];
            long multipliedValue = currentValue * inputs[index];
            long concatValue = long.Parse(currentValue.ToString() + inputs[index].ToString());

            if(index == inputs.Count - 1)
                if (addedValue == result || multipliedValue == result || concatValue == result)
                    return true;

            bool getMultiplied = GetResult(result, inputs, multipliedValue, index + 1);
            bool getAdded = GetResult(result, inputs, addedValue,  index + 1);
            bool getConcat = isSecond && GetResult(result, inputs, concatValue,  index + 1);

            return getMultiplied || getAdded || getConcat;
        }
    }
}