#nullable disable
public static class Day20
{
    public static void Solve1()
    {
        List<string> lines = File.ReadAllLines("src/Day20/20.in").ToList();
        Dictionary<string,Module> modules = ParseInput(lines);
        HashSet<string> states = [];
        states.Add(ToState(modules));
        int low = 0;
        int high = 0;
        for(int i = 0; i < 1000; i++)
        {
            (int _low, int _high) = PressButton(modules, out modules);
            low += _low;
            high += _high;
        }
        Console.WriteLine(low * high);
    }

    public static void Solve2()
    {
        List<string> lines = File.ReadAllLines("src/Day20/20.in").ToList();
        Dictionary<string,Module> modules = ParseInput(lines);

        Module broadcaster = modules["broadcaster"];
        List<long> cycles = [];
        HashSet<string> states = [];
        states.Add(ToState(modules));
        foreach(var dest in broadcaster.Dests)
        {
            modules["broadcaster"].Dests = [dest];
            for(int i = 0; ; i++)
            {
                PressButton(modules, out modules);
                if(!states.Add(ToState(modules)))
                {
                    cycles.Add(i);
                    break;   
                }
            }
        }
        Console.WriteLine(cycles.Aggregate(1L, (acc, cycle) => acc * cycle));
    }

    static (int,int) PressButton(Dictionary<string,Module> modules, out Dictionary<string,Module> outModules)
    {
        Queue<Pulse> pulses = [];
        pulses.Enqueue(new Pulse("broadcaster", 0, "button"));
        int low = 0; int high = 0;
        while(pulses.Count != 0)
        {
            Pulse pulse = pulses.Dequeue();
            if (pulse.Power == 0)
                low++;
            else 
                high++;

            if (modules.TryGetValue(pulse.Label, out var module))
            {
                switch (module.Type)
                {
                    case 0: // Flippity Floppity, your pulse is now my property
                        if (pulse.Power == 0)
                        {
                            module.On = !module.On;
                            module.Dests.ForEach(dest => pulses.Enqueue(new Pulse(dest, module.On ? 1 : 0, pulse.Label)));
                        }
                        break;
                    case 1:
                        module.Inputs[pulse.Sender] = pulse.Power == 1;
                        int pow = module.Inputs.All(b => b.Value) ? 0 : 1;
                        module.Dests.ForEach(dest => pulses.Enqueue(new Pulse(dest, pow, pulse.Label)));
                        break;
                    default:
                        module.Dests.ForEach(dest => pulses.Enqueue(new Pulse(dest, pulse.Power, pulse.Label)));
                        break;
                }
            }
        }
        outModules = modules;
        return (low,high);
    }

    static string ToState(Dictionary<string, Module> modules)
    {
        return string.Join("\n", modules.Values.Select(m =>
        {
            string inputs = string.Join(",", m.Inputs.Select(input => $"{input.Key}:{input.Value}"));
            return $"{m.Label} {m.On} {inputs}";
        }));
    }


    static Dictionary<string,Module> ParseInput(List<string> lines)
    {
        Dictionary<string,Module> modules = [];
        foreach(string l in lines)
        {
            List<string> targets = l.Split('>')[1].Trim().Split(',').Select(s => s.Trim()).ToList();
            string lab = l.Split('-')[0].Trim().ToString();
            switch (l[0])
            {
                case '%':
                    modules.Add(lab[1..],new Module(lab[1..],0,targets,false,[])); //flip flop
                    break;
                case '&':
                    modules.Add(lab[1..],new Module(lab[1..],1,targets,false,[])); // confluence
                    break;
                default:
                    modules.Add(lab,new Module(lab,2,targets,false,[])); //broadcaster...
                    break;
            }
        }
        foreach (var m in modules.Values)
        {
            foreach (string dest in m.Dests)
            {
                if (modules.TryGetValue(dest, out var module) && module.Type == 1)
                {
                    module.Inputs.Add(m.Label, false);
                }
            }
        }

        return modules;
    }

    class Module(string lab, int type, List<string> dests, bool on, Dictionary<string, bool> inputs)
    {
        public string Label { get; } = lab;
        public int Type { get; } = type;
        public List<string> Dests { get; set; } = dests;
        public bool On { get; set; } = on;
        public Dictionary<string, bool> Inputs { get; set; } = inputs;
    }

    readonly record struct Pulse(string Label, int Power, string Sender);
}