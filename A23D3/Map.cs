namespace A23D3;

public class Map
{
    public List<string> Lines { get; set; }
    
    public List<PartNumber> PartNumbers { get; set; }
    
    public string Signs { get; set; }
    
    public List<Star> Stars { get; set; }

    public Map(List<string> lines)
    {
        PartNumbers = new List<PartNumber>();
        Stars = new List<Star>();
        Lines = lines;
        Signs = FindAllSigns(lines);
        var numbers = "0123456789";
        for (int i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var word = "";
            for (int j = 0; j < line.Length; j++)
            {
                var str = line.Substring(j);
                var capitalLetter = str[0];
                if (capitalLetter == '*') Stars.Add(new Star(i,j));
                if (numbers.Contains(capitalLetter))
                {
                    word += capitalLetter;
                }
                else if (word.Length > 0)
                {
                    PartNumbers.Add(new PartNumber(i,j - word.Length ,word,lines, Signs, this));
                    word = "";
                } 
                if (j == line.Length - 1 && word.Length > 0) 
                    PartNumbers.Add(new PartNumber(i,j - word.Length ,word,lines, Signs, this));
            }
        }
        
        // add all partnumbers to stars
        foreach (var star in Stars)
        {
            var row = star.Row;
            var col = star.Col;
            var partNumbers = new List<PartNumber>();
            for (int i = 0; i < PartNumbers.Count; i++)
            {
                var part = PartNumbers[i];
                if (part.Row == row - 1 || part.Row == row || part.Row == row + 1)
                {
                    if ((part.Col <= col && col <= part.Col + part.Number.ToString().Length - 1) ||
                        (part.Col <= col - 1 && col - 1 <= part.Col + part.Number.ToString().Length - 1) ||
                        (part.Col <= col + 1 && col + 1 <= part.Col + part.Number.ToString().Length - 1))
                        partNumbers.Add(part);
                }
            }

            star.Parts = partNumbers;
        }
    }

    public string? FindAllSigns(List<string> lines)
    {
        var str = "";
        foreach (var line in lines)
        {
            str += line;
        }

        var numbers = "0123456789";
        str = str.Replace(".", "");
        var listChar = str.Where(s => !numbers.Contains(s)).Distinct();
        return new string(listChar.ToArray());
    }
    
    public string? FindAllSigns(string line)
    {
        var str = line;
        var numbers = "0123456789";
        str = str.Replace(".", "");
        var listChar = str.Where(s => !numbers.Contains(s)).Distinct();
        return new string(listChar.ToArray());
    }
}

public class Star
{
    public int Row { get; set; }
    public int Col { get; set; }

    public List<PartNumber>  Parts { get; set; }

    public Star(int row, int col)
    {
        Row = row;
        Col = col;
    }
}

public class PartNumber
{
    public int Number { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public string Surroundings { get; set; }
    
    public bool IsPartNumber { get; set; }
    

    public PartNumber(int row, int col, string num, List<string> lines, string signs, Map map)
    {
        Row = row;
        Col = col;
        Number = int.Parse(num);
        var numLength = num.Length;
        var high = lines.Count;
        var length = lines[0].Length;
        var top = Row != 0 ? lines[Row - 1].Substring(col, num.Length) : "";
        var buttom = Row != high - 1 ? lines[Row + 1].Substring(col, num.Length) : "";
        
        var topLeft = col != 0 ? Row != 0 ? lines[Row - 1].Substring(col - 1, 1) : "":"";
        var leftStr = col != 0 ? lines[Row].Substring(col - 1, 1):"";
        var buttomLeft = col != 0 ? Row != length - 1 ? lines[Row +1].Substring(col - 1, 1) : "":"";
        var left =  topLeft + leftStr + buttomLeft ;
        
        var topRight = col + numLength < length ? Row != 0 ? lines[Row - 1].Substring(col + numLength , 1) : "" :"";
        var rightStr = col + numLength < length ? lines[Row].Substring(col + numLength , 1):"";
        var buttomRight = col + numLength < length ? Row != length - 1 ? lines[Row + 1].Substring(col + numLength , 1) : "":"";
        var right = topRight + rightStr + buttomRight ;
        Surroundings = top + buttom + left + right;

        var signsSurr = map.FindAllSigns(Surroundings);
        
        var isPartNumber = true;
        
        foreach (var c in signsSurr)
        {
            if (!signs.Contains(c)) {
                isPartNumber = false;
                break;
            }
        }
        IsPartNumber = signsSurr.Length != 0 && isPartNumber;
    }
        
}