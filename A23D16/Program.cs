using Tools;

namespace A23D16;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023 day 16");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        var map = new Map(lines);
        Console.WriteLine("Aoc2023 Day 16 part 1 : "+map.CalculateWithStartingPosition(0,0,1));
        Console.WriteLine("Aoc2023 Day 16 part 2 : "+map.CalculateStartFromAllEdges());
    }
}