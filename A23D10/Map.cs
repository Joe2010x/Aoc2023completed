using System.Diagnostics;
using System.Security.Cryptography;

namespace A23D10;

public class Map
{
    public PipeMap PipeMap { get; set; }
    
    public List<Sign> Signs { get; set; }
    
    public Pipe Start { get; set; }
    
    public List<List<Pipe>> Loops { get; set; }
    
    public List<Pipe> TheLoop { get; set; }
    public int Area { get; set; }
    
    public int InsideCube { get; set; }

    public Map(List<string> lines, List<string> linesSigns)
    {

        Signs = linesSigns.Select(s => new Sign(s)).ToList();
        var startSign = new Sign("S south and east");
        startSign.Ends.Add("north");
        startSign.Ends.Add("west");
        Signs.Add(startSign);
        PipeMap = new PipeMap(lines);
        Start = PipeMap.Pipes.Find(p => p.Letter == "S");
        CreateConnections();
        GetLoop();
        GetArea();
    }

    private void GetArea()
    {
        var first = TheLoop.First();
        var last = TheLoop.Last();
        var area = 0;
        for (int i = 1; i < TheLoop.Count; i++)
        {
            var prev = TheLoop[(i == 1 ? TheLoop.Count : i) - 1];
            var curr = TheLoop[i];
            var next = TheLoop[(i == (TheLoop.Count - 1) ? 0 : i) + 1];
            area += (curr.Col * (next.Row - prev.Row));
        }

        Area = Math.Abs(area / 2);
        InsideCube = Area - (TheLoop.Count - 1) / 2 + 1;
        
    }

    private void GetLoop()
    {
        var row = Start.Row;
        var col = Start.Col;
        var connections = new List<Pipe>();
        var up = row == 0 ? null : PipeMap.Pipes.Find(p => p.Row == row - 1 && p.Col == col);
        if (up != null) connections.Add(up);
        var down = row == PipeMap.Row ? null : PipeMap.Pipes.Find(p => p.Row == row + 1 && p.Col == col);
        if (down != null) connections.Add(down);
        var left = col == 0 ? null : PipeMap.Pipes.Find(p => p.Row == row && p.Col == col - 1);
        if (left != null) connections.Add(left);
        var right = col == PipeMap.Col ? null : PipeMap.Pipes.Find(p => p.Row == row && p.Col == col + 1);
        if (right != null) connections.Add(right);
        Loops = connections.Select(p => new List<Pipe> { new Pipe(row,col,"s"), p }).ToList();
        do
        {
            Step();
        
        } while (!SyncLoop());

        TheLoop = Loops[0];
        Console.WriteLine("Aoc 2023 day 10 part 1 " + (TheLoop.Count / 2));
    }

    private List<Pipe> FoundLoop(List<List<Pipe>> loops)
    {
        var counts = loops.Select(l => l.Count).ToList();
        var countOfCount = counts.Select(c => counts.Count(cc => c == cc)).ToList();
        var index = countOfCount.First(c => c == 2);
        return loops[index];
    }

    private bool SyncLoop()
    {
        var counter = 0;
        Loops.ForEach(loop =>
        {
            if (loop.Last().Col == Start.Col && Start.Row == loop.Last().Row)
            { 
                counter++;
            }
        });
        if (counter > 2)
        {
            return true;
        }
        return false;
    }

    private void PrintMapAfterFoundLoop()
    {
        var map = PipeMap.Pipes;
        Console.WriteLine();
        for (int i = 0; i < 140; i++)
        {
            for (int j = 0; j < 140; j++)
            {
                Console.Write(TheLoop.Exists(pipe => pipe.Row == i && pipe.Col == j) ? "*": ".");
            }
            Console.WriteLine();
        }
        
    }

    private bool HasLoop()
    {
        var result = false;
        Loops.ForEach(loop =>
        {
            if (loop.Last().Col == Start.Col && Start.Row == loop.Last().Row)
            {
                Console.WriteLine("found loop, length is " + loop.Count +" steps to fartherest "+Math.Round((double) loop.Count / 2));
                
                result = true;
            }
        });

        return result;
    }

    public void Step()
    {
        Loops.ForEach(loop =>
        {
            var last = loop.Last();
            var beforeLast = loop[loop.Count - 2];
            if (last.Letter != "." && last.Letter != "S")
            {
                var theOtherEnd = last.Connections.First(c => !(c.Row == beforeLast.Row && c.Col == beforeLast.Col));
                var nextPosition = theOtherEnd;
                loop.Add(nextPosition);
            }
            
        });
    }

    private void CreateConnections()
    {
        foreach (var pipe in PipeMap.Pipes)
        {
            if (pipe.Letter != "." && pipe.Letter != "S")
            {
                pipe.Sign = Signs.First(s => s.Letter == pipe.Letter);
                pipe.Connections = pipe.Sign.Ends.Select(direction => pipe.DirectionToPipe(direction, this)).ToList();
            }
        } 
    }
}

public class PipeMap
{
    public int Row { get; set; }
    public int Col { get; set; }
    
    public List<Pipe> Pipes { get; set; }

    public PipeMap(List<string> lines)
    {
        Row = lines.Count;
        Col = lines[0].Length;
        var pipes = new List<Pipe>();
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                var pipe = new Pipe(i, j, lines[i][j].ToString());
                pipes.Add(pipe);
            }
        }

        Pipes = pipes;
    }
}

public class Sign
{
    public string Letter { get; set; }
    public List<string> Ends { get; set; }
    public Sign(string s)
    {
        var split = s.Split(' ');
        Letter = split[0];
        var ends = new List<string>();
        ends.Add(split[1]);
        ends.Add(split[3]);
        Ends = ends;
    }
}

public class Pipe
{
    public int Row { get; set; }
    public int Col { get; set; }
    public Sign Sign { get; set; }
    public string Letter { get; set; }
    
    public List<Pipe?> Connections { get; set; }
    public Pipe(int row, int col, string sign)
    {
        Row = row;
        Col = col;
        Letter = sign;
    }

    public Pipe DirectionToPipe(string direction, Map map)
    {
        switch (direction)
        {
            case "north":
                return Row == 0 ? null:  map.PipeMap.Pipes.Find(p => p.Row == Row - 1 && p.Col == Col);
                break;
            case "south":
                return Row == map.PipeMap.Row ? null:  map.PipeMap.Pipes.Find(p => p.Row == Row + 1 && p.Col == Col);
                break;
            case "west":
                return Col == 0 ? null:  map.PipeMap.Pipes.Find(p => p.Row == Row && p.Col == Col - 1);
                break;
            case "east":
                return Col == map.PipeMap.Col ? null:  map.PipeMap.Pipes.Find(p => p.Row == Row && p.Col == Col + 1);
                break;
        }

        return null;
    }
}