using AdventOfCodeBase;

namespace AdventOfCode_2024
{
    internal class PrintQueue
    {
        static bool isSecond = false;
        public static int GetInput()
        {
            int result = 0;
            isSecond = InputGatherer.GetUserInput("Print Queue");
            Queue<String> fileQueue = InputGatherer.GetInputs("5 - PrintQueue");
            string input = "";
            List<List<int>> updates = new List<List<int>>();
            List<KeyValuePair<int, int>> safetyProtocols = new List<KeyValuePair<int, int>>();
            while((input = fileQueue.Dequeue()) != "\r") 
            {
                int[] inputs = input.Split('|',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse).ToArray();
                safetyProtocols.Add(new KeyValuePair<int, int>(inputs[0], inputs[1]));
            }
            while(fileQueue.Count() > 0)
            {
                input = fileQueue.Dequeue();
                updates.Add(input.Split(',',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse).ToList());
            }
            foreach(List<int> update in updates)
            {
                result += GetValid(safetyProtocols, update);
            }
            return result;
        }

        public static int GetValid(List<KeyValuePair<int, int>> safetyProtocols, List<int> update)
        {
            bool updated = false;
            for (int i = 0; i < update.Count; i++)
            {
                foreach (KeyValuePair<int, int> KvP in safetyProtocols)
                {
                
                    if (KvP.Value == update[i])
                    {
                        int index = update.FindIndex(x => x == KvP.Key);
                        if (update.Contains(KvP.Key) && index > i)
                        {
                            update = Update(update, i, index);
                            updated = true;
                            i = -1;
                            break;
                        }
                    }
                }
            }
            if ((isSecond ^ updated))
                return 0;
            foreach (int p in update)
            {
                Console.Write($"{p}, ");
            }
            Console.WriteLine();
            return update[update.Count() / 2];
        }
        public static List<int> Update(List<int> toUpdate, int i, int j)
        {
            int first = toUpdate[i];
            toUpdate[i] = toUpdate[j];
            toUpdate[j] = first;
            return toUpdate;
        }
    }
}
