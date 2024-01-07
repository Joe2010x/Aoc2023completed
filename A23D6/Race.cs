namespace A23D6;

public class Race
{
    public long Time { get; set; }
    public long Distance { get; set; }

    public bool Win(long time)
    {
        var speed = time ;
        var timeRemains = Time - time;
        var travelledDistance = speed * timeRemains;
        return travelledDistance > Distance;
    }

    public long FirstWin()
    {
        var counter = 0;
        do
        {
            counter++;
        } while (!Win(counter));

        return counter;
    }

    public long LastWin()
    {
        var counter = Time;
        do
        {
            counter--;
        } while (!Win(counter));

        return counter;
    }

    public int PossibleWins()
    {
        return (int)(LastWin() - FirstWin() + 1);
    }

    public Race(long time, long distance)
    {
        Time = time;
        Distance = distance;
    }
}

public class Game
{
    public List<Race> Races { get; set; }
    
    public Race Race2 { get; set; }

    public Game(List<string> lines)
    {
        var times = lines[0].Split(':')[1].Split(' ').Where(s => s != "").Select(s=>long.Parse(s)).ToList();
        var time2 = long.Parse(lines[0].Split(':')[1].Replace(" ", ""));
        var distances = lines[1].Split(':')[1].Split(' ').Where(s => s != "").Select(s=>long.Parse(s)).ToList();
        var distance2 = long.Parse(lines[1].Split(':')[1].Replace(" ", ""));
        var races = new List<Race>();
        for (int i = 0; i < times.Count; i++)
        {
            var race = new Race(times[i], distances[i]);
            races.Add(race);
        }

        Races = races;
        Race2 = new Race(time2, distance2);
    }
}