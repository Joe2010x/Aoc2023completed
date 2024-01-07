using Tools;

namespace A23D11;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine("Total num of lines "+lines.Count);
        
        var universePart1 = new Universe(lines);
        universePart1.Expand(2);
        universePart1.CalculateDistance();
        Console.WriteLine("Advent of code 2023 day 11 Part 1 "+universePart1.TotalDistance/2);
        var universePart2 = new Universe(lines);
        
        universePart2.Expand(1000000);
        universePart2.CalculateDistance();
        Console.WriteLine("Advent of code 2023 day 11 Part 2 "+universePart2.TotalDistance/2);
    }
}