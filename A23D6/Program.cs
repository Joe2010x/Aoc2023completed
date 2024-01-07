using Tools;
namespace A23D6;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("d6pc2.txt");
        var game = new Game(lines);
        var wins = new List<int>();
        foreach (var race in game.Races)
        {
            var win = race.PossibleWins();
            wins.Add(win);
        }
        var result = wins.Aggregate(1, (a, b) => a * b);
        Console.WriteLine("Aoc 2023 Day 06 Part 1 result: "+ result);
        Console.WriteLine("Aoc 2023 Day 06 Part 2 result: "+game.Race2.PossibleWins());
    }
}