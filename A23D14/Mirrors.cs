
namespace A23D14;
public class Mirrors
{
    public int Row {get; set;}
    public int Col {get; set;}
    public List<char[]> Position {get; set;}
    public List<List<char[]>> PositionCollections { get; set; }
    public int CircleStart { get; set; }
    public int CircleEnd { get; set; }
    public bool IsStable { get; set; } 

    public Mirrors(List<string> lines)
    {
        Row = lines.Count;
        Col = lines[0].Length;
        Position =  Enumerable.Range(0, Col)
            .Select(col => Enumerable.Range(0, Row)
                .Select(row => lines[col][row])
                .ToArray())
            .ToList();
        PositionCollections = new List<List<char[]>> { Copy(Position) };
        IsStable = false;
        DiscoverCircles();
    }

    private void DiscoverCircles()
    {
        do
        {
            RollCircle();
            (IsStable, CircleStart, CircleEnd) = IsStablePositions(Position, PositionCollections);
            PositionCollections.Add(Copy(Position));
        } while (!IsStable);
    }

    private (bool,int,int) IsStablePositions(List<char[]> position, List<List<char[]>> positionCollections)
    {
        for (int i = 0; i < positionCollections.Count; i++)
        {
            if (IsStablePosition(position, positionCollections[i])) return (true, i, positionCollections.Count);
        }
        return (false, 0, 0);
    }

    private bool IsStablePosition(List<char[]> position, List<char[]> targetPosition)
    {
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Col; c++)
            {
                if (position[r][c] != targetPosition[r][c]) return false;
            }
        }
        Console.WriteLine("stable reached. ");
        return true;
    }

    private List<char[]> Copy(List<char[]> position)
    {
        var newPosition = new List<char[]>();
        for (int r = 0; r < Row; r++)
        {
            var row = new char[Col];
            for (int c = 0; c < Col; c++)
            {
                row[c] = position[r][c];
            }
            newPosition.Add(row);
        }
        return newPosition;
    }

    private void RollCircle()
    {
        RollDirection("north");
        RollDirection("west");
        RollDirection("south");
        RollDirection("east");
    }

    private int Calculate(List<char[]> position)
    {
        var value = 0;
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Col; c++)
            {
                var distanceToNorth = Row - r;
                if (position[r][c] == 'O') value += distanceToNorth;
            }
        }

        return value;
    }

    private void PrintMap(List<char[]> position)
    {
            for (int row = 0; row < Row; row++)
            {
                Console.WriteLine();
                for (int col = 0; col < Col; col++)
                {
                    Console.Write( position[row][col] );
                }
            }
            Console.WriteLine();
    }

    private List<char[]> RollNorth(List<char[]> position)
    {
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Col; c++)
            {
                position = Roll(r,c,position, "north");
            }
        }

        return position;
    }

    private void RollDirection(string direction)
    {
        switch (direction)
        {
            case "north":
            {
                for (int r = 0; r < Row; r++)
                {
                    for (int c = 0; c < Col; c++)
                    {
                        Position = Roll(r,c,Position, direction);
                    }
                }
                break;
            }
            case "south":
            {
                for (int r = Row - 1; r >= 0; r--)
                {
                    for (int c = 0; c < Col; c++)
                    {
                        Position = Roll(r,c,Position, direction);
                    }
                }
                break;
            }
            case "west":
            {
                for (int c = 0; c < Col; c++)
                {
                    for (int r = 0; r < Row; r++)
                    {
                        Position = Roll(r,c,Position, direction);
                    }
                }
                break;
            }
            case "east":
            {
                for (int c = Col - 1; c >=0 ; c--)
                {
                    for (int r = 0; r < Row; r++)
                    {
                        Position = Roll(r,c,Position, direction);
                    }
                }
                break;
            }
        }
    }

    private List<char[]> Roll(int r, int c, List<char[]> position, string direction)
    {
        var point = position[r][c];
        if (point != 'O') return position;
        var nextPosition = 'x';
        var nextR = -1;
        var nextC = -1;
        switch (direction)
        {
            case "north":
            {
                nextR = r != 0 ? r - 1 : r;
                nextC = c;
                nextPosition = r != 0 ? position[r - 1][c] : 'x';
                
                break;
            }
            case "south":
            {
                
                nextR = r != Row -1 ? r + 1 : r;
                nextC = c;
                nextPosition = r != Row - 1 ? position[r + 1][c] : 'x';
                break;
            }
            case "west":
            {
                
                nextR = r;
                nextC =  c != 0 ? c-1: c ;
                nextPosition = c != 0 ? position[r][c-1] : 'x';
                break;
            }
            case "east":
            { 
                
                nextR = r;
                nextC =  c != Col -1 ? c + 1: c ;
                nextPosition = c != Col - 1 ? position[r][c+1] : 'x';
                break;
            }
            default: nextPosition = 'x';
                break;
        }
        if (nextPosition != '.') return position;
        position[nextR][nextC] = position[r][c];
        position[r][c] = '.';
        return Roll(nextR, nextC, position, direction);
    }

    public int GetValue(int target)
    {
        if (target == 1)
        {
            var p = RollNorth(PositionCollections[0]);
            // PrintMap(p);
            return Calculate(p);
        } 
        var circle = CircleEnd - CircleStart;
        var convertedCircleValue = (Math.Max(target , 0)) < CircleStart ? Math.Max(target , 0) :  ((target - CircleStart) % circle + CircleStart);
        return Calculate(PositionCollections[convertedCircleValue]);
    }
}