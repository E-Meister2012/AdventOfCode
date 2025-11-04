using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode_2023
{
    internal class CubeGame
    {
        public static int GetInput()
        {
            bool isSecondPart = InputGatherer.GetUserInput("Cube Game");
            Queue<string> fileQueue = InputGatherer.GetInputs("2 - Cube Game");

            int sum = 0;
            while (fileQueue.Count > 0)
            {
                Game currentGame = new Game(fileQueue.Dequeue());
                sum += isSecondPart ? CalculatePower(currentGame.gameList) : (EvaluateResults(currentGame.gameList) ? currentGame.id : 0);
            }
            return sum;
        }



        static bool EvaluateResults(string[] gameData)
        {
            foreach (string blockData in gameData)
            {
                string[] blocks = blockData.Split(',');
                foreach (string block in blocks)
                {
                    string trimmedBlock = block.TrimStart();
                    string[] parts = trimmedBlock.Split(' ');

                    if (Enum.TryParse(parts[1], out Blocks blockEnum) && (int)blockEnum < int.Parse(parts[0]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static int CalculatePower(string[] gameData)
        {
            int[] powers = new int[3];
            foreach (string blockData in gameData)
            {
                string[] blocks = blockData.Split(',');
                foreach (string block in blocks)
                {
                    string trimmedBlock = block.TrimStart();
                    string[] parts = trimmedBlock.Split(' ');

                    if (Enum.TryParse(parts[1], out PowerBlocks powerBlockEnum))
                    {
                        powers[(int)powerBlockEnum] = Math.Max(powers[(int)powerBlockEnum], int.Parse(parts[0]));
                    }
                }
            }
            return powers[0] * powers[1] * powers[2];
        }

        enum Blocks
        {
            green = 13,
            blue = 14,
            red = 12
        }

        enum PowerBlocks
        {
            green,
            blue,
            red
        }
    }

    class Game
    {
        public int id;
        public string[] gameList;

        public Game(string input)
        {
            string[] games = input.Split(':');
            id = int.Parse(games[0].Split(' ')[1]);
            gameList = games[1].Split(';');
        }
    }
}
