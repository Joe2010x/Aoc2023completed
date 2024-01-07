using System.Runtime.InteropServices;
using Tools;
namespace A23D5;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("d5pc.txt");
        var sourceMap = new SourceMap(lines);
        // the second part of the result will run for about 40 - 60 mins. 
        var resultList1 = sourceMap.Seeds1.Select(thisSeed => sourceMap.ToFinalDestinationa(thisSeed)).ToList();
        Console.WriteLine("Aoc 2023 Day 5 Part 1 result: "+ resultList1.Min());
        var resultList = sourceMap.Seeds.Select(thisSeed => sourceMap.ToFinalDestinationa(thisSeed)).ToList();
        Console.WriteLine("Aoc 2023 Day 5 Part 2 result: "+resultList.Min());
    }
}