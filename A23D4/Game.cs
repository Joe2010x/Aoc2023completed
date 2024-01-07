
namespace A23D4;

public class Game
{
    public List<Card> Cards { get; set; }

    public Game(List<string> lines)
    {
        var cards = lines.Select(l => new Card(l)).ToList();
        Cards = cards;
    }
}

public class Card
{
    public int Id { get; set; }
    public List<int> WinningNum { get; set; }
    public List<int> MyNum { get; set; }
    public List<int> MyWinningNums { get; set; }
    public int Points { get; set; }

    public int NumOfCardsInHand { get; set; }

    public Card(string line)
    {
        var gameArr = line.Split(':');
        Id = int.Parse(gameArr[0].Split(' ').Where(c => c.Length != 0).ToList()[1]);
        var numArr = gameArr[1].Split('|');
        var wNumList = numArr[0].Trim().Split(' ').Where(c => c.Length != 0).ToList();
        WinningNum = wNumList
            .Select(n =>
        {
            return int.Parse(n.Trim());
        }).ToList();
        wNumList = numArr[1].Trim().Split(' ').ToList();
        MyNum =  wNumList
            .Where (s=> s.Trim().Length != 0)
            .Select(n =>
        {
            return int.Parse(n.Trim());
        }).ToList();
        MyWinningNums = MyNum.Where(n => WinningNum.Contains(n)).ToList();
        Points = (int) Math.Pow(2,  (MyWinningNums.Count - 1));
        NumOfCardsInHand = 1;
    }
}