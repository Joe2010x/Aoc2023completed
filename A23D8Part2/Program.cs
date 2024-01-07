using Tools;
namespace Practice1;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine(lines.Count);
        var map = new Map(lines);
        var game = new Game(map);
        var counter = 0;
        var wholePath = new List<Node>();
        var result = false;
        var path = new List<Node>();
        var isCycle = false;
        var startingNodes = map.Nodes.Where(node => node.Name.Last() == 'A').ToList();
        var wholePaths = startingNodes.Select(n =>
        {
            var listStarting = new List<Node>() { n };
            return listStarting;
        }).ToList();
        var results = startingNodes.Select(s => false).ToList();
        var newEndingNoods = startingNodes.Select(s => "").ToList();
        var resultPaths = new List<List<Node>>();
        wholePaths.ForEach(sn =>
        {
            
            var counter = 0;
            var newPath = sn ;
            do
            {
                
                (result,path) = game.FollowInstructions( newPath.Last().Name);
                newPath = AddPath(newPath,path);
                counter++;
            } while (!result);
            resultPaths.Add(newPath);
        });
            
        var collections = resultPaths.Select(p => p.Count - 1).ToList();
        
        var gcd = GCD(collections.ToArray());
        collections = collections.Select(c => c / gcd).ToList();
        collections.Add(gcd);
        ulong resultx= (ulong) Multiple( collections);
        Console.WriteLine("Aoc 2023 day 08 part 2 result "+resultx);
    }

    private static ulong Multiple(List<int> collections)
    {
        ulong result = 1;
        foreach (var c in collections)
        {
            result *= (ulong) c;
        }

        return result;
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
    
    public static int GCD (int[] numbers)
    {
        return numbers.Aggregate(GCD);
    }

    public static int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
}