using System.Data;

namespace A23D11;

public class Universe
{
    public int Row {get; set;}
    public int Col {get; set;}
    public List<Galaxy> Galaxies {get; set;}
    public Char[,] Map {get; set;}
    public List<List<int>> DistanceMatrix {get; set;}
    public long TotalDistance {get;set;}
    public void CalculateDistance()
    {
        for (int i = 0; i < Galaxies.Count; i++)
        {
            var distance = new List<long>();
            for (int j = 0; j < Galaxies.Count; j++)
            {
                var d = Math.Abs(Galaxies[j].Row - Galaxies[i].Row) + Math.Abs(Galaxies[j].Col - Galaxies[i].Col);
                distance.Add(d);
            }
            TotalDistance = TotalDistance + distance.Sum();
        }
    }
    public void Expand (int times)
    {
        times --;
        var rows = new List<char[]>();
        var cols = new List<char[]>();
        for (int col = 0; col < Col; col++)
        {
            var newCol = new char[Row];
            cols.Add(newCol);
        }
        for (int row = 0; row < Row; row++)
        {
            var newRow = new char[Col];
            for (int col = 0; col < Col; col++)
            {
                newRow[col] = Map[row,col];
                cols[col][row] = Map[row,col];
            }
            rows.Add(newRow);
        }
        var emptyRows = EmptyLines(rows);
        var emptyCols = EmptyLines(cols);
        UpdateGalaxies(emptyRows, emptyCols, times);
    }
    public void UpdateGalaxies(int[] emptyRows, int[] emptyCols, int times)
    {
        var newGalaxies = new List<Galaxy>();
        foreach (var galaxy in Galaxies)
        {
            var galaxy1 = galaxy;
            var newRow = emptyRows.Count(r => galaxy1.Row > r) * times + galaxy.Row;
            
            var newCol = emptyCols.Count(r => galaxy.Col > r) * times + galaxy.Col;
            var newGalaxy = new Galaxy(newRow, newCol);
            newGalaxies.Add(newGalaxy);
        }

        Galaxies = newGalaxies;
    }
    public int[] EmptyLines (List<char[]> lines)
    {
        var numOfLines = new List<int>();
        for (int num = 0; num < lines.Count; num++)
        {
            var thisLine = lines[num];
            if (thisLine.Any(c => c != '.')) continue;
            numOfLines.Add(num);
        }
        return numOfLines.ToArray();
    }
    public void PrintUniverse ()
    {
        for (int row = 0; row < Row; row++)
        {
            Console.WriteLine();
            for (int col = 0; col < Col; col++)
            {
                Console.Write(Map[row,col]);
            }
        }
        Console.WriteLine();
    }
    public List<Galaxy> ReadGalaxies(char[,] lines)
    {
        // Console.WriteLine("read Galaxy starts");
        var galaxies = new List<Galaxy> ();
        for (int row = 0; row < Row; row++)
        {
            for (int col = 0; col < Col; col++)
            {
                var point = lines[row, col];
                if (point == '#') {
                    galaxies.Add(new Galaxy(row, col));
                }
            }
        }
        return galaxies;
    }
    public Char[,] ReadMap(List<string> lines)
    {
        var galaxies = new List<Galaxy> ();
        var map = new Char[Row,Col];
        for (int row = 0; row < Row; row++)
        {
            for (int col = 0; col < Col; col++)
            {
                map[row,col] = lines[row][col];
            }
        }
        return map;
    }
    public Universe (List<string> lines)
    {
        Row = lines.Count;
        Col = lines[0].Length;
        Map = ReadMap(lines);
        Galaxies = ReadGalaxies(Map);
    }
}
public class Galaxy
{
    public int Row {get; set;}
    public int Col {get; set;}
    public Galaxy (int row, int col)
    {
        Row = row;
        Col = col;
    }
}
public class AddedLine
{
    public int Start {get; set;}
    public int End {get; set;}
}