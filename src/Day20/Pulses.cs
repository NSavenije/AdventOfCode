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
        HashSet<string> states = [];
        
        states.Add(ToState(modules));
        Console.WriteLine(ToState(modules));
        long tx = -1;
        long dd = -1;
        long jh = -1;
        long ph = -1;
        for(int i = 0; i < 1000000; i++)
        {
            if (i % 100000 == 0)
                Console.WriteLine(i);
            if(PressButton2(modules, out modules))
            {
                if(tx == -1)
                tx = i;
                // Console.WriteLine(i);
            }
            if(!states.Add(ToState(modules)))
            {
                Console.WriteLine(ToState(modules));
                Console.WriteLine("CYCLE " + i);
                break;
            }
        }
        // foreach(var state in states)
        // {
        //     Console.WriteLine(state);
        // }
    }

    static (int,int) PressButton(Dictionary<string,Module> modules, out Dictionary<string,Module> outModules)
    {
        Queue<Pulse> pulses = [];
        pulses.Enqueue(new Pulse("broadcaster", 0, "button"));
        int low = 0; int high = 0;
        while(pulses.Count != 0)
        {
            Pulse pulse = pulses.Dequeue();
            // Console.WriteLine(pulse.Label + " " + pulse.Power);
            if (pulse.Power == 0)
                low++;
            else 
                high++;

            if (modules.TryGetValue(pulse.Label, out var module))
            {
                switch (module.Type)
                {
                    case 0: // Flippity Floppity, your pulse is now my property
                        // Console.WriteLine("flipflop");
                        if (pulse.Power == 0)
                        {
                            module.On = !module.On;
                            foreach(var dest in module.Dests)
                            {
                                pulses.Enqueue(new Pulse(dest,module.On ? 1 : 0, pulse.Label));
                            }
                        }
                        break;
                    case 1:
                    // Console.WriteLine("conf");
                        module.Inputs[pulse.Sender] = pulse.Power == 1;
                        if (module.Inputs.All(b => b.Value))
                        {
                            foreach(var dest in module.Dests)
                            {
                                pulses.Enqueue(new Pulse(dest, 0, pulse.Label));
                            }
                        }
                        else
                        {
                            foreach(var dest in module.Dests)
                            {
                                pulses.Enqueue(new Pulse(dest, 1, pulse.Label));
                            }
                        }
                        break;
                    default:
                        // Console.WriteLine("broadcast");
                        foreach(var dest in module.Dests)
                        {
                            pulses.Enqueue(new Pulse(dest, pulse.Power, pulse.Label));
                        }
                        break;
                }
            }
        }
        outModules = modules;
        return (low,high);
    }
    static bool PressButton2(Dictionary<string,Module> modules, out Dictionary<string,Module> outModules)
    {
        Queue<Pulse> pulses = [];
        pulses.Enqueue(new Pulse("broadcaster", 0, "button"));
        int tx = 0;
        while(pulses.Count != 0)
        {
            Pulse pulse = pulses.Dequeue();
            // Console.WriteLine(pulse.Label + " " + pulse.Power);
            // if (pulse.Label == "rx" && pulse.Power == 0)
            //     rx++;
            if (modules.TryGetValue(pulse.Label, out var module))
            {
                switch (module.Type)
                {
                    case 0: // Flippity Floppity, your pulse is now my property
                        // Console.WriteLine("flipflop");
                        if (pulse.Power == 0)
                        {
                            module.On = !module.On;
                            foreach(var dest in module.Dests)
                            {
                                pulses.Enqueue(new Pulse(dest,module.On ? 1 : 0, pulse.Label));
                            }
                        }
                        break;
                    case 1:
                        module.Inputs[pulse.Sender] = pulse.Power == 1;
                        // if (module.Label == "ls")
                        // {
                        //     Console.WriteLine($"Received pulse from {pulse.Sender} with power {pulse.Power}");
                        // }
                        // List<string> lsss = [];
                        // foreach(var kv in module.Inputs)
                        // {
                        //     lsss.Add("(" + kv.Key + " " + kv.Value.ToString() + ")"); 
                        // }
                        // Console.WriteLine(module.Label + ": " + string.Join(',', lsss));
                        if (module.Inputs.All(b => b.Value))
                        {
                            
                            foreach(var dest in module.Dests)
                            {
                                // Console.WriteLine($"Send LOW from {module.Label} to {dest}");
                                if (dest == "nz")
                                {
                                    // Console.WriteLine($"Send LOW from {module.Label} to {dest}");
                                    tx++;
                                }

                                pulses.Enqueue(new Pulse(dest, 0, pulse.Label));
                            }
                        }
                        else
                        {
                            foreach(var dest in module.Dests)
                            {
                                pulses.Enqueue(new Pulse(dest, 1, pulse.Label));
                            }
                        }
                        break;
                    default:
                        // Console.WriteLine("broadcast");
                        foreach(var dest in module.Dests)
                        {
                            pulses.Enqueue(new Pulse(dest, pulse.Power, pulse.Label));
                        }
                        break;
                }
            }
        }
        outModules = modules;
        return tx == 1;
    }

    static State ToState2(Dictionary<string,Module> modules)
    {
        List<(string,bool,int)> state = [];
        
        foreach(Module m in modules.Values)
        {
            state.Add((m.Label, m.On, m.Inputs.GetHashCode()));
        }
        return new State(state);
    }

    static string ToState(Dictionary<string,Module> modules)
    {
        List<(string,bool,int)> state = [];
        string states = "";
        foreach(Module m in modules.Values)
        {
            string inputs = "";
            foreach(var input in m.Inputs)
            {   
                inputs += input.Key + ":" + input.Value + ",";
            }
            states += $"{m.Label} {m.On} {inputs} \n";
        }
        return states;
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
        foreach(Module m in modules.Values)
        {
            foreach(string dest in m.Dests)
            {
                if (modules.TryGetValue(dest, out var module))
                {
                    if (module.Type == 1)
                    {
                        module.Inputs.Add(m.Label, false);
                    }
                }
            }
        }
        return modules;
    }

    class Module(string lab, int type, List<string> dests, bool on, Dictionary<string, bool> inputs)
    {
        public string Label { get; } = lab;
        public int Type { get; } = type;
        public List<string> Dests { get; } = dests;
        public bool On { get; set; } = on;
        public Dictionary<string, bool> Inputs { get; set; } = inputs;
    }

    readonly record struct State(List<(string,bool,int)> Modules);
    readonly record struct Pulse(string Label, int Power, string Sender);
}

//tx, dd, nz, ph
//tx <- &dc <-
//dd <-&qm
//nz <- &jh
//ph <- &zq


//4050 tx
//3888 dd
//3778 ph
//3906 nz