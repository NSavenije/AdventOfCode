#nullable disable

static class Blmanac{
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
                continue;
            }
            maps[mapIndex].Add(new Map(line.Split(' ')));
        }

        List<Range> ranges = new();
        for (int i = 0; i < seeds.Count; i += 2)
            ranges.Add(new Range(seeds[i], seeds[i] + seeds[i + 1] - 1));

        foreach (List<Map> map in maps)
        {
            //Sort by 'from'
            map.Sort();

            List<Range> newRanges = new();
            foreach (Range r in ranges)
            {
                Range range = r;
                foreach (Map mapping in map)
                {
                    // Range from seeds origin, to the origin of the next range or the seed end.
                    // Say we have seed from [5..15] and mapping rule 1 from [10..20]
                    // This is the Part from [5..9] (not 10 ;-;)
                    if (range.From < mapping.From)
                    {
                        newRanges.Add(new Range(range.From, Math.Min(range.To, mapping.From - 1)));
                        // Everything before 10 is handled, so seed now goes [10..15]
                        range.From = mapping.From;
                        // If there's nothing left of this seed we can quit looking.
                        if (range.From > range.To)
                            break;
                    }

                    if (range.From <= mapping.To)
                    {
                        newRanges.Add(new Range(range.From + mapping.Jump, Math.Min(range.To, mapping.To) + mapping.Jump));
                        range.From = mapping.To + 1;
                        if (range.From > range.To)
                            break;
                    }
                }
                if (range.From <= range.To)
                    newRanges.Add(range);
            }
            ranges = newRanges;
            // Console.WriteLine(ranges.Count);
        }
        
        long result = ranges.Min(r => r.From);
        
        return result.ToString();
    }

    private struct Map : IComparable<Map>
    {
        public Map(long from, long to, long jump)
        {
            From = from;
            To = to;
            Jump = jump;
        }

        public Map(string[] inp)
        {
            From = long.Parse(inp[1]);
            To = long.Parse(inp[1]) + long.Parse(inp[2]) - 1;
            Jump = long.Parse(inp[0]) - long.Parse(inp[1]);
        }

        public long From { get; }
        public long To { get; }
        public long Jump { get; }

        public override readonly string ToString() => $"({From}, {To}, {Jump})";

        public int CompareTo(Map other)
        {
            if (other.From > From)
                return -1;
            else if (other.From < From)
                return 1;
            else
                return 0;
        }
    }

    private struct Range
    {
        public Range(long from, long to)
        {
            From = from;
            To = to;
        }

        public long From { get; set; }
        public long To { get; set; }

        public override readonly string ToString() => $"({From}, {To})";

        public int CompareTo(Map other)
        {
            if (other.From > From)
                return -1;
            else if (other.From < From)
                return 1;
            else
                return 0;
        }
    }
}