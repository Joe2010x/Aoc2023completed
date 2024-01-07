using Tools;

namespace A23D24;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023!");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        Console.WriteLine(lines.Count);
        var weather = new Weather(lines, 200000000000000,400000000000000);
        Console.WriteLine("Aoc 2023 day 24 part 1 result "+weather.Intersects);
        Console.WriteLine("Aoc 2023 day 24 part 2 result "+weather.Solve(weather.Hails));
    }
}