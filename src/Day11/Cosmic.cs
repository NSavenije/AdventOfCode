#nullable disable

using System.Text;

public static class Day11
{
    public static void Solve1()
    {
        Solve(2);
    }

    public static void Solve2()
    {
        Solve(1000000);
    }

    static void Solve(int expansion = 2)
    {
        List<string> input = File.ReadAllLines("src/Day11/11.in").ToList();
        List<int> rows = FindEmptyRows(input);
        List<int> cols = FindEmptyRows(Transpose(input));
        List<(int r,int c)> galaxies = FindGalaxies(input);

        long sum = 0;
        foreach((int x1,int y1) in galaxies)
        {
            foreach((int x2,int y2) in galaxies)
            {
                if (x2 < x1 || ( x2 == x1 && y2 <= y1))
                    continue;

                int xs = Enumerable.Range(x1, x2 - x1).Intersect(rows).Count() * (expansion - 1);
                int low = Math.Min(y1, y2); 
                int high = Math.Max(y1, y2);
                int ys = Enumerable.Range(low, high - low).Intersect(cols).Count() * (expansion - 1);
                int dist = Math.Abs(x2 - x1) + Math.Abs(y2 - y1) + xs + ys;
                sum += dist;
                // Console.WriteLine($"({x1},{y1})-({x2},{y2}): {dist}({Math.Abs(x2 - x1)},{Math.Abs(y2 - y1)},{xs},{ys})");
            }
        }
        Console.WriteLine(sum);
    }

    static List<string> Transpose(List<string> rows) =>
        Enumerable.Range(0,rows[0].Length).Select(i => string.Concat(rows.Select(row => row[i]))).ToList();

    static List<int> FindEmptyRows(List<string> input)
    {
        List<int> rows = [];
        for(int i = 0; i < input.Count; i++)
        {
            if (input[i].All(c => c == '.'))
                rows.Add(i);   
        }
        return rows;
    }
    
    static List<(int, int)> FindGalaxies(List<string> input) =>
        input.SelectMany((s, row) => s.AllIndexesOf('#').Select(col => (row, col))).ToList();

    public static List<int> AllIndexesOf(this string str, char value) =>
        Enumerable.Range(0, str.Length).Where(i => str[i] == value).ToList();
}
