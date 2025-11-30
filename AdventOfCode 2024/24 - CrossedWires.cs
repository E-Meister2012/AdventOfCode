using AdventOfCodeBase;
using System.Diagnostics;

namespace AdventOfCode_2024
{
    internal class CrossedWires
    {
        static Dictionary<string, bool> initialWires = new();
        static List<Gate> gates = new();
        static bool isSecond;
        public static string GetInput()
        {
            string result = "";
            isSecond = InputGatherer.GetUserInput("Crossed Wires");
            Queue<string> fileQueue = InputGatherer.GetInputs("24 - CrossedWires");
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            while (fileQueue.Count > 0)
            {
                string input = fileQueue.Dequeue();
                if (input.Contains(':'))
                {
                    string[] split = input.Split(':', StringSplitOptions.TrimEntries);
                    initialWires.Add(split[0], split[1] == "1");
                }
                else
                {
                    string[] split = input.Split(' ', StringSplitOptions.TrimEntries);
                    gates.Add(new Gate(split[0], split[2], split[1], split[4]));
                }
            }

            result = isSecond ? FaultyWires(gates) : ProcessGates().ToString();

            watch.Stop();
            Console.WriteLine($"The program took {watch.ElapsedMilliseconds}ms");
            return result;
        }
        static long ProcessGates()
        {
            long result = 0;
            Dictionary<string, bool> knownWires = new(initialWires);
            HashSet<Gate> processedGates = new();
            while (processedGates.Count < gates.Count)
                foreach (Gate gate in gates)
                {
                    if (processedGates.Contains(gate)) continue;
                    if (!knownWires.ContainsKey(gate.input1) || !knownWires.ContainsKey(gate.input2)) continue;
                    knownWires[gate.output] = gate.GiveOutput(knownWires[gate.input1], knownWires[gate.input2]);
                    processedGates.Add(gate);
                }
            List<string> zWires = knownWires.Keys
                .Where(k => k.StartsWith('z'))
                .OrderByDescending(k => int.Parse(k[1..]))
                .ToList();

            foreach (string wire in zWires)
            {
                result = (result << 1) | (uint)(knownWires[wire] ? 1 : 0);
            }
            return result;
        }
        static string FaultyWires(List<Gate> gates)
        {
            var faultyGates = new HashSet<Gate>();
            var lastZGate = gates.Where(g => g.output.StartsWith('z'))
                .OrderByDescending(g => g.output)
                .First();

            foreach (var gate in gates)
            {
                var isFaulty = false;

                if (gate.output.StartsWith('z') && gate.output != lastZGate.output)
                {
                    isFaulty = gate.operation != "XOR";
                }
                else if (!gate.output.StartsWith('z') && !IsInputWire(gate.input1) && !IsInputWire(gate.input2))
                {
                    isFaulty = gate.operation == "XOR";
                }
                else if (IsInputWire(gate.input1) && IsInputWire(gate.input2) && !AreFirstBit(gate.input1, gate.input2))
                {
                    var output = gate.output;
                    var expectedNextType = gate.operation == "XOR" ? "XOR" : "OR";

                    var feedsIntoExpectedGate = gates.Any(other =>
                        other != gate &&
                        (other.input1  == output || other.input2 == output) &&
                        other.operation == expectedNextType);

                    isFaulty = !feedsIntoExpectedGate;
                }

                if (isFaulty)
                {
                    faultyGates.Add(gate);
                }
            }
            static bool AreFirstBit(string input1, string input2) => input1.EndsWith("00") && input2.EndsWith("00");
            static bool IsInputWire(string wire) => wire.StartsWith('x') || wire.StartsWith('y');
            return string.Join(",", faultyGates.Select(g => g.output).OrderBy(w => w));
        }

    }




    class Gate
    {
        internal string input1;
        internal string input2;
        internal string operation;
        internal string output;
        public Gate(string input1, string input2, string operation, string output)
        {
            this.input1 = input1; this.input2 = input2;
            this.operation = operation;
            this.output = output;
        }
        internal bool GiveOutput(bool input1, bool input2) => operation switch
        {
            "AND" => input1 && input2,
            "OR" => input1 || input2,
            "XOR" => input1 ^ input2,
            _ => throw new Exception("ERROR: Operation is not valid")
        };
    }
}
