using Tools;

namespace A23D2;
class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        var games = new List<Game>();
        
        lines.ForEach(l => games.Add(new Game(l)));
        
        var banchSet = new Set(12, 13, 14);
        var result = 0;
        
        games.ForEach(g => result += SatisfiedGame(g, banchSet));
        
        Console.WriteLine("Aoc 2023 day 02 Part 1 result: "+result);

        var part2Result = games.Sum(g => g.MinSet.Blue * g.MinSet.Red * g.MinSet.Green);
        Console.WriteLine("Aoc 2023 day 02 Part 2 result: "+ part2Result);
    }

    private static int SatisfiedGame(Game game, Set benchSet) => game.Sets.All(set => benchSet.Red >= set.Red && benchSet.Green >= set.Green && benchSet.Blue >= set.Blue) ? game.Id : 0;

}