using Tools;

namespace A23D11;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine(lines.Count);
        var lavaIsland = new LavaIsland(lines);
        var sum = lavaIsland.Maps.Sum(m => m.Value);
        var sumChanged = lavaIsland.Maps.Sum(m => m.ChangedValue);
        Console.WriteLine("AoC 2023 Day 13 part1 result: "+ sum);
        Console.WriteLine("AoC 2023 Day 13 part2 result: "+ sumChanged);
    }
}