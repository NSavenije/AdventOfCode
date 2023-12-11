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
        using StreamReader sr = new("src/Day11/11.in");
        string line;

        List<string> input = [];
        int counter = 0;
        List<int> rows = [];
        List<int> cols = [];
        while ((line = sr.ReadLine()) != null)
        {
            input.Add(line);
            if (line.All(c => c == '.'))
                rows.Add(counter);
            counter++;
        }
        List<string> transposed = TransposeRowsToColumns(input);
        List<string> result = [];
        counter = 0;

        foreach(string s in transposed)
        {
            result.Add(s);
            if (s.All(c => c == '.'))
                cols.Add(counter);
            counter++;
        }
        result = TransposeRowsToColumns(result);


        // System.IO.File.WriteAllLines("11.out", result);
        int[,] universe = new int[result[0].Length,result.Count];
        counter = 0;
        List<(int x,int y)> galaxies = [];
        for(int x = 0; x < universe.GetLength(0); x++)
        {
            for(int y = 0; y < universe.GetLength(1); y++)
            {
                if (result[y][x] == '#')
                {
                    galaxies.Add((x, y));
                    universe[x,y] = counter++;
                }
                else
                    universe[x,y] = result[y][x];
            }
        }

        long sum = 0;
        List<int> dists = [];
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
                dists.Add(dist);
                // Console.WriteLine($"({x1},{y1})-({x2},{y2}): {dist}({Math.Abs(x2 - x1)},{Math.Abs(y2 - y1)},{xs},{ys})");
            }
        }
        Console.WriteLine(string.Join(',',cols));
        Console.WriteLine(string.Join(',',rows));
        Console.WriteLine(sum);
        
    }

    static List<string> TransposeRowsToColumns(List<string> rows)
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
}