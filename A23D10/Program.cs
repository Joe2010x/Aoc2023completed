using Tools;

namespace A23D10;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var lines = FileReaderClass.ReadFromFile("real.txt");
        var lines2 = FileReaderClass.ReadFromFile("signMap.txt");
        Console.WriteLine(lines.Count);
        var map = new Map(lines,lines2);
        Console.WriteLine("Aoc 2023 day 10 part 2 "+map.InsideCube);
    }
}

