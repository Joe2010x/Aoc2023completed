using Tools;

namespace A23D9;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine("number of lines "+lines.Count);
        var oasis = new Oasis(lines);
        var (sumPrev, sumNext) = GetSum(oasis.Sequences);
        Console.WriteLine("Advent of code 2023 day 9 part 1 "+sumNext);
        Console.WriteLine("Advent of code 2023 day 9 part 2 "+sumPrev);
    }
    
    private static (long, long) GetSum(List<Sequence> oasisSequences)
    {
        long resultPrev = 0;
        long resultNext = 0;
        foreach (var seq in oasisSequences)
        {
            resultPrev += seq.PreValue;
            resultNext += seq.Next;
        }

        return (resultPrev,resultNext);
    }
}