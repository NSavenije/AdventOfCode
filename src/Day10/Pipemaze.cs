#nullable disable

using System.Diagnostics.Metrics;
using System.Net.Security;

public static class Day10
{
    public static void Solve1()
    {
        string[] input = ParseInput();
        Coord root = GetRoot(input);
        Coord curCoord = GetStart(root, input);
        List<Coord> path = GeneratePath([root,curCoord], input);
        Console.WriteLine((path.Count - 1) / 2);
    }

    public static void Solve2()
    {
        string[] input = ParseInput();
        Coord root = GetRoot(input);
        Coord curCoord = GetStart(root, input);
        List<Coord> path = GeneratePath([root,curCoord], input);

        // Convert string[] to char[][]
        char[][] inp = new char[input.Length][];
        for(int row = 0; row < input.Length; row++){
            inp[row] = input[row].ToCharArray();
        }

        // Replace 'S' with actual symbol.
        var p1 = path[1];
        var pn = path[^3];
        foreach(char c in "F-7L|J")
        {
            var ts = Targets(root, c);
            if (ts == (p1,pn) || ts == (pn,p1))
            {
                inp[root.Row][root.Col] = c;
            }
        }
        
        // Scale up maze
        List<Coord> lp = [];
        int rows = inp.Length;
        int cols = inp[0].Length;

        char[][] maze = new char[rows * 2][];

        for (int i = 0; i < rows * 2; i+=2)
        {
            maze[i] = new char[cols * 2];
            maze[i + 1] = new char[cols * 2];
            for (int j = 0; j < cols * 2; j+=2)
            {
                char c = inp[i / 2][j / 2];
                maze[i][j] = c;
                maze[i + 1][j + 1] = '.';
                if (path.Contains(new(i / 2, j / 2)))
                {
                    lp.Add(new(i, j));
                }
                
                switch (c)
                {
                    case '.':
                    case 'J':
                        maze[i + 0][j + 1] = '.';
                        maze[i + 1][j + 0] = '.';           
                        break;
                    case '-':
                    case 'L':
                        maze[i + 0][j + 1] = '-';
                        maze[i + 1][j + 0] = '.';
                        if (path.Contains(new(i / 2, j / 2)))
                        {
                            lp.Add(new(i, j + 1));
                        }
                        break;
                    case '|':
                    case '7':
                        maze[i + 0][j + 1] = '.';
                        maze[i + 1][j + 0] = '|';
                        if (path.Contains(new(i / 2, j / 2)))
                        {
                            lp.Add(new(i + 1, j));
                        }
                        break;
                    case 'F':
                        maze[i + 0][j + 1] = '-';
                        maze[i + 1][j + 0] = '|';
                        if (path.Contains(new(i / 2, j / 2)))
                        {
                            lp.Add(new(i, j + 1));
                            lp.Add(new(i + 1, j));
                        }
                        break;
                }  
            }
        }

        // Flood Fill large maze
        Stack<(int,int)> stack = new();
        List<(int,int)> visited = [];
        stack.Push(new(0,0));
        
        while (stack.Count > 0)
        {
            (int r, int c) = stack.Pop();
            if (!visited.Contains((r,c)))
            {
                visited.Add((r,c));
                if (InBounds(r,c, maze.Length, maze[0].Length) && (!lp.Contains(new (r,c))))
                {
                    maze[r][c] = '0';
                    stack.Push((r,c-1));
                    stack.Push((r,c+1));
                    stack.Push((r-1,c));
                    stack.Push((r+1,c));
                }
            }
        }

        // Mark the unvisited nodes with '1'
        for(int row = 0; row < maze.Length; row++){
            for(int col = 0; col < maze[0].Length; col++)
            {
                if (!lp.Contains(new (row,col)) && maze[row][col] != '0')
                {
                    maze[row][col] = '1';
                }
            }
        }

        // Count all '1'. Note, we only count the top lefts of the 2x2 cells.
        // This saves us from having to scale down again.
        int count = 0;
        for(int row = 0; row < inp.Length; row++){
            for(int col = 0; col < inp[0].Length; col++)
            {
                inp[row][col] = maze[row * 2][col * 2];
                if (inp[row][col] == '1')
                {
                    count++;
                }
            }
        }

        // Write output.
        List<string> output = ConvertCharArrayToList(inp);
        File.WriteAllLines("src/Day10/10.out", output);
        Console.WriteLine(count);
        
    }

    static List<string> ConvertCharArrayToList(char[][] charArray)
    {
        List<string> resultList = new List<string>();

        foreach (var row in charArray)
        {
            string rowString = new string(row);
            resultList.Add(rowString);
        }

        return resultList;
    }

    static string[] ParseInput()
    {
        string filePath = "src/Day10/10.in"; 
        using StreamReader sr = new(filePath);
        string line;
        List<string> res = [];
        while ((line = sr.ReadLine()) != null)
        {
            res.Add(line);
        }
        return res.ToArray();
    }

    static bool InBounds(int r,int c, int maxR, int maxC)
    {
        if (r < 0 || c < 0 || r >= maxR || c >= maxC)
            return false;
        return true;
    }

    static Coord GetRoot(string[] field)
    {  
        for(int row = 0; row < field.Length; row++){
            if (field[row].Contains('S'))
            {
                int col = field[row].Select((s, i) => new { Str = s, Index = i })
                    .Where(x => x.Str == 'S')
                    .Select(x => x.Index).First();
                return new Coord(row,col);
            }
        }
        Console.WriteLine("FAIL");
        return new Coord(-1,-1);
    }

    static Coord GetStart(Coord root, string[] input)
    {
        for(int row = root.Row - 1; row <= root.Row + 1; row++)
        {
            if (row > -1 && row < input[0].Length)
            {
                for(int col = root.Col - 1; col <= root.Col + 1; col++)
                {
                    if (col > -1 && col < input.Length)
                    {
                        char cur = input[row][col];
                        var (t1,t2) = Targets(new Coord(row, col), cur); 
                        if ((t1.Row == root.Row && t1.Col == root.Col) ||
                            (t2.Row == root.Row && t2.Col == root.Col))
                            {
                                // Console.WriteLine($"{cur}({row},{col}): {targets}");
                                return new(row,col);
                            }
                    }
                }
            }
        }
        return new(-1,-1);
    }

    static List<Coord> GeneratePath(List<Coord> path, string[] input)
    {
        bool loop = false;
        while(!loop)
        {
            Coord cc = path.Last();   
            // Console.WriteLine(curCoord);
            char cur = input[cc.Row][cc.Col];
            if (cc == path.First())
            {
                loop = true;
            }
            var (t1, t2) = Targets(cc, cur);
            // Console.WriteLine("cur: " + cc + " targets: " + (t1,t2) + " prev: " + path[path.Count-2]);
            if (t1 == path[path.Count - 2])
            {  
                
                path.Add(t2);
                // curCoord = t2;
            }
            else
            {
                path.Add(t1);
                // curCoord = t1;
            }
            // Console.WriteLine(path.Count);
        }
        return path;
    }

    static (Coord t1,Coord t2) Targets(Coord orig, char type)
    {
        (Coord,Coord) res;
        switch(type){
            case '-':
                res.Item1 = new Coord(orig.Row, orig.Col - 1);
                res.Item2 = new Coord(orig.Row, orig.Col + 1);
                break;
            case '|':
                res.Item1 = new Coord(orig.Row + 1, orig.Col);
                res.Item2 = new Coord(orig.Row - 1, orig.Col);
                break;
            case 'L':
                res.Item1 = new Coord(orig.Row -1, orig.Col);
                res.Item2 = new Coord(orig.Row, orig.Col + 1);
                break;
            case 'J':
                res.Item1 = new Coord(orig.Row - 1, orig.Col);
                res.Item2 = new Coord(orig.Row, orig.Col - 1);
                break;
            case '7':
                res.Item1 = new Coord(orig.Row + 1, orig.Col);
                res.Item2 = new Coord(orig.Row, orig.Col - 1);
                break;
            case 'F':
                res.Item1 = new Coord(orig.Row + 1, orig.Col);
                res.Item2 = new Coord(orig.Row, orig.Col + 1);
                break;
            case 'S':
            case '.':
            default:
                res.Item1 = new Coord(-1, -1);
                res.Item2 = new Coord(-1, -1);
                break;
               
        }
        return res;
    }
    
}

record struct Coord(int Row, int Col)
{
    public int Row { get; } = Row;
    public int Col { get; } = Col;

    public override string ToString() => $"({Row}, {Col})";
}