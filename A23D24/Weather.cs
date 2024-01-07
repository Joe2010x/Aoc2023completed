using Microsoft.Z3;

namespace A23D24;

public class Weather
{
    public List<Hail> Hails { get; set; }
    public long Min { get; set; }
    public long Max { get; set; }
    public int Intersects { get; set; }

    public Weather(List<string> lines, long min, long max)
    {
        Max = max;
        Min = min;
        Hails = new List<Hail>();
        Intersects = 0;
        foreach (var line in lines)
        {
            var split = line.Split('@', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var position = split[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(double.Parse).ToArray();
            var velocity = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(double.Parse).ToArray();
            var hail = new Hail(
                new Position(position[0], position[1], position[2]),
                new Position(velocity[0], velocity[1], velocity[2]));

            var next = hail.Move();
            var slope = (next.Position.Y - hail.Position.Y) / (next.Position.X - hail.Position.X);
            var intercept = next.Position.Y - slope * next.Position.X;

            Hails.Add(hail with { Slope = slope, Intersect = intercept });
        }

        FindIntersects();

    }

    private void FindIntersects()
    {

        var visited = new HashSet<Hail>();
        const int b = -1;
        foreach (var first in Hails)
        {
            visited.Add(first);
            var a1 = first.Slope;
            var c1 = first.Intersect;
            var x1 = first.Position.X;
            var y1 = first.Position.Y;
            var vx1 = first.Velocity.X;
            var vy1 = first.Velocity.Y;

            foreach (var second in Hails.Where(h => !visited.Contains(h)))
            {
                var a2 = second.Slope;
                var c2 = second.Intersect;
                var x2 = second.Position.X;
                var y2 = second.Position.Y;
                var vx2 = second.Velocity.X;
                var vy2 = second.Velocity.Y;

                if (a1 == a2)
                {
                    continue;
                }

                var x = (b * c2 - b * c1) / (a1 * b - a2 * b);
                var y = (a2 * c1 - a1 * c2) / (a1 * b - a2 * b);

                if (x - x1 < 0 != vx1 < 0 || y - y1 < 0 != vy1 < 0)
                {
                    continue;
                }

                if (x - x2 < 0 != vx2 < 0 || y - y2 < 0 != vy2 < 0)
                {
                    continue;
                }

                if (new Position(x, y).InBounds(Min, Max))
                {
                    Intersects++;
                }
            }
        }
        
    }


    public long Solve(List<Hail> hails)
    {
        var ctx = new Context();
        var solver = ctx.MkSolver();

        // Coordinates of the stone
        var x = ctx.MkIntConst("x");
        var y = ctx.MkIntConst("y");
        var z = ctx.MkIntConst("z");

        // Velocity of the stone
        var vx = ctx.MkIntConst("vx");
        var vy = ctx.MkIntConst("vy");
        var vz = ctx.MkIntConst("vz");

        // For each iteration, we will add 3 new equations and one new condition to the solver.
        // We want to find 9 variables (x, y, z, vx, vy, vz, t0, t1, t2) that satisfy all the equations, so a system of 9 equations is enough.
        for (var i = 0; i < 3; i++)
        {
            var t = ctx.MkIntConst($"t{i}"); // time for the stone to reach the hail
            var hail = hails[i];

            var px = ctx.MkInt(Convert.ToInt64(hail.Position.X));
            var py = ctx.MkInt(Convert.ToInt64(hail.Position.Y));
            var pz = ctx.MkInt(Convert.ToInt64(hail.Position.Z));

            var pvx = ctx.MkInt(Convert.ToInt64(hail.Velocity.X));
            var pvy = ctx.MkInt(Convert.ToInt64(hail.Velocity.Y));
            var pvz = ctx.MkInt(Convert.ToInt64(hail.Velocity.Z));

            var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx)); // x + t * vx
            var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy)); // y + t * vy
            var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz)); // z + t * vz

            var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx)); // px + t * pvx
            var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy)); // py + t * pvy
            var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz)); // pz + t * pvz

            solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
            solver.Add(ctx.MkEq(xLeft, xRight)); // x + t * vx = px + t * pvx
            solver.Add(ctx.MkEq(yLeft, yRight)); // y + t * vy = py + t * pvy
            solver.Add(ctx.MkEq(zLeft, zRight)); // z + t * vz = pz + t * pvz
        }

        solver.Check();
        var model = solver.Model;

        var rx = model.Eval(x);
        var ry = model.Eval(y);
        var rz = model.Eval(z);

        return Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString());
    }
}

public record Position(double X, double Y, double Z = 0)
{
    public Position Move(double x, double y, double z) => new(X + x, Y + y, Z + z);
    public bool InBounds(double min, double max) => X >= min && X <= max && Y >= min && Y <= max;
}
 
public record Hail(Position Position, Position Velocity, double Slope = 0, double Intersect = 0)
{
    public Hail Move() => this with { Position = Position.Move(Velocity.X, Velocity.Y, Velocity.Z) };
}