using System.Drawing;
using System.Text;
#nullable disable
public static class Day18
{
    public static void Solve1()
    {
        string filePath = "src/Day18/18.in";
        List<(long, long)> lagoon = ParseInput(filePath, hexInput: false, out long edge);
        Console.WriteLine("p1: " + CalculateCubicMeters(lagoon,edge));
    }

    public static void Solve2()
    {
        string filePath = "src/Day18/18.in";
        List<(long, long)> lagoon = ParseInput(filePath, hexInput: true, out long edge);
        Console.WriteLine("p2: " + CalculateCubicMeters(lagoon,edge));
    }

    static long CalculateCubicMeters(List<(long x,long y)> lagoon, long edge)
    {
        // Shoelace formula
        var combinations = lagoon.Zip(lagoon.Skip(1), (fst, snd) => (fst, snd));
        long result = 0;
        foreach(var (fst, snd) in combinations)
        {
            result += (fst.x * snd.y) - (fst.y * snd.x);
            // Console.WriteLine($"{fst} {snd}");
        }
        // Console.WriteLine("shoelace: " + result);
        // Console.WriteLine("edge: " + edge);

        // Picks theorem
        long area = (result / 2L) - (edge / 2L) + 1;
        // Console.WriteLine("Interior: " + area);
        return area + edge;
    }

    static List<(long,long)> ParseInput(string filePath, bool hexInput, out long edge)
    {
        Dictionary<string, (int,int)> dirs = [];
        dirs.Add("R",(1, 0));
        dirs.Add("D",(0, 1));
        dirs.Add("L",(-1,0));
        dirs.Add("U",(0,-1));
        Dictionary<char, (int,int)> intDirs = [];
        intDirs.Add('0',(1, 0));
        intDirs.Add('1',(0, 1));
        intDirs.Add('2',(-1,0));
        intDirs.Add('3',(0,-1));

        long x = 0;
        long y = 0;
        edge = 0;
        List<(long,long)> lagoon = [];
        lagoon.Add((0,0));

        StreamReader sr = new(filePath);
        string line;
        if (hexInput)
        {
            while ((line = sr.ReadLine()) != null)
            {
                string hex = line.Split(" ")[2].Replace("(", "").Replace(")", "").Replace("#", "");
                (int x, int y) dir = intDirs[hex[^1]];
                string hexDist = new(hex.Take(hex.Length - 1).ToArray());
                long dist = long.Parse(hexDist.ToUpper(), System.Globalization.NumberStyles.HexNumber);
                x += dist * dir.x;
                y += dist * dir.y;
                lagoon.Add((x, y));
                // Console.WriteLine($"{dir} {dist}");
                // Console.WriteLine($"{x} {y}");

                edge += dist;
            }
        }
        else
        {
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split(" ");
                (int x, int y) dir = dirs[parts[0]];
                long dist = long.Parse(parts[1]);
                x += dist * dir.x;
                y += dist * dir.y;
                lagoon.Add((x, y));
                // Console.WriteLine($"{dir} {dist}");
                // Console.WriteLine($"{x} {y}");

                edge += dist;
            }
        }

        return lagoon;
    }
}