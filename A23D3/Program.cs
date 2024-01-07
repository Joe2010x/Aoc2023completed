using Tools;
namespace A23D3;
class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        var map = new Map(lines);
        var sum = map.PartNumbers
            .Where(p=> p.IsPartNumber)
            .Sum(p => p.Number);
        Console.WriteLine("Aoc 2023 day 03 Part 1 result: "+sum);
        var sumGears = map.Stars
            .Where(s => s.Parts.Count == 2)
            .Sum(s => s.Parts[0].Number * s.Parts[1].Number);
        Console.WriteLine("Aoc 2023 day 03 Part 2 result: "+sumGears);
    }
}