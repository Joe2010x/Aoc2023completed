using System.Runtime.Serialization.Json;

namespace A23D9;

public class Oasis
{
    public List<Sequence> Sequences { get; set; }

    public Oasis(List<string> lines)
    {
        var sequences = new List<Sequence>();
        foreach (var line in lines)
        {
            sequences.Add(new Sequence(line, null));
        }
        Sequences = sequences;
    }

    public void ProduceNexts()
    {
        foreach (var sequence in Sequences)
        {
            sequence.ProduceNext();
        }
    }
}

public class Sequence
{
    public string Line { get; set; }
    
    public List<int> Items { get; set; }
    
    public Sequence Difference { get; set; }
    
    public Sequence? Upper { get; set; }
    
    public string DifferenceStr { get; set; }
    
    public bool DifferenceAllZero { get; set; }
    
    public int Next { get; set; }
    
    public int PreValue { get; set; }

    public Sequence(string line, Sequence upper)
    {
        Line = line;
        Items = line.Split(' ').Where(s => s != "").Select(s=> int.Parse(s)).ToList();
        Upper = upper;
        Next = -1;
        DifferenceStr = ProduceSubsequence();
        DifferenceAllZero = DifferenceStr.Split(' ').All(i => int.Parse(i) == 0);
        if (!DifferenceAllZero) Difference = new Sequence(DifferenceStr, this);
        Next = GetNext();
        PreValue = GetPrev();
        
    }
    private static void PrintSequence(Sequence seq)
    {
        Console.WriteLine("Prev "+ seq.PreValue+" "+string.Join(' ', seq.Items) +" next is " +seq.Next  );
    }

    private int GetPrev()
    {
        
        if (DifferenceAllZero) return Items.First();
        return Items.First() - Difference.GetPrev();
    }

    public int GetNext()
    {
        if (DifferenceAllZero) return Items.Last();
        return Items.Last() + Difference.GetNext();
    }
    public void ProduceNext()
    {
        if (DifferenceAllZero)
        {
            Console.WriteLine("difference all zero true");
            Next = Items.Last();
        }
        else
        { 
                Difference.ProduceNext();
        }
    }

    private string ProduceSubsequence()
    {
        var str = "";
        for (int i = 0; i < Items.Count - 1; i++)
        {
            var diff = Items[i + 1] - Items[i];
            str = str + diff + " ";
        }

        return str.Trim();
    }
}