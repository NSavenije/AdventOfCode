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

        int counter = 0;
        List<int> rows = [];
        List<(int x,int y)> galaxies = [];
        foreach(string s in input)
        {
            if (s.All(c => c == '.'))
                rows.Add(counter);
            else if (s.Contains('#'))
                galaxies.AddRange(s.AllIndexesOf("#").Select(x => (counter,x)));
            counter++;
        }
        
        List<string> transposed = Transpose(input);
        List<int> cols = transposed
            .Select((s, index) => new { Index = index, String = s })
            .Where(item => item.String.All(c => c == '.'))
            .Select(item => item.Index)
            .ToList();
        
        long sum = 0;
        foreach((int x1,int y1) in galaxies)
        {
            foreach((int x2,int y2) in galaxies)
            {
                if (x2 < x1 || ( x2 == x1 && y2 <= y1))
                    continue;

                int low, high;
                low = Math.Min(x1, x2); high = Math.Max(x1, x2);
                int xs = Enumerable.Range(low, high - low).Intersect(cols).Count() * (expansion - 1);
                low = Math.Min(y1, y2); high = Math.Max(y1, y2);
                int ys = Enumerable.Range(low, high - low).Intersect(rows).Count() * (expansion - 1);
                int dist = Math.Abs(x2 - x1) + Math.Abs(y2 - y1) + xs + ys;
                sum += dist;
                // Console.WriteLine($"({x1},{y1})-({x2},{y2}): {dist}({Math.Abs(x2 - x1)},{Math.Abs(y2 - y1)},{xs},{ys})");
            }
        }
        
    }

    static List<string> Transpose(List<string> rows)
    {
        List<string> result = [];

        for (int columnIndex = 0; columnIndex < rows[0].Length; columnIndex++)
        {
            StringBuilder columnBuilder = new();
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                columnBuilder.Append(rows[rowIndex][columnIndex]);
            }
            result.Add(columnBuilder.ToString());
        }
        return result;
    }

    static IEnumerable<int> AllIndexesOf(this string c, string searchstring)
    {
        int minIndex = c.IndexOf(searchstring);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = c.IndexOf(searchstring, minIndex + searchstring.Length);
        }
    }
}