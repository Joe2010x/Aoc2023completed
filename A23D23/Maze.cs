namespace A23D23;
public class Maze
{
    public int Row { get; set; }
    public int Col { get; set; }
    
    public Dictionary<Position, HashSet<Position>> graph1;
    public Dictionary<Position, HashSet<Position>> graph2;

    public Maze(List<string> lines)
    {
        Row = lines.Count;
        Col = lines[0].Length;
        graph1 = BuildGraph(lines, 1);
        graph2 = BuildGraph(lines, 2);
    }

    public int FindLongestPath(HashSet<Position> visited, Position current, Position end, Dictionary<Position, HashSet<Position>> graph)
    {
        var result = 0;

        if (current == end)
        {
            // Console.WriteLine("reached!");
            return visited.Count;
        }

        // var val = graph.First(g => g.Key.Row == current.Row && g.Key.Col == current.Col).Value;
        // foreach (var neighbor in val)
        foreach (var neighbor in graph[current])
        {
            if (!visited.Add(neighbor))
            {
                continue;
            }

            var length = FindLongestPath(visited, neighbor, end, graph);
            result = Math.Max(result, length);
            visited.Remove(neighbor);
        }

        return result;
    }

    public Dictionary<Position, HashSet<Position>> BuildGraph(List<string> lines, int indicator)
    {
        var graph = new Dictionary<Position, HashSet<Position>>();

        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];

            for (var col = 0; col < line.Length; col++)
            {
                if (line[col] == '#')
                {
                    continue;
                }
                
                var pos = new Position(row, col);
                graph[pos] = new HashSet<Position>();
                
                if (indicator == 1)
                {
                    switch (line[col])
                    {
                        case '>':
                            graph[pos].Add(pos.Move(Direction.Right));
                            continue;
                        case 'v':
                            graph[pos].Add(pos.Move(Direction.Down));
                            continue;
                        case '<':
                            graph[pos].Add(pos.Move(Direction.Left));
                            continue;
                    }
                }


                if (row > 0 && lines[row - 1][col] != '#')
                {
                    graph[pos].Add(pos.Move(Direction.Up));
                }

                if (row < lines.Count - 1 && lines[row + 1][col] != '#')
                {
                    graph[pos].Add(pos.Move(Direction.Down));
                }

                if (col > 0 && lines[row][col - 1] != '#')
                {
                    graph[pos].Add(pos.Move(Direction.Left));
                }

                if (col < line.Length - 1 && lines[row][col + 1] != '#')
                {
                    graph[pos].Add(pos.Move(Direction.Right));
                }
            }
        }
        // Console.WriteLine(graph.Count());
        // Console.WriteLine(graph.Select(e=>e.Value.Count()).Sum());
        // Console.Read();
        return graph;
    }
}

public record Direction(int Row, int Col)
{

    public static Direction Up = new(-1, 0);
    public static Direction Down = new(1, 0);
    public static Direction Left = new(0, -1);
    public static Direction Right = new(0, 1);
}

public record Position(int Row, int Col)
{
    public Position Move(Direction dir)
    {
        return new Position(Row + dir.Row, Col + dir.Col);
    }
}