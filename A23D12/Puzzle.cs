using System.Text.RegularExpressions;

namespace A23D12;

public class Puzzle
{
    public List<Spring> Springs { get; set; }
    
    public List<Spring> Springs2 { get; set; }

    public Dictionary<string, long> Cache { get; set; }

    public long Total { get; set; }
    
    public long Total2 { get; set; }

    public Puzzle(List<string> lines)
    {
        Cache = new Dictionary<string, long>();
        Springs = lines.Select(l => new Spring(l)).ToList();
        Springs2 = lines.Select(l => new Spring(l,2)).ToList();
        Total = Springs.Sum(s => Calculate(s.Signs, s.Numbers));
        Total2 = Springs2.Sum(s => Calculate(s.Signs, s.Numbers));
    }

    private long Calculate(string signs, List<int> numbers)
    {
        var key = signs + string.Join(',', numbers);
        if (Cache.TryGetValue(key, out var value))
        {
            return value;
        }

        value = Count(signs, numbers);
        Cache[key] = value;
        return value;
    }

    private long Count(string signs, List<int> numbers)
    {
        while (true)
        {
            if (numbers.Count == 0)
            {
                return signs.Contains('#') ? 0 : 1;
            }

            if (string.IsNullOrEmpty(signs))
            {
                return 0;
            }

            if (signs.StartsWith('.'))
            {
                signs = signs.Trim('.');
                continue;
            }

            if (signs.StartsWith('?'))
            {
                return Calculate("." + signs[1..], numbers) + Calculate("#" + signs[1..], numbers);
            }

            if (signs.StartsWith('#'))
            {
                if (numbers.Count == 0)
                {
                    return 0;
                }

                if (signs.Length < numbers[0])
                {
                    return 0;
                }

                if (signs[..numbers[0]].Contains('.'))
                {
                    return 0;
                }

                if (numbers.Count > 1)
                {
                    if (signs.Length < numbers[0] + 1 || signs[numbers[0]] == '#')
                    {
                        return 0;

                    }

                    signs = signs[(numbers[0] + 1)..];
                    numbers = numbers.Skip(1).ToList();
                    continue;
                }

                signs = signs[numbers[0]..];
                numbers = numbers.Skip(1).ToList();
                continue;
            }
        }
    }
}

public class Spring
    {
        public string Signs { get; set; }
        public List<int> Numbers { get; set; }

        public Spring(string line)
        {
            var split = line.Split(' ');
            Signs = split[0];
            Numbers = split[1].Trim().Split(',').Select(s => int.Parse(s)).ToList();
        } 
        public Spring(string line, int indicator)
        {
            var split = line.Split(' ');
            Signs = indicator == 2 ? string.Join('?', Enumerable.Range(0,5).Select(c => split[0] ).ToArray()) :split[0];
            var split2 = indicator == 2? string.Join(',',Enumerable.Range(0,5).Select(c => split[1]).ToArray()) : split[1].Trim();
            Numbers = split2.Split(',').Select(s => int.Parse(s)).ToList();
        }
    }

 