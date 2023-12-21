public static class Day21
{
    public static void Solve1()
    {
        var input = ParseInput("src/Day21/21.in");
        Console.WriteLine(Solve(input, 64));
    }

    public static void Solve2()
    {
        char[,] input = ParseInput("src/Day21/21.in");   
        //Read input for square grids
        int gridSize = input.GetLength(0);
        if (input.GetLength(0) != input.GetLength(1))
            throw new NotImplementedException();

        int totalSteps = 26501365;

        long grids = totalSteps / gridSize; // 202300
        long rem = totalSteps % gridSize; // 65

        var results = new List<int>();
        HashSet<(int x, int y)> coords = [FindStart(input)];
        var steps = 0;
        HashSet<(int,int)> garden = [];
        for(int x = 0; x < input.GetLength(0); x++)
            for(int y = 0; y < input.GetLength(1); y++)
                if (input[x,y] == 'S' || input[x,y] == '.')
                    garden.Add((x,y));

        for (var n = 0; n < 3; n++)
        {
            while (steps < n * gridSize + rem)
            {
                HashSet<(int, int)> newCoords = [];
                int[] dx = [1,-1,0,0];
                int[] dy = [0,0,1,-1];
                foreach (var (x, y) in coords)
                {
                    for (int i = 0; i < dx.Length; i++)
                    {
                        (int newX, int newY) = (x + dx[i], y + dy[i]);

                        int newXRem = ((newX % 131) + 131) % 131;
                        int newYRem = ((newY % 131) + 131) % 131;

                        if (garden.Contains((newXRem, newYRem)))
                        {
                            newCoords.Add((newX, newY));
                        }
                    }
                }
                coords = newCoords;
                steps++;
            }
            results.Add(coords.Count);
        }

        //Quadratic function F(n) = ax^2 + bx + c
        // n := 0 -> F(0) = c
        // n := 1 -> F(1) = 1a + 1b + c
        // n := 2 -> F(2) = 4a + 2b + c
        int F0 = results[0];
        int F1 = results[1];
        int F2 = results[2];
        
        int c = F0;
        int oneAoneB = F1 - c;
        int fourAtwoB = F2 - c;
        int twoAtwoB = 2 * oneAoneB;
        int a = (fourAtwoB - twoAtwoB) / 2;
        int b = oneAoneB - a;
        Console.WriteLine(Quadratic(a,b,c,grids));
    }

    static long Quadratic(int a, int b ,int c, long n) =>
        a * (n * n) + b * n + c;

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
        int[] dx = [1,-1,0,0];
        int[] dy = [0,0,1,-1];
        if (step == steps)
            return coords.Count;
        
        HashSet<(int,int)> newCoords = [];
        foreach(var (x, y) in coords)
        {
            for(int i = 0; i < 4; i++)
            //Right
            if (garden.Contains((x + dx[i], y + dy[i])))
            {
                newCoords.Add((x + dx[i], y + dy[i]));
            }
        }
        // Console.WriteLine(step + 1 + ": " + newCoords.Count);
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