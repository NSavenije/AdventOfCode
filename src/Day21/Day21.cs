public static class Day21
{
    public static void Solve1()
    {
        var input = ParseInput("src/Day21/21.in");
        Console.WriteLine(Solve(input, 64));
    }

    public static void Solve2()
    {
        var input = ParseInput("src/Day21/21b.in");   
        Console.WriteLine(Solve(input, 64));
    }

    static long Solve(char[,] garden, int steps)
    {
        HashSet<(int x, int y)> coords = [];
        coords.Add(FindStart(garden));
        HashSet<(int x,int y)> grdn = [];
        for(int x = 0; x < garden.GetLength(0); x++)
        {
            for(int y = 0; y < garden.GetLength(1); y++)
            {
                if (garden[x,y] == 'S' || garden[x,y] == '.')
                {
                    grdn.Add((x,y));
                }
            }
        }
        return DoSteps(coords, grdn, 0, steps);
    }

    static long DoSteps(HashSet<(int x,int y)> coords, HashSet<(int x, int y)> garden, int step, int steps)
    {
        if (step == steps)
            return coords.Count;
        
        HashSet<(int,int)> newCoords = [];
        foreach(var (x, y) in coords)
        {
            //Right
            if (garden.Contains((x + 1, y)))
            {
                newCoords.Add((x + 1,y));
            }
             //Left
            if (garden.Contains((x - 1, y)))
            {
                newCoords.Add((x - 1,y));
            }
            //Down
            if (garden.Contains((x, y + 1)))
            {
                newCoords.Add((x,y + 1));
            }
            //Right
            if (garden.Contains((x, y - 1)))
            {
                newCoords.Add((x,y - 1));
            }
        }
        Console.WriteLine(step + 1 + ": " + newCoords.Count);
        return DoSteps(newCoords,garden,step + 1, steps);
    }

    static (int,int) FindStart(char[,] input)
    {
        for(int x = 0; x < input.GetLength(0); x++)
        {
            for(int y = 0; y < input.GetLength(1); y++)
            {
                if (input[x,y] == 'S')
                    return (x,y);
            }
        }
        return(-1,-1);
    }

    static char[,] ParseInput(string filePath)
    {
        string[] strings = File.ReadAllLines(filePath);
        char[,] result = new char[strings[0].Length,strings.Length];
        for(int y = 0; y < strings.Length; y++)
            for(int x = 0; x < strings[0].Length; x++)
                result[x,y] = strings[x][y];
        return result;
    }
}