namespace A23D25;

public class Board
{
    public int NoOfNodes { get; set; }
    public int[,] Conn { get; set; }
    public List<string> Name { get; set; }
    
    public int Result { get; set; }
    
    public Board(List<string> lines)
    {
        Dictionary<string, int> ind = new Dictionary<string, int>();
        Name = new List<string>();
        int index = 0;
        foreach (string line in lines)
        {
            string nodename = line.Substring(0, 3);
            if (!ind.ContainsKey(nodename))
            {
                ind[nodename] = index;
                index++;
                Name.Add(nodename);
            }
            string[] wds = line.Substring(5).Split(' ');
            foreach (string wd in wds)
            {
                if (!ind.ContainsKey(wd))
                {
                    ind[wd] = index;
                    index++;
                    Name.Add(wd);
                }
            }
        }
        NoOfNodes = index;
        
        Conn = new int[NoOfNodes, NoOfNodes];
        foreach (string line in lines)
        {
            int row = ind[line.Substring(0, 3)];
            string[] wds = line.Substring(5).Split(' ');
            for (int i = 0; i < wds.Length; i++)
            {
                int col = ind[wds[i]];
                Conn[col, row] = 1;
                Conn[row, col] = 1;
            }
        }
        
        Solve();
    }

    public void Solve()
    {
        
        for (int j = 1; j < NoOfNodes; j++)
        {
            int flow = MaxFlow(0, j);
            if (flow <= 3)
            {
               break;
            }
        }
    }
    int FindPath(int[,] flow, int start, int end)
    {
        int[] from = new int[NoOfNodes];
        for (int i = 0; i < NoOfNodes; i++)
        {
            from[i] = -1;
        }
        from[start] = 0;
        int steps = 0;
        List<int> todo = new List<int>();
        todo.Add(start);
        while (todo.Count > 0 && from[end] == -1)
        {
            int curr = todo[0];
            steps++;
            todo.RemoveAt(0);
            for (int j = 0; j < NoOfNodes; j++)
            {
                if (Conn[curr, j] - flow[curr, j] > 0 && from[j] == -1)
                {
                    from[j] = curr;
                    todo.Add(j);
                }
            }
        }
        if (from[end] >= 0)
        {
            int i = end;
            while (i != start)
            {
                flow[from[i], i]++;
                flow[i, from[i]]--;
                i = from[i];
            }
            return 0;
        }
        return steps;
    }

    int MaxFlow(int start, int end)
    {
        int[,] flow = new int[NoOfNodes, NoOfNodes];
        int flowVal = 0;
        while (true)
        {
            int compSize = FindPath(flow, start, end);
            if (compSize == 0)
            {
                flowVal++;
            }
            else
            {
                if (flowVal == 3)
                { 
                    Result = (compSize * (NoOfNodes - compSize));
                }
                break;
            }
        }
        return flowVal;
    }
}