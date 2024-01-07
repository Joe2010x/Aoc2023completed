using System.Runtime.CompilerServices;

namespace A23D17;

public class LavaPuzzle
{ 
        public int Row { get; set; }
        public int Col { get; set; }
        public Position Start { get; set; }
        public int[][] Map { get; set; }
        
        public PriorityQueue<Path, int> Queue { get; set; }
        public HashSet<string> Visited { get; set; }

        public int TotalHeat { get; set; }
        public int TotalHeat2 { get; set; }
        
        public LavaPuzzle(List<string> lines)
        {
                Row = lines.Count;
                Col = lines[0].Length;
                Start = new Position(0, 0);
                TotalHeat = 0;
                TotalHeat2 = 0;
                Map = lines.Select(s => s.Select(c => int.Parse(c.ToString())).ToArray())
                        .ToArray();
                
                Queue = new PriorityQueue<Path, int>();
                Queue.Enqueue(new Path(Start, Direction.Right, 0), 0);
                Visited = new HashSet<string>();
                Move();
                
                Queue.Clear();
                Visited.Clear();
                Queue.Enqueue(new Path(Start, Direction.Right, 0), 0);
                Queue.Enqueue(new Path(Start, Direction.Down, 0), 0);
                Move2();
        }

        private void Move2()
        {
                while (Queue.Count > 0)
                {
                        var path = Queue.Dequeue();
                        if (path.Position.Row == Map.Length - 1 && path.Position.Col == Map[0].Length - 1 &&
                            path.StraightLineLength >= 4)
                        {
                                TotalHeat2 = path.Heat;
                                break;
                        }

                        if (path.StraightLineLength < 10)
                        {
                                TryMove(path, path.Direction);
                        }

                        if (path.StraightLineLength >= 4)
                        {
                                TryMove(path, path.Direction.TurnLeft());
                                TryMove(path, path.Direction.TurnRight());
                        }
                }
        }

        private void Move()
        {
                while (Queue.Count > 0)
                {
                        var path = Queue.Dequeue();

                        if (path.Position.Row == Map.Length - 1 && path.Position.Col == Map[0].Length - 1)
                        {
                                TotalHeat = path.Heat;
                                break;
                        }

                        if (path.StraightLineLength < 3)
                        {
                                TryMove(path, path.Direction);
                        }

                        TryMove(path, path.Direction.TurnLeft());
                        TryMove(path, path.Direction.TurnRight());
                }
                
        }
        void TryMove(Path path, Direction direction)
        {
                var candidate = new Path(path.Position.Move(direction), direction,
                        direction == path.Direction ? path.StraightLineLength + 1 : 1);

                if (candidate.Position.Row < 0 || candidate.Position.Row >= Map.Length ||
                    candidate.Position.Col < 0 || candidate.Position.Col >= Map[0].Length)
                {
                        return;
                }

                var key =
                        $"{candidate.Position.Row},{candidate.Position.Col},{candidate.Direction.Row},{candidate.Direction.Col},{candidate.StraightLineLength}";
                if (Visited.Contains(key))
                {
                        return;
                }

                Visited.Add(key);

                candidate.Heat = path.Heat + Map[candidate.Position.Row][candidate.Position.Col];
                Queue.Enqueue(candidate, candidate.Heat);
        }
        
        
}

public class Path
{
        public Position Position { get; set; }
        public Direction Direction { get; set; }
        public int StraightLineLength { get; set; }
    
        public int Heat { get; set; }
    
        public Path (Position position, Direction direction, int straightLineLength)
        {
                Position = position;
                Direction = direction;
                StraightLineLength = straightLineLength;
        }
}

public class Direction
{
    
        public int Row { get; set; }
        public int Col { get; set; }
        public Direction (int row, int col)
        {
                Row = row;
                Col = col;
        }
 
        public Direction TurnLeft()
        {
                return new Direction(-Col, Row);
        }
 
        public Direction TurnRight()
        {
                return new Direction(Col, -Row);
        }
 
        public static Direction Up = new(-1, 0);
        public static Direction Down = new(1, 0);
        public static Direction Left = new(0, -1);
        public static Direction Right = new(0, 1);

        public Direction(int col) : this(0, col)
        {
        }
}

public class Position
{
        public int Row { get; set; }
        public int Col { get; set; }

        public Position(int row, int col)
        {
                Row = row;
                Col = col;
        }
    
        public Position Move(Direction dir)
        {
                return new Position(Row + dir.Row, Col + dir.Col);
        }
}