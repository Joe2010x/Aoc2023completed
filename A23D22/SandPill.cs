namespace A23D22;

public class SlabPill 
{
    public List<Slab> Slabs {get;set;}

    public int NumberOfMoveable {get; set;}

    public int TotalFallable {get;set;}

    public SlabPill (List<string> lines)
    {
        var length = lines.Count;
        Slabs = new List<Slab>();
        NumberOfMoveable = 0;
        for (int i = 0; i < length; i++)
        {
            Slabs.Add(new Slab(lines[i],i));
        }
        // Console.WriteLine(string.Join(',',Slabs.Select(s=> s.Z.First()).ToArray()));
        Slabs = Slabs.OrderBy(s => s.Z.First()).ToList();
        Drop();
        // Console.WriteLine(string.Join(',',Slabs.Select(s=> s.Z.First()).ToArray()));

        Slabs = Slabs.OrderBy(s => s.Z.First()).ToList();

        SupportsAndLays();
        
        NumberOfMoveable = Slabs.Count(s=> {
            s.CheckMoveable();
            return s.Moveable;
            });
        
        // Slabs.ForEach(s => {
        //     TotalFallable += s.CountFalls(new List<Slab>{s}); 
        // });

        Slabs.ForEach(s => {
            s.Fallable = s.CountFalls(this);
        } );
        TotalFallable = Slabs.Sum(s=> s.Fallable);
        
   
    }
    void SupportsAndLays()
    {
        for (int i = 1; i < Slabs.Count; i++)
        {
            var slabA = Slabs[i];
            var newZ = 1;
            for (int j = 0; j < i; j++)
            {
                var slabB = Slabs[j];

                var intZ = slabB.Z.Contains( slabA.Z.First()-1);
                var intY = slabB.Y.Intersect(slabA.Y);
                var intX = slabB.X.Intersect(slabA.X);
                if (intZ && intY.Any() && intX.Any() ) 
                {
                    slabA.LaysOn.Add(slabB);
                    slabB.Supports.Add(slabA);
                }
            }
        }
    }
    void Drop()
    {
        for (int i = 1; i < Slabs.Count; i++)
        {
            var slabA = Slabs[i];
            var newZ = 1;
            for (int j = 0; j < i; j++)
            {
                var slabB = Slabs[j];
                var intZ = slabB.Z.Contains( slabA.Z.First()-1);
                var intY = slabB.Y.Intersect(slabA.Y);
                var intX = slabB.X.Intersect(slabA.X);
                // slabA.PrintSlab();
                // slabB.PrintSlab();
                // Console.WriteLine(intZ+" "+intY.Any()+" "+intX.Any());
                if (intY.Any() && intX.Any() ) {
                    newZ = Math.Max(newZ, slabB.Z.Max() + 1);
                }
            }
            var zDiff = slabA.Z[0] - newZ;
            slabA.Z = slabA.Z.Select(z=> z-zDiff).ToList();
        }
    }
}

public class Slab
{
    public int Index {get; set;}
    public List<int> X {get; set;}
    public List<int> Y {get; set;}
    public List<int> Z {get; set;}

    public List<Slab> Supports {get; set;}

    public List<Slab> LaysOn {get;set;}

    public bool Moveable {get;set;}

    public int Fallable {get;set;}

    public Slab (string line, int index) 
    {
        Index = index;
        Fallable = 0;
        var split = line.Split('~');
        var splitFirst = split[0].Split(',');
        var splitSecond = split[1].Split(',');
        Supports = new List<Slab>();
        LaysOn = new List<Slab>();
        X = Enumerable.Range(int.Parse(splitFirst[0].ToString())
        ,(int.Parse(splitSecond[0].ToString()) - int.Parse(splitFirst[0].ToString())  + 1)).ToList();
        Y = Enumerable.Range(int.Parse(splitFirst[1].ToString())
        ,(int.Parse(splitSecond[1].ToString()) - int.Parse(splitFirst[1].ToString()) + 1)).ToList();
        Z = Enumerable.Range(int.Parse(splitFirst[2].ToString())
        ,(int.Parse(splitSecond[2].ToString()) - int.Parse(splitFirst[2].ToString()) + 1)).ToList();
        // PrintSlab();
    }
    public void PrintSlab()
    {
        Console.WriteLine(" from ( "+X[0]+","+Y[0]+","+Z[0]+" )");
        Console.WriteLine("  To  ( "+X.Last()+","+Y[Y.Count-1]+","+Z.Last()+" )");
    }

    public void CheckMoveable()
    {
        Moveable = Supports.Count == 0 || Supports.All(s => s.LaysOn.Count > 1);
    }
    public int CountFalls(SlabPill sp)
    {
        if (Supports.Count == 0) return 0;
        var next = Supports;
        var collections = new List<Slab>{this};
        var result = 0;
        var falling = 0;
        do
        {
            falling = 0;
            var idCollection = collections.Select(c => c.Index).ToArray();
            var sps = sp.Slabs.Where(s => !idCollection
                .Contains(s.Index)).ToList() ;
            foreach (var s in sps )
            {
                if (s.LaysOn.Count != 0 && s.LaysOn.Where(sl => !idCollection.Contains(sl.Index)).Count() == 0)
                {
                    falling ++;
                    collections.Add(s);
                }
            }
            result += falling;
        } while (falling > 0);
        return result;
        
    }
}