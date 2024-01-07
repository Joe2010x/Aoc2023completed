using System.Runtime.ExceptionServices;
using System.Xml;

namespace A23D7;

public class CamelGame
{
    public List<Card> Cards { get; set; }

    public List<Card> CardsSorted { get; set; }

    public void Sort(int indicator)
    {
        
        for (int i = 0; i < Cards.Count - 1; i++)
        {
            // var a = Cards[i];
            for (int j = i + 1; j < Cards.Count; j++)
            {
                // var b = Cards[j];
                if (Cards[j].Greater2(Cards[i], indicator))
                {
                    // Console.WriteLine(" do exchange "+Cards[j].Hands+ " greater "+Cards[i].Hands);
                    var tempCard = new Card(Cards[i].Line);
                    Cards[i] = Cards[j];
                    Cards[j] = tempCard;
                }
            }
        }
    }
    
    public CamelGame(List<string> lines)
    {
        Cards = lines.Select(l => new Card(l)).ToList();
    }
}

public class Card
{
    public string Hands { get; set; }
    public int Bid { get; set; }
    
    public string CardOrder { get; set; }
    public string CardOrder1 { get; set; }
    public string Line { get; set; }

    public CardType CardType { get; set; }
    

    public Card(string line)
    {
        CardOrder  = "AKQT98765432J";
        CardOrder1 = "AKQJT98765432";
        var split = line.Split(' ').Where(s => s != "").ToList();
        Hands = split[0];
        Bid =  split.Count == 1 ?  0: int.Parse(split[1]);
        Line = line;
        var cardDictionary = CreateCardDictionary(Hands);
        CardType = new CardType(cardDictionary, Hands);
    }

    private Dictionary<char,int> CreateCardDictionary(string hands)
    {
        var cards = hands.ToCharArray().Distinct().ToList();
        var dict = new Dictionary<char, int>();
        foreach (var c in cards)
        {
            var num = hands.Count(h => h == c);
            dict.Add(c,num);
        }
        return dict;
    }

    private bool CompareHand(string hand1, string hand2, int indicator)
    {
        if (indicator == 1)
        {
            if (CardOrder1.IndexOf(hand1[0]) < CardOrder1.IndexOf(hand2[0])) return true;
        }
        else if (CardOrder.IndexOf(hand1[0]) < CardOrder.IndexOf(hand2[0])) return true;
        
        if (hand1[0] == hand2[0])
        {
            if (hand1.Length == 1) return false;
            return CompareHand(hand1.Substring(1), hand2.Substring(1), indicator);
        }
        return false;
    }

    public bool Greater2(Card card, int i)
    {
        var c1 = CardType;
        var c2 = card.CardType;
        
        if (IsFiveOfAKind(c1, i) && !IsFiveOfAKind(c2, i)) return true;
        if (IsFiveOfAKind(c2, i) && !IsFiveOfAKind(c1, i)) return false;
        if (IsFiveOfAKind(c1, i) && IsFiveOfAKind(c2, i)) return CompareHand(c1.Hand, c2.Hand, i);

        if (Is4OfAKind(c1, i) && !Is4OfAKind(c2, i)) return true;
        if (Is4OfAKind(c2, i) && !Is4OfAKind(c1, i)) return false;
        if (Is4OfAKind(c1, i) && Is4OfAKind(c2, i)) return CompareHand(c1.Hand, c2.Hand, i);

        if (IsFullHouse(c1, i) && !IsFullHouse(c2, i)) return true;
        if (IsFullHouse(c2, i) && !IsFullHouse(c1, i)) return false;
        if (IsFullHouse(c1, i) && IsFullHouse(c2, i)) return CompareHand(c1.Hand, c2.Hand, i);
        
        if (IsThreeKind(c1, i) && !IsThreeKind(c2, i)) return true;
        if (IsThreeKind(c2, i) && !IsThreeKind(c1, i)) return false;
        if (IsThreeKind(c1, i) && IsThreeKind(c2, i)) return CompareHand(c1.Hand, c2.Hand, i);
        
        if (IsTwoPair(c1, i) && !IsTwoPair(c2, i)) return true;
        if (IsTwoPair(c2, i) && !IsTwoPair(c1, i)) return false;
        if (IsTwoPair(c1, i) && IsTwoPair(c2, i)) return CompareHand(c1.Hand, c2.Hand, i);
        
        if (IsPair(c1, i) && !IsPair(c2, i)) return true;
        if (IsPair(c2, i) && !IsPair(c1, i)) return false;
        if (IsPair(c1, i) && IsPair(c2, i)) return CompareHand(c1.Hand, c2.Hand, i);
        
        return CompareHand(c1.Hand, c2.Hand, i);
    }

    private bool IsPair(CardType c1, int indicator)
    {
        if (indicator == 1) return IsOldPair(new Card(c1.Hand).CardType);
        var carSetNoJ = CardOrder.Substring(0, CardOrder.Length - 1);
        var result = carSetNoJ.Any(newChar => IsOldPair(new Card ( c1.Hand.Replace('J',newChar) ).CardType ) ) ;
        return result;
    }
    
    private bool IsOldPair(CardType c1)
    {
        return c1.KindOfTwo == null ? false : c1.KindOfTwo.Count == 1;
    }

    private bool IsTwoPair(CardType c1, int indicator)
    {
        if (indicator == 1) return IsOldTwoPair(new Card(c1.Hand).CardType);
        var carSetNoJ = CardOrder.Substring(0, CardOrder.Length - 1);
        var result = carSetNoJ.Any(newChar => IsOldTwoPair(new Card ( c1.Hand.Replace('J',newChar) ).CardType ) ) ;
        return result;
    }
    
    private bool IsOldTwoPair(CardType c1)
    {
        
        return c1.KindOfTwo == null ? false : c1.KindOfTwo.Count == 2;
    }

    private bool IsThreeKind(CardType c1, int indicator)
    {
        if (indicator == 1) return IsOldThreeKind(new Card(c1.Hand).CardType);
        var carSetNoJ = CardOrder.Substring(0, CardOrder.Length - 1);
        var result = carSetNoJ.Any(newChar => IsOldThreeKind(new Card ( c1.Hand.Replace('J',newChar) ).CardType ) ) ;
        return result;
    }  
    
    private bool IsOldThreeKind(CardType c1)
    {
        return c1.KindOfThree is not null;
    }

    private bool IsFullHouse(CardType c1, int indicator)
    {
        if (indicator == 1) return IsOldFullHouse(new Card(c1.Hand).CardType);
        var carSetNoJ = CardOrder.Substring(0, CardOrder.Length - 1);
        var result = carSetNoJ.Any(newChar => IsOldFullHouse(new Card ( c1.Hand.Replace('J',newChar) ).CardType ) ) ;
        return result;
    }

    private bool IsOldFullHouse(CardType c1)
    {
        
        return c1.KindOfThree is not null && c1.KindOfTwo is not null;
    }


    private bool Is4OfAKind(CardType c1, int indicator)
    {
        if (indicator == 1) return IsOldFiveOfAKind(new Card(c1.Hand).CardType);
        var carSetNoJ = CardOrder.Substring(0, CardOrder.Length - 1);
        var result = carSetNoJ.Any(newChar => IsOld4OfAKind(new Card ( c1.Hand.Replace('J',newChar) ).CardType ) ) ;
        return result;
    }

    private bool IsOld4OfAKind(CardType c1)
    {
        return c1.KindOfFour is not null;
    }

    private bool IsFiveOfAKind(CardType c1, int indicator)
    {
        if (indicator == 1) return IsOldFiveOfAKind(new Card(c1.Hand).CardType);
        var cardSetNoJ = CardOrder.Substring(0, CardOrder.Length - 1);
        var result = cardSetNoJ.Any(newChar => IsOldFiveOfAKind(new Card ( c1.Hand.Replace('J',newChar) ).CardType ) ) ;
        return result;
    }

    private bool IsOldFiveOfAKind(CardType c1)
    {
        return c1.KindOfFive is not null;
    }
}

public class CardType
{
    public string Hand { get; set; }
    public int NumOfKind { get; set; }
    public char? KindOfFive { get; set; } 
    
    public char? KindOfFour { get; set; }
    public char? KindOfThree { get; set; }
    public List<char>? KindOfTwo { get; set; }
    public List<char>? KindOfOne { get; set; }

    public void Print()
    {
        Console.WriteLine(Hand+ " num of kind "+NumOfKind);
        Console.WriteLine("kind of 5 is "+ (KindOfFive == null ? "null" : KindOfFive)); 
        Console.WriteLine("kind of 4 is "+ (KindOfFour == null ? "null" : KindOfFour)); 
        Console.WriteLine("kind of 3 is "+(KindOfThree == null ? "null" : KindOfThree)); 
        Console.WriteLine("kind of 2 is " + (KindOfTwo == null ? "null" : string.Join(',', KindOfTwo)));
        Console.WriteLine("kind of 1 is "+ (KindOfOne == null ? "null" : string.Join(',', KindOfOne)));
    }

    public CardType(Dictionary<char, int> dict, string hand)
    {
        Hand = hand;
        NumOfKind = dict.Count;
        KindOfFive =  dict.FirstOrDefault(cd => cd.Value == 5).Equals(default(KeyValuePair<char, int>)) 
            ? null 
            : dict.FirstOrDefault(cd => cd.Value == 5).Key ;
        KindOfFour = dict.FirstOrDefault(cd => cd.Value == 4).Equals(default(KeyValuePair<char, int>)) 
            ? null 
            : dict.FirstOrDefault(cd => cd.Value == 4).Key ;
        KindOfThree = dict.FirstOrDefault(cd => cd.Value == 3).Equals(default(KeyValuePair<char, int>)) 
            ? null 
            : dict.FirstOrDefault(cd => cd.Value == 3).Key ;
        KindOfTwo = dict.FirstOrDefault(cd => cd.Value == 2).Equals(default(KeyValuePair<char, int>)) 
            ? null 
            : dict.Where(cd => cd.Value == 2)
                .Select(cd => cd.Key)
                .ToList();
        KindOfOne = dict.FirstOrDefault(cd => cd.Value == 1).Equals(default(KeyValuePair<char, int>)) 
            ? null 
            : dict.Where(cd => cd.Value == 1).Select(cd => cd.Key).ToList();
        // Print();
    }
}

public class FourOfAKind
{
    public string Hand { get;  }
    public bool IsFourOfAKind { get; set; }
    public char? KindFour { get; set; }
    public char? KindOne { get; set; }
    public FourOfAKind(Dictionary<char, int> cardDictionary)
    {
        var numOfKind = cardDictionary.Count;
        IsFourOfAKind = cardDictionary.Any(cd => cd.Value == 4);
        KindFour = IsFourOfAKind ? cardDictionary.Where(cd => cd.Value == 4).First().Key: null;
        KindOne = IsFourOfAKind ? cardDictionary.Where(cd => cd.Value == 1).First().Key: null;
    }
}

public class FiveOfAKind
{
    public bool IsFullHouse { get; set; }
    public char? Kind { get; set; }

    public FiveOfAKind(Dictionary<char, int> cardDictionary)
    {
        var numKinds = cardDictionary.Count();
        IsFullHouse = numKinds == 1;
        Kind = IsFullHouse ? cardDictionary.Where(cd => cd.Value == 5).First().Key: null;
    }
}