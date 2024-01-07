using Tools;
namespace A23D7;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine(lines.Count);
        
        var game2 = new CamelGame(lines);
        game2.Sort(2);
        Console.WriteLine("Aoc 2023 Day 07 Part 2 result: "+GetTotal(game2));
    }

    private static long GetTotal(CamelGame game)
    {
        var length = game.Cards.Count;
        var total = 0;
        for (int i = 0; i < length; i++)
        {
            var c = game.Cards[i];
            var rank = length - i;
            var prize = rank * c.Bid;
            total += prize;
        }
    
        return total;
    }
    
}