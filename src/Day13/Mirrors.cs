public static class Day13
{
    public static void Solve1() => Solve(false);
    public static void Solve2() => Solve(true);

    static void Solve(bool smudge)
    {
        List<List<string>> input = ParseInput("src/Day13/13.in");
        long sum = input.Select(ss => FindMirrorValues(ss,smudge)).Sum();
        Console.WriteLine(sum);
    }

    static long FindMirrorValues(List<string> input, bool smudge) =>
        FindMirrorVs(input, smudge, false) + FindMirrorVs(Transpose(input), smudge, true);

    static long FindMirrorVs(List<string> input, bool smudge, bool transposed)
    {
        for(int i = 1; i < input.Count; i++)
            if(CompareString(input[i], input[i-1]) <= (smudge ? 1 : 0) && TestMirror(input, i, smudge))
                return transposed ? i : i * 100;
        return 0;
    }

    static int CompareString(string a, string b) =>
        a.Zip(b, (x, y) => x == y).Count(z => !z);

    static bool TestMirror(List<string> ss, int mirrorIndex, bool smudge)
    {
        // Console.WriteLine(string.Join('\n', ss));
        int smudges = CompareString(ss[mirrorIndex - 1], ss[mirrorIndex]);
        if (smudge && smudges > 1)
            return false;

        for(int i = Math.Max(0, 2 * mirrorIndex - ss.Count); i < mirrorIndex - 1; i++)
        {
            smudges += CompareString(ss[i], ss[2 * mirrorIndex - 1 - i]);
            if (smudges > (smudge ? 1 : 0))
                return false;
        }
        return !smudge || smudges == 1;
    }
    
    static List<string> Transpose(List<string> rows) =>
        Enumerable.Range(0,rows[0].Length).Select(i => string.Concat(rows.Select(row => row[i]))).ToList();

    static List<List<string>> ParseInput(string filePath) =>
        File.ReadAllLines(filePath).Aggregate(new List<List<string>> { new() }, (acc, s) =>
        {
            if (s == "") 
                acc.Add([]);
            else 
                acc.Last().Add(s);
            return acc;
        });
}