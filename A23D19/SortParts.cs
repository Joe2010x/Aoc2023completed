using System.Text.RegularExpressions;

namespace A23D19;

public class SortParts
{
        public List<Dictionary<Part, int>> Parts { get; set; }
        public Dictionary<string, List<Rule>> Rules { get; set; }
    
        public int Part1ret { get; set; }
        public ulong Total { get; set; }

        public SortParts(List<string> lines)
        {
                var rulesTranslate = true;
                Rules = new Dictionary<string, List<Rule>>();
                Parts = new List<Dictionary<Part, int>>();
                foreach (var line in lines)
                {
                       if (line.Length == 0)
                       {
                               rulesTranslate = false;
                               continue;
                       }
                       if (rulesTranslate)
                       {
                               var tuple = TranslateRule(line);
                               Rules.Add(tuple.Item1, tuple.Item2);
                       }
                       else
                       {
                               Parts.Add(TranslateParts(line));
                       }
                }
                FollowRules();
                SplitRange();
        }

        private void SplitRange()
        {
                var complete = new List<Dictionary<Part, PRange>>();    
                var initial = new Dictionary<Part, PRange>()
                {
                        { Part.X, new PRange(1, 4000) }, { Part.M, new PRange(1, 4000) },
                        { Part.A, new PRange(1, 4000) }, { Part.S, new PRange(1, 4000) }
                };
                var q = new Queue<(Dictionary<Part, PRange>, string, int)>();
                q.Enqueue((initial, "in", 0));
   
                while (q.Count > 0)
                {
                        var (parts, name, stepNum) = q.Dequeue();
                        var rule = Rules[name];
                        var step = rule[stepNum];
                        foreach (var res in SplitRange(parts, step, name, stepNum))
                        {
                                if (res.Item2 == "A")
                                        complete.Add(res.Item1);
                                else if (res.Item2 != "R")
                                        q.Enqueue(res);            
                        }
                }

                foreach (var r in complete)
                {
                        ulong a = Convert.ToUInt64(r[Part.X].Hi - r[Part.X].Lo + 1);
                        ulong b = Convert.ToUInt64(r[Part.M].Hi - r[Part.M].Lo + 1);
                        ulong c = Convert.ToUInt64(r[Part.A].Hi - r[Part.A].Lo + 1);
                        ulong d = Convert.ToUInt64(r[Part.S].Hi - r[Part.S].Lo + 1);
                        Total += a * b * c * d;        
                }
        }
        
        static List<(Dictionary<Part, PRange>, string, int)> SplitRange(Dictionary<Part, PRange> parts, Rule step, string stepName, int stepNum)
        {   
                var result = new List<(Dictionary<Part, PRange> parts, string, int)>();
                var pass = parts.ToDictionary(e => e.Key, e => e.Value);
                var fail = parts.ToDictionary(e => e.Key, e => e.Value);
                int lo, hi;

                switch (step.Op)
                {
                        case Op.LT:
                                lo = parts[step.In].Lo;
                                hi = parts[step.In].Hi;
                                pass[step.In] = new PRange(lo, step.Val - 1);
                                fail[step.In] = new PRange(step.Val, hi);
                                result.Add((pass, step.Out, 0));
                                result.Add((fail, stepName, stepNum + 1));
                                break;
                        case Op.GT:
                                lo = parts[step.In].Lo;
                                hi = parts[step.In].Hi;
                                pass[step.In] = new PRange(step.Val + 1, hi);
                                fail[step.In] = new PRange(lo, step.Val);
                                result.Add((pass, step.Out, 0));
                                result.Add((fail, stepName, stepNum + 1));
                                break;
                        case Op.EQ:
                                result.Add((parts, step.Out, 0));
                                break;
                }
    
                return result;
        }

        private void FollowRules()
        {
                Part1ret = Parts.Where(p => Test(p, Rules))
                        .Select(p => p.Values.Sum())
                        .Sum();
                
        }
        
        static bool Test(Dictionary<Part, int> part, Dictionary<string, List<Rule>> rules)
        {
                string stage = "in";

                do
                {
                        stage = Classify(part, rules[stage], 0);
                }
                while (!(stage == "A" || stage == "R"));
    
                return stage == "A";
        }
        static string Classify(Dictionary<Part, int> part, List<Rule> steps, int s)
        {
                return steps[s].Op switch
                {
                        Op.LT => part[steps[s].In] < steps[s].Val ? steps[s].Out : Classify(part, steps, s + 1),
                        Op.GT => part[steps[s].In] > steps[s].Val ? steps[s].Out : Classify(part, steps, s + 1),        
                        Op.EQ => steps[s].Out
                };
        }

        static Dictionary<Part, int> TranslateParts(string line)
        {    
                var m = Regex.Match(line, @"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
                return new Dictionary<Part, int>
                {
                        { Part.X, int.Parse(m.Groups[1].Value) }, { Part.M, int.Parse(m.Groups[2].Value) },
                        { Part.A, int.Parse(m.Groups[3].Value) }, { Part.S, int.Parse(m.Groups[4].Value) }
                };    
        }
        
        public static (string, List<Rule>) TranslateRule(string line)
        {
                int n = line.IndexOf('{');
                string name = line.Substring(0, n);
                var steps = new List<Rule>();
    
                foreach (var p in line[(n+1)..^1].Split(','))
                {
                        int sc = p.IndexOf(':');
                        if (p.IndexOf('<') > -1) 
                        {
                                Part f = ToField(p[0]);
                                int v = int.Parse(p[(p.IndexOf('<')+1)..sc]);
                                steps.Add(new Rule(f, p[(sc+1)..], Op.LT, v));
                        }
                        else if (p.IndexOf('>') > -1)
                        {
                                Part f = ToField(p[0]);
                                int v = int.Parse(p[(p.IndexOf('>')+1)..sc]);
                                steps.Add(new Rule(f, p[(sc+1)..], Op.GT, v));
                        }
                        else
                        {
                                steps.Add(new Rule(Part.Any, p, Op.EQ, -1));
                        }
                }
    
                return (name, steps.ToList());
        }
        static Part ToField(char ch)
        {
                return ch switch 
                {
                        'x' => Part.X,
                        'm' => Part.M,
                        'a' => Part.A,
                        's' => Part.S
                };
        }
}





public enum Op { LT, GT, EQ }
public enum Part { X, M, A, S, Any }
public record Rule (Part In, string Out, Op Op, int Val);
public record PRange(int Lo, int Hi);