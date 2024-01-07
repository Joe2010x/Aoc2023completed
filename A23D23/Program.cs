
using Tools;

namespace A23D23;

class Program
{
    static void Main()
    {
        var lines = FileReaderClass.ReadFromFile("test0.txt");

        var start = new Position(0, 1);
        var end = new Position(lines.Count - 1, lines[0].Length - 2);

        var maze = new Maze(lines);
        Console.WriteLine("Aoc 2023 day 23 Part 1 result "+maze.FindLongestPath(new HashSet<Position>(),start,end,maze.graph1));
        // it takes about 40 mins to get the part2 result
        Console.WriteLine("Aoc 2023 day 23 Part 2 result "+maze.FindLongestPath(new HashSet<Position>(),start,end,maze.graph2));
    }
    
}
