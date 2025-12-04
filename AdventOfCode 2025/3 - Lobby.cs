using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeBase;

namespace AdventOfCode_2025
{
    internal class Lobby
    {
        static int batteryAmount = -1;

        public static long GetInput()
        {
            List<int> bank = [];
            long result = 0;
            while (batteryAmount < 0)
            {
                Console.Write("How many batteries are being used?  ");
                _ = int.TryParse(Console.ReadLine(), out batteryAmount);
            }
            Queue<string> fileQueue = InputGatherer.GetInputs("3 - Lobby");
            Stopwatch watch = new();
            watch.Restart();
            while (fileQueue.Count > 0)
            {
                bank.Clear();
                string input = fileQueue.Dequeue();
                foreach (char c in input)
                    bank.Add(c - '0');
                result += Solve(bank);
            }
            //Implement Puzzle
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        static long Solve(List<int> bank)
        {

            int openItems = batteryAmount - 1;
            List<int> editedBank;
            double returner = 0;
            int[] trueBatteries = new int[batteryAmount];
            for (int i = 0; i < batteryAmount; i++)
            {
                editedBank = [.. bank];
                editedBank.RemoveRange(bank.Count - openItems, openItems);
                int index = ReturnMaxIndex(editedBank);
                trueBatteries[i] = bank[index];
                bank.RemoveRange(0, index + 1);
                openItems--;
            }
            for (int i = 0; i < trueBatteries.Length; i++)
                returner += trueBatteries[i] * Math.Pow(10, batteryAmount - 1 - i);
            Console.WriteLine(returner);
            Console.WriteLine();
            return (long)returner;
        }
        static int ReturnMaxIndex(List<int> bank)
        {
            int max = 0;
            foreach (int i in bank)
                max = Math.Max(max, i);
            return bank.IndexOf(max);
        }

    }
}
