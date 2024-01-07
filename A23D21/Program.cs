using Tools;

namespace A23D21;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine("number of lines " + lines.Count);
        var walker = new Walker(lines);
        Console.WriteLine("Aoc 2023 day 21 part 1 result "+walker.WalkAndCountSteps(64));
        Console.WriteLine("Aoc 2023 day 21 part 2 result "+walker.PartTwo());
    }
}
