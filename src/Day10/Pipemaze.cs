#nullable disable

using System.Diagnostics.Metrics;
using System.Net.Security;

public static class Day10
{
    public static void Solve1()
    {
        string[] input = ParseInput();
        Coord root = GetRoot(input);
        // Console.WriteLine(root);

        Coord curCoord = new(-1,-1);

        for(int row = root.Row - 1; row <= root.Row + 1; row++)
        {
            if (row > -1 && row < input[0].Length)
            {
                for(int col = root.Col - 1; col <= root.Col + 1; col++)
                {
                    if (col > -1 && col < input.Length)
                    {
                        char cur = input[row][col];
                        var targets = Targets(new Coord(row, col), cur); 
                        
                        if ((targets.t1.Row == root.Row && targets.t1.Col == root.Col) ||
                            (targets.t2.Row == root.Row && targets.t2.Col == root.Col))
                            {
                                // Console.WriteLine($"{cur}({row},{col}): {targets}");
                                curCoord = new(row,col);
                                break;
                            }
                    }
                }
            }
        }
        // Console.WriteLine(cu);
        bool loop = false;
        List<Coord> path = [];
        path.Add(root);
        path.Add(curCoord);
        while(!loop)
        {
            Coord cc = curCoord;   
            char cur = input[cc.Row][cc.Col];
            if (cc == root)
            {
                loop = true;
            }
            var (t1, t2) = Targets(cc, cur);
            // Console.WriteLine("cur: " + cc + " targets: " + (t1,t2) + " prev: " + path[path.Count-2]);
            if (t1 == path[path.Count - 2])
            {  
                
                path.Add(t2);
                curCoord = t2;
            }
            else
            {
                path.Add(t1);
                curCoord = t1;
            }
            // Console.WriteLine(path.Count);
        }
        Console.WriteLine((path.Count - 1) / 2);
    }

    public static void Solve2()
    {
        string[] input = ParseInput();
        Coord root = GetRoot(input);
        // Console.WriteLine(root);

        Coord curCoord = new(-1,-1);

        for(int row = root.Row - 1; row <= root.Row + 1; row++)
        {
            
            if (row > -1 && row < input[0].Length)
            {
                
                for(int col = root.Col - 1; col <= root.Col + 1; col++)
                {
                    // Console.WriteLine($"row {row}, col {col}: cur {input[row][col]} {input.Length}");
                    if (col > -1 && col < input[0].Length)
                    {
                        char cur = input[row][col];
                        var (t1,t2) = Targets(new Coord(row, col), cur); 
                        // Console.WriteLine($"{cur}({row},{col}): {targets}");
                        if ((t1.Row == root.Row && t1.Col == root.Col) ||
                            (t2.Row == root.Row && t2.Col == root.Col))
                            {
                                // Console.WriteLine($"{cur}({row},{col}): {targets}");
                                curCoord = new(row,col);
                                break;
                            }
                    }
                }
            }
        }
        // Console.WriteLine(cu);
        bool loop = false;
        List<Coord> path = [];
        path.Add(root);
        path.Add(curCoord);
        while(!loop)
        {
            Coord cc = curCoord;   
            // Console.WriteLine(curCoord);
            char cur = input[cc.Row][cc.Col];
            if (cc == root)
            {
                loop = true;
            }
            var (t1, t2) = Targets(cc, cur);
            // Console.WriteLine("cur: " + cc + " targets: " + (t1,t2) + " prev: " + path[path.Count-2]);
            if (t1 == path[path.Count - 2])
            {  
                
                path.Add(t2);
                curCoord = t2;
            }
            else
            {
                path.Add(t1);
                curCoord = t1;
            }
            // Console.WriteLine(path.Count);
        }


        
        // Console.WriteLine((path.Count - 1) / 2);

        // If a bit is outside the loop make it 0
        // If a bit is enclosed make it 1
        // Count all 1's
        // A bit is enclosed if: it neighbours only ? and Main loop cells
        // A bit is not encloudes if: borders edge or borders not enclosed cell
        // Just keep looping
        // Each loop fill out more ? and 0
        // at the very end all remaining ? will become 1
        // first pass: if cell not part of main loop? if cell borders edge or 0? cell = o
        char[][] inp = new char[input.Length][];
        for(int row = 0; row < input.Length; row++){
            inp[row] = input[row].ToCharArray();
        }

        var p1 = path[1];
        var pn = path[path.Count - 3];
        foreach(char c in "F-7L|J")
        {
            var ts = Targets(root, c);
            if (ts == (p1,pn) || ts == (pn,p1))
            {
                inp[root.Row][root.Col] = c;
            }
        }
        
        // Outer perimeter determined. Now time to scale up
        char[][] maze = DoubleSizeOfCharArray(inp, path, out List<Coord> lp);//new char[inp.Length * 2][];
        

        static char[][] DoubleSizeOfCharArray(char[][] inp, List<Coord> path, out List<Coord> lp)
        {
            lp = [];
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
                    // Console.WriteLine($"{c} @ {i / 2},{j / 2}");
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

            return maze;
        }

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

        static bool InBounds(int r,int c, int maxR, int maxC)
        {
            if (r < 0 || c < 0 || r >= maxR || c >= maxC)
                return false;
            return true;
        }

        for(int row = 0; row < maze.Length; row++){
            for(int col = 0; col < maze[0].Length; col++)
            {
                if (!lp.Contains(new (row,col)) && maze[row][col] != '0')
                {
                    maze[row][col] = '1';
                }
            }
        }

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

        // List<string> output = ConvertCharArrayToList(inp);
        // foreach(string s in output)
        // {
        //     // Console.WriteLine(s.ToString());
        // }
        // System.IO.File.WriteAllLines("src/Day10/10.out", output);
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

public record struct Coord(int Row, int Col)
{
    public int Row { get; } = Row;
    public int Col { get; } = Col;

    public override string ToString() => $"({Row}, {Col})";
}