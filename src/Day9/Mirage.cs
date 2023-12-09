public static class Day9
{
    public static void Solve1()
    {
        List<List<int>> histories = ParseInput();
        List<int> results = [];
        foreach(List<int> seq in histories)
        {
            List<List<int>> seqs = BuildPyramid(seq);
            int depth = seqs.Count - 1;
            // Add the padding value and loop back to the start
            seqs[depth].Add(0);
            depth--;
            while(depth >= 0)
            {
                seqs[depth].Add(seqs[depth].Last() + seqs[depth + 1].Last());
                // Console.WriteLine(string.Join(',', seqs[depth]) + " <> " + string.Join(',', seqs[depth + 1])); 
                depth--;
            }
            int extrapolation = seqs[0].Last();
            results.Add(extrapolation);
        }
        // Console.WriteLine(string.Join(',',results));
        Console.WriteLine(results.Sum());
    }
    public static void Solve2()
    {
        List<List<int>> histories = ParseInput();
        List<int> results = [];
        foreach(List<int> seq in histories)
        {
            List<List<int>> seqs = BuildPyramid(seq);
            int depth = seqs.Count - 1;
            // Add the padding value and loop back to the start
            seqs[depth].Add(0);
            depth--;
            while(depth >= 0)
            {
                seqs[depth].Add(seqs[depth].First() - seqs[depth + 1].Last());
                // Console.WriteLine($"@dept {depth}: Me {string.Join(',', seqs[depth])}  parent {string.Join(',', seqs[depth + 1])}"); 
                depth--;
            }
            int extrapolation = seqs[0].Last();
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

    static List<List<int>> BuildPyramid(List<int> sequence)
    {
        List<List<int>> pyramid = [];
        pyramid.Add([]);
        pyramid[0] = sequence;
        int depth = 0;
        bool allZeroes = false;
        while(!allZeroes)
        {
            pyramid.Add(new List<int>());
            depth++;
            for(int i = 0; i < pyramid[depth - 1].Count - 1; i++)
            {
                //parent
                List<int> parent = pyramid[depth - 1];
                // Console.WriteLine(string.Join(',', parent));
                //compare all pairs
                pyramid[depth].Add(parent[i+1] - parent[i]);
            }
            // Console.WriteLine($"@depth {depth}: {string.Join(',', pyramid[depth])}");
            allZeroes = pyramid[depth].All(c => c == 0);
        }
        return pyramid;
    }
}