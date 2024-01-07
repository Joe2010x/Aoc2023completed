using Tools;

namespace A23D12;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        var puzzle = new Puzzle(lines);
        Console.WriteLine("Aoc 2023 day 12 part 1 result "+puzzle.Total);
        Console.WriteLine("Aoc 2023 day 12 part 1 result "+puzzle.Total2);
    }
}