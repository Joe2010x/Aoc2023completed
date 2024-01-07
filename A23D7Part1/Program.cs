using System.Runtime.InteropServices;
using Tools;

class Program
{
    static void Main(string[] args)
    {
        var lines = FileReaderClass.ReadFromFile("real.txt");
        Console.WriteLine(lines.Count);
        var cards = lines.Select(line => new Card(line)).ToList();
        cards.Sort((x, y) => x.CompareTo(y));
        Console.WriteLine("Aoc 2023 Day 07 Part 1 result: "+ GetTotal(cards));
    }
    private static long GetTotal(List<Card> cards)
    {
        var length = cards.Count;
        var total = 0;
        for (int i = 0; i < length; i++)
        {
            var c = cards[i];
            var rank = i + 1 ;
            var prize = rank * c.Bid;
            total += prize;
        }

        return total;
    }
}
class Card
{
    public string Line { get; set; }
    public string Hand { get; set; }
    public int Bid { get; set; }
    public CardType CardType { get; set; }

    public Card(string line)
    {
        var cardOrder = "AKQJT98765432";
        Line = line;
        var split = Line.Split(' ');
        Hand = split[0];
        Bid = int.Parse(split[1]);
        CardType = GetCardType(cardOrder);
    }

    public int CompareTo(Card card2)
    {
        var cardOrder = "AKQJT98765432";
        var card1 = this;
        if (card1.CardType < card2.CardType) return 1;
        if (card1.CardType == card2.CardType && CompareHand(card1.Hand, card2.Hand, cardOrder)) return 1;
        return -1;
    }
    
    private bool CompareHand(string hand1, string hand2, string cardOrder)
    {
        if (cardOrder.IndexOf(hand1[0]) < cardOrder.IndexOf(hand2[0])) return true;
        
        if (hand1[0] == hand2[0])
        {
            if (hand1.Length == 1)
            {
                // Console.WriteLine("found same hands");
                return true;
            }
            return CompareHand(hand1.Substring(1), hand2.Substring(1), cardOrder);
        }
        return false;
    }

    private CardType GetCardType(string cardOrder)
    {
        if (cardOrder.Any(c => Hand.ToCharArray().ToList().Count(h => h == c) == 5)) return CardType.FiveAKind;
        if (cardOrder.Any(c => Hand.ToCharArray().ToList().Count(h => h == c) == 4)) return CardType.FourAKind;
        var c1 = cardOrder.FirstOrDefault(c => Hand.ToCharArray().ToList().Count(h => h == c) == 3);
        var c2 = cardOrder.FirstOrDefault(c => Hand.ToCharArray().ToList().Count(h => h == c) == 2);
        if ( c1 != '\0' && c2 != '\0' &&  c1 != c2) return CardType.FullHouse;
        if (cardOrder.Any(c => Hand.ToCharArray().ToList().Count(h => h == c) == 3)) return CardType.ThreeAKind;
        var c2List = cardOrder.Select(c => Hand.ToCharArray().ToList().Count(h => h == c) == 2).Count(c => c);
        if (c2List == 2) return CardType.TwoPairs;
        if (c2List == 1) return CardType.OnePair;
        return CardType.HighCard;
    }
}
public enum CardType { FiveAKind, FourAKind, FullHouse, ThreeAKind, TwoPairs, OnePair, HighCard }
