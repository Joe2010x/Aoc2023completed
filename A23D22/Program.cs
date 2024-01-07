using Tools;

namespace A23D22;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023!");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        Console.WriteLine(lines.Count);
        var sandPill = new SlabPill(lines);
        Console.WriteLine("Aoc 2023 Day 22 part 1 "+sandPill.NumberOfMoveable);
        Console.WriteLine("Aoc 2023 Day 22 part 2 "+ sandPill.TotalFallable);
        
    }
}