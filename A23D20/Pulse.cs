using System.Runtime.InteropServices.ComTypes;

namespace A23D20;

public class Pulse
{
    public List<Module> Modules { get; set; }
    
    public int Lows { get; set; }
    public int Highs { get; set; }
    public List<long> FoundHigh { get; set; }
    public string[] Keys { get; set; }

    public Pulse(List<string> lines, int push)
    {
        Modules = lines.Select(l => new Module(l)).ToList();
        Lows = 0;
        Highs = 0;
        Keys = new string[4] { "js", "zb", "bs", "rr" };
        FoundHigh = new List<long>();
        // find Conjunction and who are the input to the conjunction
        ConnectDestinations();
        SetupConjunctions();
        //  Connect to Destinations
        if (push == 2023)
        {
            var counter = 0;
            do
            {
                counter++;
                TriggerButton(counter);
            } while (FoundHigh.Count < 4);
            
        } else for (int i = 0; i < push; i++)
        {
            TriggerButton(i);
        }
    }

    private void TriggerButton(int buttonInput)
    {
        // send low to broadcaster
        var module = Modules.Find(m => m.Name == "broadcaster");
        module.Input.Enqueue(new DTM("button","low"));
        Lows++;
        NextRound(new List<Module>{module}, buttonInput);
    }

    private void NextRound(List<Module> modules, int buttonInput)
    {
        while (modules.Count != 0)
        {
            var nextRound = new List<Module>();
            var modulesList = modules.Select(m => m.Name).ToList();
            modules.ForEach(m =>
            {
                modulesList.RemoveAt(0);
                var result = m.Roll();
 
                nextRound.AddRange(result.Item1);
                Lows += result.Item2;
                Highs += result.Item3;
                // part 2
                if (Keys.Contains(m.Name) && result.Item3 == 1) 
                {
                    FoundHigh.Add(buttonInput);
                }
            });
            modules = nextRound;
        }
    }

    private void ConnectDestinations()
    {
        Modules.ForEach(m =>
        {
            foreach (var mo in Modules)
            {
                if (m.DestinationNames.Contains(mo.Name))
                {
                    m.Destinations.Add(mo);
                }
            }
        });
    }

    private void SetupConjunctions()
    {
        var conjunctions = Modules.Where(m => m.Type == "Conjunction").ToList();
        Modules.ForEach(m =>
        {
            var conjunctionsName = conjunctions.Select(c => c.Name).ToList();
            var desConj =m.Destinations.Where(d => d.Type == "Conjunction").ToList();
            if (desConj.Count != 0) desConj.ForEach(d =>
            {
                d.Memory.Add(m.Name, "low");
                d.SourceModules.Add(m.Name);
            });
        });
    }
}

public class Module
{
    public string Type { get; set; }
    
    public string Name { get; set; }
    
    public string Status { get; set; }

    public Queue<DTM> Input { get; set; }
    
    public Queue<DTM> Output { get; set; }

    public List<string> SourceModules { get; set; }
    
    public List<string> DestinationNames { get; set; }
    
    public List<Module> Destinations { get; set; }
    
    public Dictionary<string,string> Memory { get; set; }

    public Module(string line)
    {
        var splitArr = line.Split("->");
        var typeAndName = splitArr[0].Trim();
        if (typeAndName[0] == '%')
        {
            Type = "Flip-Flop";
            Status = "off";
            Name = typeAndName.Substring(1);
            // action rules
            // input high pulse , out pulse nothings
            // input low pulse, // to on output high pulse,
                                // to off output low pulse,
        } else if (typeAndName[0] == '&')
        {
            Type = "Conjunction";
            Name = typeAndName.Substring(1);
            // memory all most recent input, initialised to low
            // if all most recent inputs are high , then output low // otherwise high
            Memory = new Dictionary<string, string>();
        }
        if (typeAndName == "broadcaster")
        {
            Type = "broadcaster";
            Name = Type;
            // what ever it received then bounse out
        }
        
        DestinationNames = splitArr[1].Trim().Split(',').Select(item => item.Trim()).ToList();
        Destinations = new List<Module>();
        Input = new Queue<DTM>();
        Output = new Queue<DTM>();
        SourceModules = new List<string>();
        // PrintModule();
    }

    private void PrintModule()
    {
        Console.WriteLine(Type+" "+Name+" "+string.Join('-', DestinationNames));
    }

    public (List<Module>, int, int) Roll()
    {
        var lows = 0;
        var highs = 0;
        if (Type == "broadcaster")
        {
            while (Input.Count != 0)
            {
                var value = Input.Dequeue();
                Destinations.ForEach(m => m.Input.Enqueue(new DTM(Name,value.Pulse)));
                if (value.Pulse == "low") lows+= DestinationNames.Count;
                if (value.Pulse == "high") highs+= DestinationNames.Count;
            }

            return (Destinations, lows, highs) ;
        }

        if (Type == "Flip-Flop")
        {
            // action rules
            // input high pulse , out pulse nothings
            // input low pulse, // to on output high pulse,
            // to off output low pulse,
            var hasOutput = false;
            var name = Name;
            while (Input.Count != 0)
            {
                var value = Input.Dequeue();
                var output = "";
                if (value.Pulse == "low")
                {
                    Status = Status == "on" ? "off" : "on";
                    output = Status == "on" ? "high" : "low";
                    Destinations.ForEach(m => m.Input.Enqueue(new DTM(Name, output)));
                    if (output == "low") lows+=DestinationNames.Count;
                    if (output == "high") highs+=DestinationNames.Count;
                    hasOutput = true;
                }
            }
            return (hasOutput ? Destinations : new List<Module>(), lows, highs);


        }
        if (Type == "Conjunction")
        {
            // memory all most recent input, initialised to low
            // if all most recent inputs are high , then output low // otherwise high
            var name = Name;
            while (Input.Count != 0)
            {
                var value = Input.Dequeue();
                Memory[value.From] = value.Pulse;
            }

            var allHigh = Memory.All(m => m.Value == "high");
            var output = allHigh ? "low" : "high";
            Destinations.ForEach(m => m.Input.Enqueue(new DTM(Name, output)));
            
            if (output == "low") lows+= DestinationNames.Count;
            if (output == "high") highs+=DestinationNames.Count;

            return (Destinations, lows, highs);

        }

        return (new List<Module>(), 0, 0);
    }
}

public class DTM
{
    public string From { get; set; }
    public string Pulse { get; set; }

    public DTM(string from, string pulse)
    {
        From = from;
        Pulse = pulse;
    }
}

