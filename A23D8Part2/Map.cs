namespace Practice1;

public class Map
{
    public string Instruction { get; set; }
    public List<Node> Nodes { get; set; }

    public Map(List<string> lines)
    {
        Instruction = lines[0];
        Nodes = lines.GetRange(2, lines.Count - 2).Select(l => new Node(l)).ToList();
        MapNames();
    }

    private void MapNames()
    {
        foreach (var node in Nodes)
        {
            node.Left = LookupNode(node.LeftName);
            node.Right = LookupNode(node.RightName);
        }
    }

    public Node LookupNode(string name)
    {
        return Nodes.Find(n => n.Name == name);
    }
}

public class Node
{
    public string Name { get; set; }
    public string LeftName { get; set; }
    public string RightName { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }

    public Node(string line)
    {
        var split = line.Split('=');
        Name = split[0].Trim();
        var ins = split[1].Split(',');
        LeftName = ins[0].Replace("(", "").Trim();
        RightName = ins[1].Replace(")", "").Trim();
        // Print();
    }

    public void Print()
    {
        Console.WriteLine("The node Name "+Name+" left "+LeftName+" right "+RightName);
    }
}

public class Game
{
    public Map Map { get; set; }
    public List<Node> Path { get; set; }
    
    

    public Game(Map map)
    {
        Map = map;
        Path = new List<Node>();
    }

    public (bool result, List<Node> path) FollowInstructions(string startIngPosition)
    {
        // Console.WriteLine("starting "+startIngPosition);
        var ins = Map.Instruction;
        // Console.WriteLine("the instruction is "+ ins);
        Path = new List<Node>();
        Path.Add(Map.LookupNode(startIngPosition));
        var currentNode = Path.Last();
        foreach (var letter in ins)
        {
            Step(letter,currentNode);
            currentNode = Path.Last();
        }
        
        // PrintPath(Path);
        return (FoundEnding('Z') , Path);
    }

    public bool FoundEnding(char c)
    {
        return Path.Last().Name.Last() == c;
    }

    private bool Found(string name)
    {
        return Path.Any(n => n.Name == name);
    }
    private static void PrintPath(List<Node> path)
    {
        Console.WriteLine(string.Join('-', path.Select(n => n.Name).ToList())+" found ending Z "+(path.Last().Name.Last() == 'Z'));
    }

    private void Step(char letter, Node current)
    {
        if (letter == 'L')
        {
            Path.Add(current.Left);
        } else
        {
            Path.Add(current.Right);
        }
    }
}