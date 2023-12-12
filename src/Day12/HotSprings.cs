using System.Data.SqlTypes;
using System.Diagnostics;
using System.Security;
#nullable disable
public static class Day12
{
    public static void Solve1()
    {
        List<string> input = File.ReadAllLines("src/Day12/12.in").ToList();
        long sum = 0;

        
        foreach(string s in input)
        {
            string records = s.Split(' ')[0];
            int[] groups = s.Split(' ')[1].Trim().Split(',').Select(int.Parse).ToArray();            
            sum += Solve(records, groups, []);
        }
        // sum += CountPermutations(input[0]);
        Console.WriteLine(sum);
    }

    public static void Solve2()
    {
        List<string> input = File.ReadAllLines("src/Day12/12.in").ToList();
        long sum = 0;

        Parallel.For(0, input.Count, i => 
        {
            string s = input[i];
            string records = s.Split(' ')[0];
            records = string.Join('?',Enumerable.Repeat(records, 5));
            
            string grps = s.Split(' ')[1].Trim();
            int[] groups = string.Join(',',Enumerable.Repeat(grps, 5)).Split(',').Select(int.Parse).ToArray();
            
            var funcWatch = Stopwatch.StartNew();
            long res = Solve(records, groups, []);
            sum += res;
            Console.WriteLine("sum: " + sum + ", res: " + res + " | " + records + " " + string.Join(',',groups));
        });

        Console.WriteLine(sum);
    }

    static long Solve(string s, int[] groups, Dictionary<(string, int[]), long> cache)
    {
        if(!cache.ContainsKey((s, groups)))
        {
            long res = Compute(s, groups, cache);
            // Console.WriteLine($"CacheHit: {s}|{string.Join(',',groups)}: {res}");
            cache.Add((s,groups), res);
        }
        // Console.WriteLine($"CacheHit: {s}|{string.Join(',',groups)}: {perms}");
        return cache[(s,groups)];
    }

    static long Compute(string s, int[] groups, Dictionary<(string,int[]),long> cache)
    {
        // Solve for empty
        if (s == "")
        {
            int result = groups.Length == 0 ? 1 : 0;
            return result;
        }
        else if (s[0] == '.')
        {
            return Solve(s[1..], groups, cache);
        }
        else if (s[0] == '#')
        {
            if (groups.Length == 0)
            {
                return 0;
            }
            int group = groups[0];
            int[] groupsTail = groups.SubArray(1, groups.Length - 1);
            int maxPossibleGroupSize = s.TakeWhile(s => s == '#' || s == '?').Count();
            if (maxPossibleGroupSize < group)
            {   
                return 0;
            }
            if (s.Length == group)
            {
                return Solve("", groupsTail, cache);
            }
            if (s[group] == '#')
            {
                return 0;
            }
            return Solve(s[(group + 1)..], groupsTail, cache);
        }
        else if (s[0] == '?')
        {
            return Solve('#' + s[1..], groups, cache) + Solve('.' + s[1..], groups, cache);
        }
        else
        {
            Console.WriteLine("NOT POSSIBLE");
            return long.MaxValue;
        }
    }

    static T[] SubArray<T>(this T[] data, int index, int length)
    {
        if (length < 0)
            return [];
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }   
}