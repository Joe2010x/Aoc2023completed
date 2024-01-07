using Tools;

namespace A23D14;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine(lines.Count);
        var target = 1000000000;        
        var mirrors = new Mirrors(lines);
        var valuePart1 = mirrors.GetValue(1);
        var valuePart2 = mirrors.GetValue(target);
        Console.WriteLine("Aoc 2023 Day 14 part1: "+ valuePart1);
        Console.WriteLine("Aoc 2023 Day 14 part2: "+ valuePart2);
    }
}