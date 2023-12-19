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

    }

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