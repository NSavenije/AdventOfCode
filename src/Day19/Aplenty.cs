#nullable disable

using System.Text;

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
        List<string> lines = File.ReadAllLines("src/Day19/19.in").ToList();
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
        long result = Solve(rls, 0, [1, 4000, 1, 4000, 1, 4000, 1, 4000], workflows);
        Console.WriteLine(result);
    }
#pragma warning disable CS1717 // Assignment made to same variable
    static long Solve(List<Rule> rules, int ruleIndex, int[] partIndices, Dictionary<string,List<Rule>> workflows)
    {  
        //This will be recursion
        //Simple cases are encountering D or OOB and action an A or and R
        Rule rule = rules[ruleIndex];
        Console.WriteLine(PrintIndices(partIndices));
        if (rule.Comp == "D" || FullyInBound(rule, partIndices))
        {
            if (rule.Action == "R")
                return 0;
            if (rule.Action == "A")
                return CalcScore(partIndices);
            return Solve(workflows[rule.Action],0,partIndices,workflows);
        }
        if(OutOfBounds(rule, partIndices))
        {
            return 0;
        }

        // Now the more interesting cases where we are going to split
        int[] matchIndices = new List<int>(partIndices).ToArray();
        int[] nextIndices = new List<int>(partIndices).ToArray();
    
        (_, _, int i) = Decode(rule.Comp, partIndices); // i is low index
        if (rule.IsLarger)
        {
            // [1000..2000] > 1500 -> NEXTINDEX [1000..1500] ; MATCHINDEX [1501..2000]
            nextIndices[i+1] = rule.Num;
            matchIndices[i] = rule.Num + 1; 
        }
        else
        {
            // [1000..2000] < 1500 -> MATCH INDEX [1000..1499] ; NEXT INDEX [1500..2000]   
            matchIndices[i + 1] = rule.Num - 1;
            nextIndices[i] = rule.Num;
        }
        long match;
        if (rule.Action == "A")
            match = CalcScore(matchIndices);
        else if (rule.Action == "R")
            match = 0;
        else
            match = Solve(workflows[rule.Action], 0, matchIndices, workflows);

        long nextRule = Solve(rules, ruleIndex + 1, nextIndices, workflows);
        return match + nextRule;
    }

#pragma warning restore CS1717 // Assignment made to same variable

//167409079868000
//167409079868000
//167010937327821
//256000000000000
//1684715944830566400
//81649509456
//81649509556

    static bool OutOfBounds(Rule r, int[] indices)
    {
        (int low, int high, _) = Decode(r.Comp, indices);
        if (r.IsLarger)
            return high <= r.Num; 
        else
            return low >= r.Num;
    }

    static bool FullyInBound(Rule r, int[] indices)
    {
        // If fully inbounds ie [0..1000] < 2000 || [3000..4000] > 2000        
        (int low, int high, _) = Decode(r.Comp, indices);
        if (r.IsLarger && low > r.Num)
            return true;
        if (!r.IsLarger && high < r.Num)
            return true;
        return false;
    }

    static (int,int,int) Decode(string c, int[] indices)
    {
        return c switch
        {
            "X" => (indices[0], indices[1],0),
            "M" => (indices[2], indices[3],2),
            "A" => (indices[4], indices[5],4),
            _   => (indices[6], indices[7],6)
        };
    }

    static long CalcScore(int[] i) 
    {
        long x = i[1] - i[0] + 1;
        long m = i[3] - i[2] + 1;
        long a = i[5] - i[4] + 1;
        long s = i[7] - i[6] + 1;
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