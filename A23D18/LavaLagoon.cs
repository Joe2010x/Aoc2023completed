namespace A23D18;

public class LavaLagoon
{
    public List<Instruction> Instructions {get; set;}

    public List<Location> Locations {get; set;}
    public List<Location> Locations2 {get; set;}

    public long Area {get; set;}

    public long Area2 {get; set;}
    
    public long InsideCube { get; set; }

    public LavaLagoon (List<string> lines)
    {
        Instructions = lines.Select(line => new Instruction(line)).ToList();
        Locations = new List<Location>();
        Locations2 = new List<Location>();
        Locations = FellowInstruction(Instructions,1);
        Locations2 = FellowInstruction(Instructions,2);
        Area = FindArea(Locations);
        Area2 = FindArea(Locations2);
    }
    public long TotalArea(int indicator)
    {
        var numOfHash = indicator == 1? Locations.Count: Locations2.Count;
        var result = (indicator == 1 ? Area : Area2)+ numOfHash / 2 + 1;
        return result;
    }
    public long FindArea (List<Location> locations)
    {

        long area = 0;
        for (int i = 0; i < locations.Count; i++)
        {
            var prev = locations[(i == 0 ? locations.Count : i) - 1];
            var curr = locations[i];
            var next = locations[(i == (locations.Count - 1) ? -1 : i) + 1];
            area += (curr.Col * (next.Row - prev.Row));
        }

        return Math.Abs(area / 2);
        
        
        
    }
    public (Location,int,int) AddFigures(int indicator)
    {
        var lowestR= 0;
        var highestR = 0;
        var lowestC = 0;
        var highestC = 0;
        foreach (var location in indicator == 1? Locations: Locations2)
        {
            lowestR = Math.Min(lowestR, location.Row);
            highestR = Math.Max(highestR, location.Row);
            lowestC = Math.Min(lowestC, location.Col);
            highestC = Math.Max(highestC, location.Col);
        }
        var topLeft = new Location(lowestR,lowestC);
        var row = highestR - lowestR + 1;
        var col = highestC - lowestC + 1;
        return (topLeft, row, col);
    }

    public List<Location>  FellowInstruction (List<Instruction> instructions, int indicator)
    {
        var locations = new List<Location>();
        var currentLocaton = new Location(0,0);
        foreach (var instruction in instructions)
        {
            var direction = indicator == 1 ? instruction.Direction : instruction.Direction2;
            for (int i = 0; i < (indicator ==1 ? instruction.Meters : instruction.Meters2); i++)
            {
                switch (direction)
                {
                    case 'U':{
                        currentLocaton = new Location (currentLocaton.Row -1, currentLocaton.Col);
                        break;
                    }
                    case 'D': {
                        currentLocaton = new Location (currentLocaton.Row +1, currentLocaton.Col);
                        break;
                    }
                    case 'L':{
                        currentLocaton = new Location (currentLocaton.Row, currentLocaton.Col-1);
                        break;
                    }
                    case 'R': {
                        currentLocaton = new Location (currentLocaton.Row, currentLocaton.Col+1);
                        break;
                    }
                }
                locations.Add(currentLocaton);
            }
        }
        return locations;
    }
}

public class Instruction
{
    public string Line {get; set;}
    public char Direction {get; set;}
    public int Meters {get; set;}
    public string Color {get; set; }

    public char Direction2 {get;set;}
    public int Meters2 {get;set;}

    public string DirectionRull {get; set;}

    public Instruction (string line)
    {
        var split = line.Split(' ');
        Line = line;
        Direction = split[0][0];
        Meters = int.Parse(split[1]);
        Color = split[2].Substring(1,7);
        DirectionRull = "RDLU";
        var hex = Color.Substring(1,5);
        var directionsCode = Color[6];
        var direction2 = DirectionRull[int.Parse(directionsCode.ToString())];
        var meters2 = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        Direction2 = direction2;
        Meters2 = meters2;
    }
}

public class Location 
{
    public int Row {get; set;}
    public int Col {get; set;}
    public Location (int r, int c)
    {
        Row = r;
        Col = c;
    }
}