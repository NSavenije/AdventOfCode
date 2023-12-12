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
            List<int> groups = s.Split(' ')[1].Trim().Split(',').Select(int.Parse).ToList();            
            sum += Solve(records, 0, groups);
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
            List<int> groups = string.Join(',',Enumerable.Repeat(grps, 5)).Split(',').Select(int.Parse).ToList();
            Console.WriteLine(records + " " + groups);
            sum += Solve(records, 0, groups);
        });
        // sum += CountPermutations(input[0]);
        Console.WriteLine(sum);
    }

    static long Solve(string s, int groupIndex, List<int> groups)
    {
        if (s == "")
        {
            // End on .
            if (groupIndex == 0 && groups.Count == 0)
                return 1;
            // End on #
            if (groups.Count == 1 && groupIndex > 0 && groupIndex == groups[0])
                return 1;
            
            return 0;
        }

        // Max size of group
        int possibleMore = s.Count(ch => ch == '#' || ch == '?');

        // If I am in a group and can't fill the rest of the group
        if (groupIndex > 0 && possibleMore + groupIndex < groups.Sum())
            return 0;
        // If Im not in a group
        if (groupIndex == 0 && possibleMore < groups.Sum())
            return 0;
        // In group while not in group
        if (groupIndex > 0 && groups.Count == 0)
            return 0;

        long poss = 0;
        if (s[0] == '.' && groupIndex > 0 && groupIndex != groups[0])
            return 0;
        if (s[0] == '.' && groupIndex > 0)
            poss += Solve(s[1..], 0, groups.GetRange(1, groups.Count - 1));
        if (s[0] == '?' && groupIndex > 0 && groupIndex == groups[0])
            poss += Solve(s[1..], 0, groups.GetRange(1, groups.Count - 1));
        if ((s[0] == '#' || s[0] == '?') && groupIndex > 0)
            poss += Solve(s.Substring(1), groupIndex == 0 ? 0 : groupIndex + 1, groups);
        if ((s[0] == '?' || s[0] == '#') && groupIndex == 0)
            poss += Solve(s[1..], 1, groups);
        if ((s[0] == '?' || s[0] == '.') && groupIndex == 0)
            poss += Solve(s[1..], 0, groups);
        
        return poss;
    }

}