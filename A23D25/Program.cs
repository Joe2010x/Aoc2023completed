using Tools;

namespace A23D25;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023!");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        Console.WriteLine(lines.Count);
        var board = new Board(lines);
        Console.WriteLine("Aoc 2023 day 25 result "+board.Result);
    }
}