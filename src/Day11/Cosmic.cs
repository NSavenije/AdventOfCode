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
        foreach((int x1,int y1) in galaxies)
        {
            foreach((int x2,int y2) in galaxies)
            {
                if (x2 < x1 || ( x2 == x1 && y2 <= y1))
                    continue;

                int xBig, xSmall, yBig, ySmall;
                if (x1 >= x2)
                {
                    xBig = x1; xSmall = x2;
                }
                else{
                    xBig = x2; xSmall = x1;
                }
                if (y1 >= y2)
                {
                    yBig = y1; ySmall = y2;
                }
                else{
                    yBig = y2; ySmall = y1;
                }
                int dist = 0;
                for(int x = xSmall + 1; x < xBig; x++)
                {
                    if (cols.Contains(x))
                        dist += expansion - 1;
                }
                for(int y = ySmall + 1; y < yBig; y++)
                {
                    if (rows.Contains(y))
                        dist += expansion - 1;
                }
                var xs = Enumerable.Range(x1,x2).Intersect(cols).Count() * (expansion - 1);
                var ys = Enumerable.Range(y1,y2).Intersect(rows).Count() * (expansion - 1);
                dist += Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
                sum += dist;
            }
        }
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