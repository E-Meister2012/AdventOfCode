using AdventOfCodeBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2024
{
    internal class LANParty
    {
        static bool isSecond;
        static Dictionary<string, List<string>> neighbors = new();
        static List<string> tComputers = new();
        public static string GetInput()
        {
            string result = "";
            isSecond = InputGatherer.GetUserInput("LAN Party");
            Queue<string> fileQueue = InputGatherer.GetInputs("23 - LANParty");
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            while (fileQueue.Count > 0)
            {
                string[] input = fileQueue.Dequeue().Split('-');
                AddInput(input[0], input[1]);
                AddInput(input[1], input[0]);
            }

            result = isSecond ? GetLongestChain() : GetPotentialHistorian().ToString();
            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }

        static int GetPotentialHistorian()
        {
            HashSet<string> groups = new();
            foreach(string s in tComputers)
            {
                foreach(string neighbor1 in neighbors[s])
                    foreach (string neighbor2 in neighbors[s])
                    {
                        if (neighbor1 == neighbor2) continue;
                        if (neighbors[neighbor1].Contains(neighbor2))
                        {
                            List<string> group = new List<string>() { s, neighbor1, neighbor2};
                            group.Sort();
                            string sortedGroup = group[0] + group[1] + group[2];
                            if (!groups.Contains(sortedGroup))
                                groups.Add(sortedGroup);
                        }
                    }
            }
            return groups.Count;
        }

        static void AddInput(string input, string neighbor)
        {
            if(!neighbors.ContainsKey(input))
                neighbors[input] = new List<string>();
            neighbors[input].Add(neighbor);
            if (input[0] == 't')
                tComputers.Add(input);
        }
        static string GetLongestChain()
        {
            List<string> nodes = new List<string>(neighbors.Keys.ToArray<string>());
            nodes.Sort((a, b) => { return neighbors[a].Count.CompareTo(neighbors[b].Count); });
            int maxneighbors = neighbors[nodes[nodes.Count - 1]].Count;
            string password = "";
            Dictionary<string, int> neighborPairs = new();
            List<string> largestConnected;
            foreach (string neighbor1 in nodes)
            {
                foreach (string neighbor in neighbors[neighbor1])
                {
                    if (neighbor[0] < neighbor1[0] || (neighbor[0] == neighbor1[0] && neighbor[1] < neighbor1[1])) continue;
                    largestConnected = new()
                    {
                        neighbor1,
                        neighbor
                    };

                    neighborPairs[neighbor1 + neighbor] = 0;
                    foreach (string neighbor2 in neighbors[neighbor1])
                    {
                        if (neighbors[neighbor].Contains(neighbor2))
                        {
                            neighborPairs[neighbor1 + neighbor]++;
                            largestConnected.Add(neighbor2);
                        }
                    }
                    if (neighborPairs[neighbor1 + neighbor] >= maxneighbors - 2)
                    {
                        bool clique = IsClique();
                        if (clique)
                        {
                            largestConnected.Sort();
                            password = "";
                            password = string.Join(',', largestConnected);
                            break;
                        }
                    }
                }
            }
            if (password.Length > 0)
                return password;
            return "No answer gotten";
            bool IsClique()
            {
                for (int i = 0; i < largestConnected.Count - 1; i++)
                {
                    for (int j = i + 1; j < largestConnected.Count; j++)
                    {
                        string neighbor1 = largestConnected[i];
                        string neighbor2 = largestConnected[j];
                        if (!neighbors[neighbor1].Contains(neighbor2)) return false;
                    }
                }
                return true;
            }
        }
    }
}
