namespace A23D16;

public class Map
{
    public List<List<Point>> Points {get; set;}

    public List<Rull> RullBook{get;set;}

    public int Row {get; set;}
    public int Col {get; set;}

    public Map (List<string> lines)
    {
        Row = lines.Count;
        Col = lines[0].Length;
        Points = WriteToPointsMap(lines,Row,Col);
        RullBook = CreateRullBook();
    }

    public int CalculateWithStartingPosition(int startRow, int startCol, int direction )
    {
        Move(startRow,startCol,1);
        return Calculate("Energized");
    }

    public int CalculateStartFromAllEdges()
    {
          var maxEnerizes = 0;
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Col; c++)
            {
                if (r == 0) Move (r,c,2);
                if (r == Row - 1) Move (r,c,4);
                if (c == 0) Move (r,c,1);
                if (c == Col - 1) Move (r,c,3);
                maxEnerizes = Math.Max(maxEnerizes, Calculate("Energized"));
                Clean();
            }
        }
        return maxEnerizes;
    }

    public void Clean()
    {
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Col; c++)
            {
                Points[r][c].IsEnergized = false;
                Points[r][c].Ins.Clear();
            }
        }
    }
    public int Calculate(string name){
        var counter = 0;
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Col; c++)
            {
                counter += Points[r][c].IsEnergized ? 1 : 0;
            }
        }
        return counter;
    }
    public void Move(int r, int c, int direction)
    {
        var current = Points[r][c];
        var sign = current.Symble;
        var rull = RullBook.Find(r => r.Symble == sign);
        current.IsEnergized = true;
        var newDirections = rull.GetExitDirection(direction);
        if (!current.Ins.Contains(direction))
        {
            current.Ins.Add(direction);
            newDirections.ForEach(d => {
                if (d == 1 && c != 0 ) 
                {
                    Move(r,c-1, 3);
                }
                else
                if (d == 2 && r != 0 ) 
                {
                    Move(r-1,c, 4);
                }
                else
                if (d == 3 && c!= Col-1 ) 
                {
                    Move(r,c+1, 1);
                }
                else 
                if (d == 4 && r != Row-1 ) 
                {
                    Move(r+1,c, 2);
                }
            });
        }
    }
    public List<Rull> CreateRullBook()
    {
        var rullBook = new List<Rull>();
        var exit1 = new List<int>{1};
        var exit2 = new List<int>{2};
        var exit3 = new List<int>{3};
        var exit4 = new List<int>{4};
        var exit13 = new List<int>{1,3};
        var exit24 = new List<int>{2,4};
        var exitSetDot = new List<List<int>> {exit3, exit4, exit1, exit2};
        rullBook.Add(new Rull('.',new List<int> {1,2,3,4}, exitSetDot));
        rullBook.Add(new Rull('|',new List<int> {1,2,3,4}, new List<List<int>> {exit24,exit4,exit24,exit2}));
        rullBook.Add(new Rull('-',new List<int> {1,2,3,4}, new List<List<int>> {exit3,exit13,exit1,exit13}));
        rullBook.Add(new Rull('/',new List<int> {1,2,3,4}, new List<List<int>> {exit2,exit1,exit4,exit3}));
        rullBook.Add(new Rull('\\',new List<int> {1,2,3,4}, new List<List<int>> {exit4,exit3,exit2,exit1}));
        return rullBook;
    }

    public List<List<Point>> WriteToPointsMap(List<string> lines, int row, int col)
    {
        var result = new List<List<Point>>();
        for (int r = 0; r < row; r++)
        {
            var newRow = new List<Point>();
            for (int c = 0; c < col; c++)
            {
                var point = new Point(lines[r][c],r,c);
                newRow.Add(point);
            }
            result.Add(newRow);
        }
        return result;
    }
}

public class Point 
{
    public int Symble {get; set;}

    public int Row {get;set;}
    public int Col {get;set;}
    public bool IsEnergized {get;set;}
    public List<int> Ins {get; set;}
    public Point(char symble,int row, int col)
    {
        Symble = symble;
        Row = row;
        Col = col;
        IsEnergized = false;
        Ins = new List<int>();
    }
}

public class Rull 
{
    public char Symble {get; set;}
    public List<int> Entry {get;set;}
    public List<List<int>> Exit {get;set;}
    public Rull(char symble, List<int> entry, List<List<int>> exit)
    {
        Symble = symble;
        Entry = entry;
        Exit = exit;
    }

    public List<int> GetExitDirection (int entry )
    {
        var index = Entry.IndexOf(entry);
        return Exit[index];
    }
}
