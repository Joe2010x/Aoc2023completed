using System.Text;

namespace A23D15;

public class CodeBook
{
    public string line { get; set; }
    
    public List<Code>[] LenBoxes { get; set; }

    public List<Code> Codes { get; set; }
    
    public int LensValue { get; set; }

    public CodeBook(string line, int hash)
    {
        var split = line.Split(',');
        Codes = split.Select(l => new Code(l.Trim(), hash)).ToList();
        LenBoxes = StoreToBoxes(Codes);
        LensValue = GetValue(LenBoxes);
    }

    private int GetValue(List<Code>[]? lenBoxes)
    {
        var result = 0;
        for (int i = 0; i < lenBoxes.Length; i++)
        {
            var box = lenBoxes[i];
            result += GetBoxValue(box) * (i+1);
        }

        return result;
    }

    private int GetBoxValue(List<Code> box)
    {
        var result = 0;
        for (int i = 0; i < box.Count; i++)
        {
            var lenBox = box[i] ;
            result += lenBox.Len * (i+1);
        }

        return result;
    }

    private List<Code>[]? StoreToBoxes(List<Code> codes)
    {
        var boxes = new List<Code>[256];
        for (var i= 0; i<boxes.Length; i++)
        {
            boxes[i] = new List<Code>();
        }
        foreach (var code in codes)
        {
            var box = boxes[code.BoxNum];
            var found = box.SingleOrDefault(b => b.BoxCode == code.BoxCode);
            if (found != null)
            {
                if (code.Sign == '-') box.Remove(found);
                found.Len = code.Len;
            }
            else
            {
                if (code.Sign == '=') box.Add(code);
            } 
            
        }

        return boxes;
    }
}

public class Code
{
    public string Source { get; set; }
    
    public char Sign { get; set; }
    
    public string BoxCode { get; set; }
    
    public int BoxNum { get; set; }
    
    public int Value { get; set; }
    
    public int Len { get; set; }

    public Code(string source, int hash)
    {
        Source = source;
        (BoxCode, BoxNum, Sign, Len) = Translate(Source);
        Value = GetResult(hash, Source);
    }

    private (string BoxCode,int BoxNum, char Sign, int Len) Translate(string source)
    {
        var index = -1;
        var sign = ' ';
        for (int i = 0; i < source.Length; i++)
        {
            
            if (source[i] == '=')
            {
                sign = source[i];
                index = i;
                break;
            }            
            
            if (source[i] == '-')
            {
                sign = source[i];
                index = i;
                break;
            }
        }

        var boxCode = source.Substring(0, index);
        var len = sign == '=' ? int.Parse( source.Substring(index + 1)): -1;
        var boxNum = GetResult(0, boxCode);
        return (boxCode,boxNum, sign, len);
    }

    private int GetResult(int hash, string source)
    {
        var multiplier = 17;
        var ascii = Encoding.ASCII.GetBytes(source);
        var result = hash;
        foreach (var b in ascii)
        {
            result = GetEncriptedValue(b, result, multiplier);
        }

        return result;
    }

    public int GetEncriptedValue(int asciiValue, int hash, int multiplier)
    {
        var mob = 256;
        var result = (asciiValue + hash) * multiplier % mob;
        return result;
    }
}