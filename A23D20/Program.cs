using Tools;

namespace A23D20;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Advent of code 2023!");
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        var pulse = new Pulse(lines,1000);
        Console.WriteLine("Aoc 2023 day 20 part 1 result "+pulse.Lows*pulse.Highs);
        var pulseX = new Pulse(lines,2023);
        Console.WriteLine("Aoc 2023 day 20 part 2 result "+ LCM(pulseX.FoundHigh));
    }
    
    public static long LCM(IEnumerable<long> stuff)
    {
        return stuff.Aggregate((a, i) => (a / GCF(a, i)) * i);
    }
    public static long GCF(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}