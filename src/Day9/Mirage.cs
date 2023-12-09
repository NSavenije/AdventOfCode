public static class Day9
{
    public static void Solve1()
    {
        
        List<List<int>> histories = ParseInput();
        List<int> results = [];
        foreach(List<int> seq in histories)
        {
            int extrapolation = - 1;
            bool allZeroes = false;
            List<List<int>> seqs = [];
            seqs.Add([]);
            seqs[0] = seq;
            int depth = 0;
            while(!allZeroes)
            {
                seqs.Add(new List<int>());
                depth++;
                for(int i = 0; i < seqs[depth - 1].Count - 1; i++)
                {
                    //parent
                    List<int> parent = seqs[depth - 1];
                    // Console.WriteLine(string.Join(',', parent));
                    //compare all pairs
                    seqs[depth].Add(parent[i+1] - parent[i]);
                }
                // Console.WriteLine($"@depth {depth}: {string.Join(',', seqs[depth])}");
                allZeroes = seqs[depth].All(c => c == 0);
            }
            // All zeroes
            // Add the padding value and loop back to the start
            seqs[depth].Add(0);
            depth--;
            while(depth >= 0)
            {
                List<int> parent = seqs[depth + 1];
                seqs[depth].Add(seqs[depth].Last() + parent.Last());
                // Console.WriteLine(string.Join(',', seqs[depth]) + " <> " + string.Join(',', seqs[depth + 1])); 
                depth--;
            }
            extrapolation = seqs[0].Last();
            results.Add(extrapolation);
        }
        Console.WriteLine(string.Join(',',results));
        Console.WriteLine(results.Sum());
    }
    public static void Solve2()
    {
                List<List<int>> histories = ParseInput();
        List<int> results = [];
        foreach(List<int> seq in histories)
        {
            int extrapolation = - 1;
            bool allZeroes = false;
            List<List<int>> seqs = [];
            seqs.Add([]);
            seqs[0] = seq;
            int depth = 0;
            while(!allZeroes)
            {
                seqs.Add(new List<int>());
                depth++;
                for(int i = 0; i < seqs[depth - 1].Count - 1; i++)
                {
                    //parent
                    List<int> parent = seqs[depth - 1];
                    // Console.WriteLine(string.Join(',', parent));
                    //compare all pairs
                    seqs[depth].Add(parent[i+1] - parent[i]);
                }
                // Console.WriteLine($"@depth {depth}: {string.Join(',', seqs[depth])}");
                allZeroes = seqs[depth].All(c => c == 0);
            }
            // All zeroes
            // Add the padding value and loop back to the start
            seqs[depth].Add(0);
            depth--;
            while(depth >= 0)
            {
                List<int> parent = seqs[depth + 1];

                int toAdd = seqs[depth].First() + parent.First();
                // Console.WriteLine("ToAdd " + toAdd);
                Console.WriteLine($"@dept {depth}: Me {string.Join(',', seqs[depth])}  parent {string.Join(',', seqs[depth + 1])}"); 
                seqs[depth].Add(seqs[depth].First() - parent.Last());
                
                depth--;
            }
            extrapolation = seqs[0].Last();
            results.Add(extrapolation);
        }
        Console.WriteLine(string.Join(',',results));
        Console.WriteLine(results.Sum());
    }
    static List<List<int>> ParseInput()
    {
        string filePath = "src/Day9/9.in"; 
        using StreamReader sr = new(filePath);
        string line;
        List<List<int>> res = [];
        while ((line = sr.ReadLine()) != null)
        {
            res.Add(line.Split(" ").Select(int.Parse).ToList());
        }
        return res;
    }
}