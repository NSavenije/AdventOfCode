#nullable disable

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;

static class Almanac{
    public static string Solve()
    {
        string filePath = "src/Day5/5.in";
        using StreamReader sr = new(filePath);
        string line;
        List<long> seeds = new();
        int lineIndex = 0;
        int mapIndex = -1;
        List<List<Map>> maps =  new();
        bool skip = false;
        while ((line = sr.ReadLine()) != null)
        {      
            if (skip)
            {
                skip = false;
                continue;
            }
            if (lineIndex == 0) 
            {
                // SOLVE A //
                seeds = line.Split(':')[1].Trim().Split(' ').Select(long.Parse).ToList();

                lineIndex++;
                continue;
            }
            // If line is empty, stop adding to map, skip next line, then start on new map.
            if (line == "")
            {
                skip = true;
                mapIndex++;
                maps.Add(new());
                maps[mapIndex].Add(new Map(0,0,0));
                continue;
            }
            maps[mapIndex].Add(new Map(line.Split(' ')));
        }
        

        foreach(List<Map> map in maps)
        {
            // Might want to preprocess
            seeds = seeds.Select(seed => seed + (map[InRange(seed,map)].Dest - map[InRange(seed,map)].Src)).ToList();
        }

        // Console.WriteLine( string.Join(", ", seeds));;
        return seeds.Min().ToString();
    }

    private static int InRange(long seed, List<Map> ranges)
    {
        for(int i = 1; i < ranges.Count; i++)
        {
            Map m = ranges[i];
            if(seed >=  m.Src && seed < m.Src + m.Sze)
            {
                return i;
            }
        }
        return 0;
    }

    private struct Map
    {
        public Map(long dest, long src, long size)
        {
            Dest = dest;
            Src = src;
            Sze = size;
        }

        public Map(string[] inp)
        {
            Dest = long.Parse(inp[0]);
            Src = long.Parse(inp[1]);
            Sze = long.Parse(inp[2]);
        }

        public long Dest { get; }
        public long Src { get; }
        public long Sze { get; }

        public override readonly string ToString() => $"({Dest}, {Src}, {Sze})";
    }
}