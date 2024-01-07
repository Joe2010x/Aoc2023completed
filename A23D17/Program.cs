using Tools;
namespace A23D17;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("AOC 2023 DAY 17");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        var map = new LavaPuzzle(lines);
        Console.WriteLine("Aoc 2023 Day 17 part 1 result "+map.TotalHeat);
        Console.WriteLine("Aoc 2023 Day 17 part 2 result "+map.TotalHeat2);
    }
}