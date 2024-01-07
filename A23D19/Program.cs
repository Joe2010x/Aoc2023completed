using Tools;

namespace A23D19;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023!");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        Console.WriteLine(lines.Count);
        var partSort = new SortParts (lines);
        Console.WriteLine("Aoc 2023 day 19 part 1 result "+partSort.Part1ret);
        Console.WriteLine("Aoc 2023 day 19 part 2 result "+partSort.Total);
    }
} 