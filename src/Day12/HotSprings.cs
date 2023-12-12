using System.Diagnostics;
#nullable disable

public static class Day12 
{
    public static void Solve1() 
    {
        // var funcWatch = Stopwatch.StartNew();
        Console.WriteLine(ParseInput("src/Day12/12.in", 1));
        // Console.WriteLine(funcWatch.ElapsedMilliseconds + "ms");
    }
    public static void Solve2()
    {
        Console.WriteLine(ParseInput("src/Day12/12.in", 5));
    }

    static long ParseInput(string filePath, int repeat) 
    {
        long sum = 0;
        List<string> input = File.ReadAllLines(filePath).ToList();
        foreach(string s in input)
        {
            string records = s.Split(' ')[0];
            records = string.Join('?',Enumerable.Repeat(records, repeat));
            
            string grps = s.Split(' ')[1].Trim();
            int[] groups = string.Join(',',Enumerable.Repeat(grps, repeat)).Split(',').Select(int.Parse).ToArray();
            
            var funcWatch = Stopwatch.StartNew();
            long res = Solve(records, 0, groups, []);
            sum += res;
            // Console.WriteLine("sum: " + sum + ", res: " + res + ", " + funcWatch.ElapsedMilliseconds + "ms : " + records + " " + string.Join(',',groups));
        }
        return sum;
    }

    static long Solve(string pattern, int i, int[] groups, Dictionary<(string,int[], int),long> cache) {
        if (!cache.ContainsKey((pattern, groups, i))) {
            cache[(pattern, groups, i)] = Compute(pattern, i, groups, cache);
        }
        return cache[(pattern, groups, i)];
    }

    static long Compute(string s, int i, int[] groups, Dictionary<(string,int[], int),long> cache) {
        if (s == "")
            return groups.Length - i == 0 ? 1 : 0;
        else if(s[0] == '.')
            return Solve(s[1..], i, groups, cache);
        else if(s[0] == '?')
            return Solve("." + s[1..], i, groups, cache) + Solve("#" + s[1..], i, groups, cache);
        else if(s[0] == '#')
        {
            if (groups.Length - i == 0)
                return 0;

            int group = groups[i];
            int maxPossibleGroupSize = s.TakeWhile(c => c == '#' || c == '?').Count();

            if (maxPossibleGroupSize < group) {
                return 0; 
            } else if (s.Length == group) {
                return Solve("", i + 1, groups, cache);
            } else if (s[group] == '#') {
                return 0; 
            } else {
                return Solve(s[(group + 1)..], i + 1, groups, cache);
            }   
        }
        else 
        {
            return long.MaxValue;
        }
    }
}