using System.Text;

public static class Day16
{
    public static void Solve1()
    {
        char[,] maze = ParseInput("src/Day16/16.in");
        int xs = maze.GetLength(0), ys = maze.GetLength(1);
        HashSet<(int,int)>[,] energy = BuilldEmptyEnergize(xs, ys);

        //Start top left, going Right
        (int x, int y) dir = (1,0);
        int x = 0, y = 0;
        Laser laser = new(x,y,dir);
        RunLaser(laser);
        
        int score = 0;
        for(int ity = 0; ity < ys; ity++)
        {
            StringBuilder sb = new();
            for(int itx = 0; itx < xs; itx++)
            {
                if (energy[itx,ity].Count > 0)
                    score++;

                // sb.Append(energy[itx,ity].Count > 0 ? '#' : '.');
                sb.Append(maze[itx,ity]);

            }
            Console.WriteLine(sb.ToString());
        }

        Console.WriteLine(score);
        
        void RunLaser(Laser l)
        {
            // Console.WriteLine($"{l}");
            if (energy[l.X,l.Y].Contains(l.Dir))
            {
                // Console.WriteLine($"HI");

                return;

            }
            energy[l.X,l.Y].Add(l.Dir);

            switch (maze[l.X,l.Y])
            {
                case '/':
                    if (l.Dir == (1,0)) l.Dir = (0,-1);
                    else if (l.Dir == (-1,0)) l.Dir = (0,1);
                    else if (l.Dir == (0,1)) l.Dir = (-1,0);
                    else if (l.Dir == (0,-1)) l.Dir = (1,0);
                    break;
                case '\\':
                    if (l.Dir == (1,0)) l.Dir = (0,1);
                    else if (l.Dir == (-1,0)) l.Dir = (0,-1);
                    else if (l.Dir == (0,1)) l.Dir = (1,0);
                    else if (l.Dir == (0,-1)) l.Dir = (-1,0);
                    break;
                case '-':
                    if (l.Dir.x == 0) // Up or Down
                    {
                        l.Dir = (-1,0);
                        if (l.X < xs - 1)
                            RunLaser(new(l.X + 1, l.Y, (1,0)));
                    }
                    break;
                case '|':
                    if (l.Dir.y == 0) // Left or Right
                    {
                        l.Dir = (0,-1);
                        if (l.Y < ys - 1)
                            RunLaser(new(l.X, l.Y + 1, (0,1)));
                    }
                    break;
                default:
                    maze[l.X,l.Y] = '#';
                    break;
            }
            l.X += l.Dir.x;
            l.Y += l.Dir.y;
            if (l.X >= xs || l.X < 0 || l.Y >= ys || l.Y < 0)
                return;
            RunLaser(l);
        }  
    }
    public static void Solve2()
    {
        char[,] maze = ParseInput("src/Day16/16.in");
        int xs = maze.GetLength(0), ys = maze.GetLength(1);
        int maxScore = 0;
        
        int score = 0;
        for(int itx = 0; itx < xs; itx++)
        {
            score = DoIt(xs,ys,itx,0,(0,1),maze);
            maxScore = Math.Max(score, maxScore);
            score = DoIt(xs,ys,itx,ys-1,(0,-1),maze);
            maxScore = Math.Max(score, maxScore);
        }
        for(int ity = 0; ity < xs; ity++)
        {
            score = DoIt(xs,ys,0,ity,(1,0),maze);
            maxScore = Math.Max(score, maxScore);
            score = DoIt(xs,ys,xs-1,0,(-1,0),maze);
            maxScore = Math.Max(score, maxScore);
        }
        Console.WriteLine(maxScore);
    }

    static int DoIt(int xs, int ys, int x, int y, (int x, int y) dir, char[,] maze)
    {
        HashSet<(int,int)>[,] energy = BuilldEmptyEnergize(xs, ys);
        //Start top, going Down
        Laser laser = new(x,y,dir);
        RunLaser(laser);
        
        int score = 0;
        for(int iy = 0; iy < ys; iy++)
        {
            for(int ix = 0; ix < xs; ix++)
            {
                if (energy[ix,iy].Count > 0)
                    score++;

            }
        }

        return score;

        void RunLaser(Laser l)
        {
            // Console.WriteLine($"{l}");
            if (energy[l.X,l.Y].Contains(l.Dir))
            {
                // Console.WriteLine($"HI");

                return;

            }
            energy[l.X,l.Y].Add(l.Dir);

            switch (maze[l.X,l.Y])
            {
                case '/':
                    if (l.Dir == (1,0)) l.Dir = (0,-1);
                    else if (l.Dir == (-1,0)) l.Dir = (0,1);
                    else if (l.Dir == (0,1)) l.Dir = (-1,0);
                    else if (l.Dir == (0,-1)) l.Dir = (1,0);
                    break;
                case '\\':
                    if (l.Dir == (1,0)) l.Dir = (0,1);
                    else if (l.Dir == (-1,0)) l.Dir = (0,-1);
                    else if (l.Dir == (0,1)) l.Dir = (1,0);
                    else if (l.Dir == (0,-1)) l.Dir = (-1,0);
                    break;
                case '-':
                    if (l.Dir.x == 0) // Up or Down
                    {
                        l.Dir = (-1,0);
                        if (l.X < xs - 1)
                            RunLaser(new(l.X + 1, l.Y, (1,0)));
                    }
                    break;
                case '|':
                    if (l.Dir.y == 0) // Left or Right
                    {
                        l.Dir = (0,-1);
                        if (l.Y < ys - 1)
                            RunLaser(new(l.X, l.Y + 1, (0,1)));
                    }
                    break;
                default:
                    // maze[l.X,l.Y] = '#';
                    break;
            }
            l.X += l.Dir.x;
            l.Y += l.Dir.y;
            if (l.X >= xs || l.X < 0 || l.Y >= ys || l.Y < 0)
                return;
            RunLaser(l);
        }
    }

    static char[,] ParseInput(string filePath)
    {
        string[] strings = File.ReadAllLines(filePath);
        char[,] result = new char[strings[0].Length,strings.Length];
        for(int y = 0; y < strings.Length; y++)
            for(int x = 0; x < strings[0].Length; x++)
                result[x,y] = strings[y][x];
        return result;
    }
    static HashSet<(int,int)>[,] BuilldEmptyEnergize(int xs, int ys)
    {
        HashSet<(int,int)>[,] result = new HashSet<(int,int)>[xs,ys];
        for(int y = 0; y < ys; y++)
            for(int x = 0; x < xs; x++)
                result[x,y] = [];
        return result;
    }

    class Laser
    {
        public Laser(int x, int y, (int, int) dir)
        {
            Dir = dir;
            X = x;
            Y = y;
        }
         public (int x, int y) Dir;
        public int X;
        public int Y;

        public override string ToString()
        {
            return $"x{X} y{Y} dir({Dir.x},{Dir.y})";
        }
    }

}
