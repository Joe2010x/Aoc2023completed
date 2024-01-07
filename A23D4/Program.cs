using System.Runtime.InteropServices;
using Tools;
namespace A23D4;
class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("test0.txt");
        Console.WriteLine(lines.Count);
        var game = new Game(lines);
        Console.WriteLine("Aoc 2023 Day 04 Part 1 result: "+game.Cards.Sum(c=> c.Points));
        var totalCards = game.Cards.Count;
        for (int i = 0; i < game.Cards.Count; i++)
        {
            var card = game.Cards[i];
            var numOfWinningNumber = card.MyWinningNums.Count;
            for (int j = 1; j <= numOfWinningNumber; j++)
            {
                if (i + j < totalCards)
                    game.Cards[i+j].NumOfCardsInHand += card.NumOfCardsInHand ;
            }
        }
        Console.WriteLine("Aoc 2023 Day 04 Part 2 result: "+game.Cards.Sum(c=> c.NumOfCardsInHand));
    }
}