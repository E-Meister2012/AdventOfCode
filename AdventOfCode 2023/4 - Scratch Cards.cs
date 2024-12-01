using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Xml.Schema;

namespace AdventOfCode_2023
{
    internal class ScratchCards
    {
        static List<int> cardScore = new List<int>();
        static List<int> winningNumbers = new List<int>();
        static List<int> gottenNumbers = new List<int>();


        public static int GetInput()
        {

            Queue<string> fileQueue = InputGatherer.GetInputs("4 - Scratch Cards");
            bool isSecond = InputGatherer.GetUserInput("Scratch Cards");

            string numbers, input;
            int finalScore = 0;
            while (fileQueue.Count > 0)
            {
                input = fileQueue.Dequeue();
                winningNumbers.Clear();

                numbers = input.Split(':')[1];
                winningNumbers = ConvertToInt(numbers.Split('|')[0]);
                gottenNumbers = ConvertToInt(numbers.Split('|')[1]);
                finalScore += isSecond ? GetCardAmount() : GetPoints();
            }
            return finalScore;
        }

        static int GetPoints()
        {
            int winningCards = -1;
            foreach (int number in gottenNumbers)
                if (winningNumbers.Contains(number))
                {
                    winningCards++;
                }
            if(winningCards > -1)
                return (int)Math.Pow(2, winningCards);
            else return 0;
        }

        static List<int> ConvertToInt(string input)
        {
            return input.Trim().Split(' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse).ToList(); ;
        }

        static int GetCardAmount()
        {
            int currentCardCount = 1;
            if (cardScore.Count > 0)
            {
                currentCardCount = cardScore[0];
                cardScore.RemoveAt(0);
            }
            int index = 0;

            foreach (int number in gottenNumbers)
                if (winningNumbers.Contains(number))
                {
                    if (index >= cardScore.Count)
                        cardScore.Add(currentCardCount + 1);
                    else
                        cardScore[index] += currentCardCount;
                    index++;
                }
            return currentCardCount;
        }
    }
}
