using Tools;

namespace A23D15;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023 day 15");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        var codeBook = new CodeBook(lines[0], 0);
        var values = codeBook.Codes.Select(c => c.Value).ToList();
        Console.WriteLine("Aoc 2023 day 15 part1 result: "+values.Sum(v=> v));
        Console.WriteLine("Aoc 2023 day 15 part2 result: "+codeBook.LensValue);
    }
}