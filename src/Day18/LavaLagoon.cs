using System.Drawing;
using System.Text;
#nullable disable
public static class Day18
{
    public static void Solve1()
    {
        string filePath = "src/Day18/18.in";
        Dictionary<(int, int), (bool, List<Color>)> lgn = ParseInput(filePath);
        Dictionary<(int, int), (bool, List<Color>)> lagoon = [];
        int minX = 0, minY = 0;
        foreach((int x, int y) in lgn.Keys)
        {
            // Console.WriteLine(x + " " + y);
            minX = Math.Min(x,minX);
            minY = Math.Min(y,minY);
        }
        Console.WriteLine(minY);
        foreach (var item in lgn)
        {
            lagoon.Add((item.Key.Item1 + (-1 * minX), item.Key.Item2 + (-1 * minY)), item.Value);
        }


        // Console.WriteLine(lagoon.Count); correct!
        int maxX = 0, maxY = 0;
        foreach((int x, int y) in lagoon.Keys)
        {
            maxX = Math.Max(x,maxX);
            maxY = Math.Max(y,maxY);
        }
        // Console.WriteLine($"({maxX},{maxY})");
        int count = 0;
        bool inside = false;
        int[,] ints = new int[maxX + 1, maxY + 1];
        for(int y = 0; y <= maxY; y++)
        {
            inside = false;
            for(int x = 0; x <= maxX; x++)
            {
                if(lagoon.TryGetValue((x,y), out var output))
                {

                    // If there is a kink in the cable, flip
                    // If there is a horizontal of 1 thick, flip
                    // If there is a cup or hat, not flip
                    // Only flip @end of range
                    if (lagoon.TryGetValue((x + 1, y), out _))
                    {

                        // Horizontal line continues
                    }
                    else
                    {
                        inside = output.Item1 ? inside : !inside;
                    }
                    ints[x,y] = 1;
                    count++;
                }
                else if (inside)
                {
                    ints[x,y] = 1;
                    count++;
                }
                else
                {
                    ints[x,y] = 0;
                }
            }
        }
        List<string> outpt = [];
        for(int y = 0; y <= maxY; y ++)
        {
            StringBuilder sb = new();
            for(int x = 0; x <= maxX; x ++)
            {
                sb.Append(ints[x,y] == 0 ? '.' : '#');
            }
            outpt.Add(sb.ToString());
        }
        File.WriteAllLines("src/Day18/18-filled.out", outpt);
        Console.WriteLine(count);
    }

    static Dictionary<(int, int), (bool, List<Color>)> ParseInput(string filePath)
    {
        Dictionary<string, (int,int)> dirs = [];
        dirs.Add("R",(1, 0));
        dirs.Add("D",(0, 1));
        dirs.Add("L",(-1,0));
        dirs.Add("U",(0,-1));

        Dictionary<(int, int), (bool, List<Color>)> lagoon = [];
        StreamReader sr = new(filePath);
        string line;
        List<((int,int) dir, int dist, Color color)> lines = [];
        while ((line = sr.ReadLine()) != null)
        {
            var parts = line.Split(" ");
            (int, int) dir = dirs[parts[0].Trim()];
            int dist = int.Parse(parts[1]);
            Color color = ParseColor(parts[2]);
            lines.Add((dir,dist,color));
        }

        int x = 0, y = 0;
        lagoon.Add((x,y),(false,[]));
        int lastDir = 0;
        for(int j = 0; j < lines.Count; j++)
        {
            ((int x,int y) dir, int dist, Color color) = lines[j];
            for(int i = 0; i < dist; i++)
            {
                x += dir.x;
                y += dir.y;
                // Console.WriteLine($"x{x} y{y}");
                if (lagoon.TryGetValue((x,y), out _))
                    lagoon[(x,y)].Item2.Add(color);
                else
                {
                    //Cup or Hat? L---J or F---7 U(L|R)D || D(L|R)U
                    if (i == dist - 1 && lines.Count > j + 1 && (lastDir == (lines[j + 1].dir.Item2 * -1)) && dir.y == 0 && lastDir != 0)
                    {
                        lagoon.Add((x,y),(true,[color]));
                    }
                    else
                    {
                        lagoon.Add((x,y),(false,[color]));
                    }

                }
            }
            lastDir = dir.y;
        }
        return lagoon;
    }

    static Color ParseColor(string input)
    {
        input = input.Replace("(", "").Replace(")", "").Replace("#", "");
        int red = Convert.ToInt32(input.Substring(0, 2), 16);
        int green = Convert.ToInt32(input.Substring(2, 2), 16);
        int blue = Convert.ToInt32(input.Substring(4, 2), 16);
        return Color.FromArgb(red, green, blue);
    }
}