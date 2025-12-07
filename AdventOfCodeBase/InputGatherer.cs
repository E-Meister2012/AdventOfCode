
namespace AdventOfCodeBase
{
    public class InputGatherer
    {
        public static Queue<string> GetInputs(string problem)
        {
            Queue<string> result = new Queue<string>();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.Combine(basePath, @"..\..\..");
            string filePath = Path.Combine(projectRoot, $@"Inputs\{problem}.txt");
            StreamReader sr = new(filePath);
            string[] file = sr.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in file)
                result.Enqueue(line);
            return result;
        }

        public static bool GetUserInput(string problem)
        {
            Console.WriteLine($"Welcome to the {problem} problem, do you want to do part 1 or part 2?");
            Console.Write("NOTE: if \"1\" or \"2\" is not entered, it defaults to part 1:  ");
            var userInput = Console.ReadLine();
            return userInput == "2";
        }
        public static bool IsInBounds(int x, int y, int length, int width) => x >= 0 && x < width && y >= 0 && y < length;
    }
}
