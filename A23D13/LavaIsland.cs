namespace A23D11;

public class LavaIsland
{
    public List<Map> Maps {get;set;}

    public LavaIsland (List<string> lines)
    {
        
        Maps = new List<Map>();
        List<string> lineBuilder = new List<string>();
        lines.Add("");

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                Maps.Add(new Map(lineBuilder));
                lineBuilder.Clear();
            }
            else
            {
                lineBuilder.Add(line);
            }
        }
    }
}

public class Map 
{
    public char[,] Position {get; set;}

    public int Row {get; set;}

    public int Col {get; set;}
    
    public List<char[]> RowArr { get; set; }

    public List<char[]> ColArr { get; set; }
    
    public List<int> RowRelection { get; set; }
    
    public List<int> ColRelection { get; set; }
    
    public List<int> NewRowRelection { get; set; }
    
    public List<int> NewColRelection { get; set; }
    
    public int Value { get; set; }
    
    
    public int ChangedValue { get; set; }

    public Map (List<string> lines)
    {
        Row = lines.Count;
        Col = lines[0].Length;
        Position = new char[Row,Col];

        var rowArr = lines.Select(line => line.ToArray()).ToList();

        var colArr = Enumerable.Range(0, Col)
            .Select(col => Enumerable.Range(0, Row)
                .Select(row => rowArr[row][col])
                .ToArray())
            .ToList();
        
        RowArr = rowArr;
        ColArr = colArr;

        (RowRelection, ColRelection) =  FindReflection(0);
        
        (NewRowRelection, NewColRelection) =  FindReflection(1);

        Calculation();

    }

    private void Calculation()
    {
        Value = RowRelection.Aggregate(0, (a, b) => a + b) * 100 + 1 * ColRelection.Aggregate(0, (a, b) => a + b);
        ChangedValue = NewRowRelection.Aggregate(0, (a, b) => a + b) * 100 + 1 * NewColRelection.Aggregate(0, (a, b) => a + b);
    }
    
    private (List<int>, List<int>) FindReflection(int diff)
    {
        List<int> rowReflection = FindReflectionIndices(Row, RowArr, diff);
        List<int> colReflection = FindReflectionIndices(Col, ColArr, diff);

        return (rowReflection, colReflection);
    }
    
    private List<int> FindReflectionIndices(int dimension, List<char[]> arr, int diff)
    {
        var reflectionIndices = new List<int>();
        for (int i = 1; i < dimension; i++)
        {
            var counter = 0;
            var lineUpperThenI = i;
            var lineLowerThenI = dimension - i;

            for (int j = 0; j < Math.Min(lineLowerThenI, lineUpperThenI); j++)
            {
                var upperRow = arr[i - j - 1];
                var lowerRow = arr[i + j];

                counter += upperRow.Zip(lowerRow, (upper, lower) => upper != lower ? 1 : 0).Sum();

            }

            if (counter == diff)
            {
                reflectionIndices.Add(i);
            }
        }

        return reflectionIndices;
    }

    public void PrintMap()
    {
        for (int row = 0; row < Row; row++)
        {
            Console.WriteLine();
            for (int col = 0; col < Col; col++)
            {
                Console.Write( Position[row, col]);
            }
        }
        Console.WriteLine();
    }
}

