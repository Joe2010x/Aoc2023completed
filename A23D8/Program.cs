using Tools;
namespace A23D8;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine(lines.Count);
        var map = new Map(lines);
        var game = new Game(map);
        var counter = 0;
        var wholePath = new List<Node>();
        var result = false;
        var path = new List<Node>();
        var isCycle = false;
        var startingNodes = map.Nodes.Where(node => node.Name == "AAA").ToList();
        var wholePaths = startingNodes.Select(n =>
        {
            var listStarting = new List<Node>() { n };
            return listStarting;
        }).ToList();
        var results = startingNodes.Select(s => false).ToList();
        do
        {
            counter++;
            for (int i = 0; i < wholePaths.Count; i++)
            {
                var wp = wholePaths[i];
                (result,path) = game.FollowInstructions( wp.Last().Name);
                results[i] = result;
                wp = AddPath(wp,path);
            }
            
        } while (AllTruth(results));
        Console.WriteLine("Aoc 2023 day 08 part 1 result "+ (wholePaths[0].Count - 1));
    }

    private static bool AllTruth(List<bool> results)
    {
        return results.Exists(r => r == false);
    }

    private static List<Node> AddPath(List<Node> wholePath, List<Node> path)
    {
        if (wholePath.Count!=0 && wholePath.Last().Name == path.First().Name) 
        {
            wholePath.RemoveAt(wholePath.Count - 1);
            wholePath.AddRange(path);
        } else wholePath.AddRange(path);

        return wholePath;
    }

    private static void PrintPath(List<Node> path)
    {
        Console.WriteLine(string.Join('-', path.Select(n => n.Name).ToList()));
    }
}