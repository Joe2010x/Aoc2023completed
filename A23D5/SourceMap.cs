namespace A23D5;

public class SourceMap
{
    public List<long> Seeds { get; set; }
    
    public List<long> Seeds1 { get; set; } 
    
    public List<SeedsRange> SeedsRanges { get; set; }
    public List<SourceToDestination> SourceToDestinations { get; set; }

    public long ToFinalDestinationa(long soource)
    {
        var s = soource;
        foreach (var s2s in SourceToDestinations)
        {
            s = s2s.FromSourceToDestination(s);
        }

        return s;
    }

    public SourceMap(List<string> lines)
    {
        var indexOfBreaks = Enumerable.Range(0, lines.Count)
            .Where(i => lines[i] == "")
            .ToList();
        var seedsLine = lines[0];
        // seeds generating
        Seeds1 = seedsLine.Split(':')[1].Split(' ').Where(s => s != "").Select(s => long.Parse(s)).ToList();
        var seeds = new List<long>();
        // seeds Range Generation
        var longList = seedsLine.Split(':')[1].Split(' ').Where(s => s != "").Select(s => long.Parse(s)).ToList();

        var seedsRanges = new List<SeedsRange>();
        
        for (int i = 0; i < longList.Count/2; i++)
        {
            var seedFrom = longList[i * 2];
            var seedNumber = longList[i * 2 + 1];
            var seedsRange = new SeedsRange(seedFrom, seedNumber);
            seedsRanges.Add(seedsRange);
            seeds.AddRange(seedsRange.Seeds);
        }
        SeedsRanges = seedsRanges;
        Seeds = seeds;
        
        var sourceToDestination = new List<SourceToDestination>();
        for (int i = 0; i < indexOfBreaks.Count; i++)
        {
            var nextLines = lines.GetRange(indexOfBreaks[i] + 1,
                (i != indexOfBreaks.Count - 1 ? indexOfBreaks[i + 1] : lines.Count) - indexOfBreaks[i] - 1);
            var s2s = new SourceToDestination(nextLines[0], nextLines.GetRange(1, nextLines.Count-1));
            sourceToDestination.Add(s2s);
        }
        SourceToDestinations = sourceToDestination;
    }
}

public class SeedsRange
{
    public long SeedFrom { get; set; }
    public long Number { get; set; }
    
    public List<long> Seeds { get; set; }

    public SeedsRange(long seedFrom, long number)
    {
        SeedFrom = seedFrom;
        Number = number;
        var seeds = new List<long>();
        for (long i = seedFrom; i < seedFrom + number; i++)
        {;
            seeds.Add(i);
        }
        Seeds = seeds;
    }
}

public class SourceToDestination
{
    public string Source { get; set; }
    public string Destination { get; set; }
    public List<Map> Maps { get; set; }

    public long FromSourceToDestination(long source)
    {
        long result = -100;
        foreach (var map in Maps)
        {
            var value = map.GetDestination(source) ;
            result = (long) (value != null ? value : (long) -100);
            if (result != -100) return result;
        }

        return result != -100 ? result : source;
    }
    public SourceToDestination(string title, List<string> lines)
    {
        var titleStr = title.Split(' ')[0];
        var nameList = titleStr.Split('-');
        Source = nameList[0];
        Destination =nameList[2];
        Maps = lines.Select(l => new Map(l)).ToList();
    }

    public void Print()
    {
        var str = "Source: " + Source + " Destination: " + Destination + " num of Maps: " + Maps.Count;
        Maps.ForEach(m=> m.Print());
        Console.WriteLine(str);
    }
}

public class Map
{
    public long SourceFrom { get; set; }
    public long DestinationaFrom { get; set; }
    public long SourceTo { get; set; }
    public long DestinationTo { get; set; }
    public long Numbers { get; set; }

    public Map(string line)
    {
        var values = line.Split(' ').Where(v => v != "").Select(v=> long.Parse(v)).ToList();
        DestinationaFrom = values[0];
        SourceFrom = values[1];
        Numbers = values[2];
        DestinationTo = DestinationaFrom + Numbers - 1;
        SourceTo = SourceFrom + Numbers - 1;
    }

    public void Print()
    {
        Console.WriteLine("Destination: "+DestinationaFrom+" Source: "+SourceFrom+" Counts "+Numbers);
    }

    public long? GetDestination(long source)
    {
        if (HasSource(source))
        {
            var distance = source - SourceFrom;
            return DestinationaFrom + distance;
        }
        return null;
    }

    private bool HasSource(long source)
    {
        if (source >= SourceFrom && source <= SourceTo) return true;
        return false;
    }
}