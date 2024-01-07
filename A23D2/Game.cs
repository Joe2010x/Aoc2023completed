namespace A23D2;

public class Game
{
    public int Id { get; set; }
    
    public List<Set> Sets { get; set; }

    public Set MinSet { get; set; }

    public Game(string line)
    {
        var lineBreak = line.Split(':');
        var GameBreak = lineBreak[0].Split(' ');
        Id = int.Parse(GameBreak[1]);
        var SetBreak = lineBreak[1].Split(';');
        var noOfSets = SetBreak.Length;
        var sets = new List<Set>();
        for (int i = 0; i < noOfSets; i++)
        {
            var setStr = SetBreak[i];
            var colorBreak = setStr.Split(',');
            var green = 0;
            var red = 0;
            var blue = 0;
            foreach (var color in colorBreak)
            {
                
                var node = color.Trim().Split(' ');
                switch (node[1])
                {
                    case "red" : red = int.Parse(node[0]);
                        break;
                    case "green" : green = int.Parse(node[0]);
                        break;
                    case "blue" : blue = int.Parse(node[0]);
                        break;
                    
                }

                var newSet = new Set(red, green, blue);
                sets.Add(newSet);
            }
        }

        Sets = sets;

        var minSet = new Set(0, 0, 0);
        foreach (var s in sets)
        {
            minSet.Red = Math.Max(minSet.Red, s.Red);
            minSet.Blue = Math.Max(minSet.Blue, s.Blue);
            minSet.Green = Math.Max(minSet.Green, s.Green);
        }

        MinSet = minSet;
    }
}

public class Set
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }

    public Set(int red, int green, int blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }
}