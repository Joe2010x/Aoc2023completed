using Tools;

namespace A23D18;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023!");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        Console.WriteLine(lines.Count);
        var lavaLagoon = new LavaLagoon(lines);
        var totalArea = lavaLagoon.TotalArea(1);
        Console.WriteLine("Advent of code 2023 day 18 part 1: "+totalArea);
        var totalArea2 = lavaLagoon.TotalArea(2);
        Console.WriteLine("Advent of code 2023 day 18 part 2: "+totalArea2);
    }
}