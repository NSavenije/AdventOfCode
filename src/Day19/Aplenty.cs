#nullable disable

public static class Day19
{
    public static void Solve1()
    {
        Dictionary<string,List<Rule>> workflows = [];
        List<Part> parts = [];
        List<string> lines = File.ReadAllLines("src/Day19/19.in").ToList();
        bool areRules = true;
        foreach(string line in lines)
        {
            if (line == "")
            {
                areRules = false;
                continue;
            }
            if (areRules)
            {
                var pts = line.Split("{");
                string lbl = pts[0];
                var rules = pts[1].Replace("}","").Split(',').ToList();
                List<Rule> maps = [];
                foreach(string rule in rules)
                {
                    var comp = rule[0] switch
                    {
                        'x' => "X",
                        'm' => "M",
                        'a' => "A",
                        's' => "S",
                        _ => "D",
                    };
                    if (comp == "D" || (rule[1] != '<' && rule[1] != '>'))
                    {
                        maps.Add(new Rule("D",true,-1,rule));
                        workflows.Add(lbl,maps);
                        break;
                    }
                    else
                    {
                        bool islrgr = rule[1] == '>';
                        var numAction = rule.Split(':');
                        int num = int.Parse(numAction[0][2..]);
                        string action = numAction[1];
                        maps.Add(new Rule(comp, islrgr, num, action));
                    }
                }

            }
            else // Are parts
            {
                var pts = line.Replace("{","").Replace("}","").Split(",").ToList();
                int x = -1,m = -1,a = -1,s = -1;
                foreach(string p in pts)
                {
                    switch (p[0])
                    {
                        case 'x': 
                            x = int.Parse(p[2..]);
                            break;
                        case 'm': 
                            m = int.Parse(p[2..]);
                            break;
                        case 'a': 
                            a = int.Parse(p[2..]);
                            break;
                        default:  
                            s = int.Parse(p[2..]);
                            break;
                    };
                }
                parts.Add(new Part(x, m, a, s));
            }   
        }
        long total = 0;
        foreach(Part p in parts)
        {
            Console.WriteLine(p);
            List<Rule> rules = workflows["in"];
            total += Traverse(p, rules, workflows);
        }
        Console.WriteLine(total);
    }

    public static void Solve2()
    {
        Dictionary<string,List<Rule>> workflows = [];
        List<string> lines = File.ReadAllLines("src/Day19/19b.in").ToList();
        foreach(string line in lines)
        {
            if (line == "")
                break;

            var pts = line.Split("{");
            string lbl = pts[0];
            var rules = pts[1].Replace("}","").Split(',').ToList();
            List<Rule> maps = [];
            foreach(string rule in rules)
            {
                var comp = rule[0] switch
                {
                    'x' => "X",
                    'm' => "M",
                    'a' => "A",
                    's' => "S",
                    _ => "D",
                };
                if (comp == "D" || (rule[1] != '<' && rule[1] != '>'))
                {
                    maps.Add(new Rule("D",true,-1,rule));
                    workflows.Add(lbl,maps);
                    break;
                }
                else
                {
                    bool islrgr = rule[1] == '>';
                    var numAction = rule.Split(':');
                    int num = int.Parse(numAction[0][2..]);
                    string action = numAction[1];
                    maps.Add(new Rule(comp, islrgr, num, action));
                }
            }
        }
        List<Rule> rls = workflows["in"];
        long result = Solve(rls, [1, 4000, 1, 4000, 1, 4000, 1, 4000], workflows, 0);
        Console.WriteLine(result);
    }
#pragma warning disable CS1717 // Assignment made to same variable
    static long Solve(List<Rule> rules, int[] indices, Dictionary<string,List<Rule>> workflows, long acc)
    {  
        foreach(Rule rule in rules)
        {
            Console.WriteLine(PrintIndices(indices) + " " + acc);
            (int low, int high) index = (-1,-1);
            int i = -1;
            switch (rule.Comp) 
            {
                case "X": 
                    i = 0;
                    index = (indices[0], indices[1]);
                    break;
                case "M": 
                    i = 2;
                    index = (indices[2], indices[3]);
                    break;
                case "A": 
                    i = 4;
                    index = (indices[4], indices[5]);
                    break;
                case "S": 
                    i = 6;
                    index = (indices[6], indices[7]);
                    break;
                default:
                    break;
            }
            if (rule.Comp == "D")
            {
                if (rule.Action == "A")
                {
                    return CalcScore(indices);
                }
                if (rule.Action == "R")
                    return 0;
                else
                    return Solve(workflows[rule.Action],indices,workflows, acc);
            }
            else if (rule.IsLarger && index.high > rule.Num)
            {
                // x > 2500, but x = [1000..2000] No action, nothing will be selected
                // x > 1500  and x = [1000..2000] solve for [1501..2000], continue with [1000.1500]
                // x >  500  and x = [1000..2000] solve for [1000..2000]
                // So only do something is index.high > rule.num
                
                // The part that false witin the new range should be solved for
                var temp = indices[i];
                indices[i] = Math.Max(index.low, rule.Num + 1);
                if (indices[i] == temp)
                {
                    if (rule.Action == "A")
                        return CalcScore(indices);
                    if (rule.Action == "R")
                        return 0;
                    else
                        return Solve(workflows[rule.Action], indices, workflows, acc); // Everyone is in the range 
                }
                if (rule.Action == "A")
                    acc += CalcScore(indices);
                else if (rule.Action == "R")
                    acc = acc;
                else
                    acc += Solve(workflows[rule.Action], indices, workflows, acc); // Some are in the range
                indices[i] = temp;
                indices[i + 1] = rule.Num;
            }
            else if (!rule.IsLarger && index.low < rule.Num)
            {
                // x < 2500, but x = [1000..2000] solve for [1000..2000]
                // x < 1500  and x = [1000..2000] solve for [1000..1499], continue with [1500..2000]
                // x <  500  and x = [1000..2000] No action, nothing will be selected
                // So only do something is index.high > rule.num
                
                // The part that false witin the new range should be solved for
                var temp = indices[i + 1];
                indices[i + 1] = Math.Min(index.high, rule.Num - 1);
                if (indices[i + 1] == temp)
                {
                    if (rule.Action == "A")
                        return CalcScore(indices);
                    if (rule.Action == "R")
                        return 0;
                    else
                        return Solve(workflows[rule.Action], indices, workflows, acc); // Everyone is in the range 
                }
                if (rule.Action == "A")
                    acc += CalcScore(indices);
                else if (rule.Action == "R")
                    acc = acc;
                else
                    acc += Solve(workflows[rule.Action], indices, workflows, acc); // Some are in the range
                indices[i + 1] = temp;
                indices[i] = rule.Num;
            }
        }
        return acc;
    }

#pragma warning restore CS1717 // Assignment made to same variable

//167409079868000
//256000000000000
//1684715944830566400
//81649509456
//81649509556

    static long CalcScore(int[] i) 
    {
        long x = i[1] - i[0];
        long m = i[3] - i[2];
        long a = i[5] - i[4];
        long s = i[7] - i[6];
        return x*m*a*s;
    }

    static string PrintIndices (int[] i) =>
        $"({i[0]},{i[1]}),({i[2]},{i[3]}),({i[4]},{i[5]}),({i[6]},{i[7]})";

    static int Traverse(Part p, List<Rule> rules, Dictionary<string,List<Rule>> workflows)
    {
        foreach(Rule rule in rules)
        {
            bool doAction = false;
            int val = GetVariableValue(rule.Comp, p);
            if (rule.Comp == "D")
            {
                doAction = true;
            }
            else if (rule.IsLarger && val > rule.Num)
            {
                doAction = true;
            }
            else if(!rule.IsLarger && val < rule.Num)
            {
                doAction = true;
            }
            if (doAction)
            {
                if (rule.Action == "A")
                {
                    return p.X + p.M + p.A + p.S;
                }
                if (rule.Action == "R")
                {
                    return 0;
                }
                else
                {
                    return Traverse(p, workflows[rule.Action],workflows);
                }
            }
        }
        Console.WriteLine("Something went to shit");
        return -1;
    }

    static int GetVariableValue(string comp, Part part)
    {
        return comp switch
        {
            "X" => part.X,
            "M" => part.M,
            "A" => part.A,
            "S" => part.S,
            _ => -1
        };
    }


    readonly record struct Part(int X, int M, int A, int S)
    {
        public int X { get; } = X;
        public int M { get; } = M;
        public int A { get;} = A;
        public int S { get; } = S;


        public override string ToString() => $"X{X} M{M} A{A} S{S}";
    }

    // public enum Comp{
    //     X = 0,
    //     M = 1,
    //     A = 2,
    //     S = 3,
    //     D = 4
    // }

    readonly record struct Rule(string Comp, bool IsLarger, int Num, string Action)
    {
        public string Comp { get; } = Comp;
        public bool IsLarger { get; } = IsLarger;
        public int Num { get;} = Num;
        public string Action {get;} = Action;


        public override string ToString() => $"{Comp} {(IsLarger ? ">" : "<")} {Num}:{Action}";
    }
}