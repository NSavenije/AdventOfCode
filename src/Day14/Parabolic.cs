#nullable disable

using System.Diagnostics.SymbolStore;
using System.Text;

public static class Day14
{
    public static void Solve1()
    {
        char[,] grid = ParseInput();
        North(grid);
        Console.WriteLine(CountLoad(grid));
    }

    public static void Solve2()
    {
        char[,] grid = ParseInput();

        int round = 0;
        List<int> loads = [];
        List<int> hashes = [];

        while (round < 400) // How do I know when a cycle is possible
        {
            Cycle(grid);
            hashes.Add(Hash(grid));
            loads.Add(CountLoad(grid));
            round++;
        }

        (int tortoise, int hare) = FindCycle(hashes);
        
        // FLoyd's tortoise and hare without a linked list improv...
        
        int length = hare - tortoise;
        int offset = (1_000_000_000 - hare) % length - 1;
        Console.WriteLine(loads[tortoise + offset]);

    }

    static (int,int) FindCycle(List<int> hashes)
    {
        int tortoise = 0;
        bool foundCycle = false;

        while (!foundCycle)
        {
            int hare;
            for (hare = tortoise + 1; hare < hashes.Count; hare++)
            {
                if (hashes[tortoise] == hashes[hare] && IsCycle(hashes, tortoise, hare))
                {
                    return (tortoise, hare);
                }
            }
            tortoise++;
        }
        return (-1,-1);
    }

    static void Cycle(char[,] grid)
    {
        North(grid);
        West(grid);
        South(grid);
        East(grid);
    }

    static char[,] ParseInput()
    {
        string filePath = "src/Day14/14.in";
        var input = File.ReadAllLines(filePath);
        // x is col, y is row
        char[,] grid = new char[input[0].Length,input.Length];
        for(int i = 0; i < input.Length; i++)
        {
            string line = input[i];
            // i = row = y; j = col = x
            for (int j = 0; j < line.Length; j++)
                grid[j,i] = line[j];
        }
        return grid;
    }

    static void North(char[,] grid)
    {
        // North 
        for(int x = 0; x < grid.GetLength(0); x++)
        {   
            //walk down
            for(int y = 0; y < grid.GetLength(1) - 1 ; y++)
            {
                char cur = grid[x, y];
                char next = grid[x, y + 1];
                if (next == 'O' && cur == '.')
                {
                    // Walk up
                    for (int z = y + 1; z > 0; z--)
                    {
                        char down = grid[x,z - 1];
                        if (down == '.')
                        {
                            grid[x,z] = '.';
                            grid[x,z - 1] = 'O';
                        }
                        if (down == '#' || down == 'O')
                            break;
                    }
                }
            }
        }
    }

    static void West(char[,] grid)
    {
        for(int y = 0; y < grid.GetLength(1); y++)
        {   
            //walk right
            for(int x = 0; x < grid.GetLength(0) - 1 ; x++)
            {
                // #.[.O].
                char cur = grid[x, y];
                char next = grid[x + 1, y];
                if (next == 'O' && cur == '.')
                {
                    // walk left
                    for (int z = x + 1; z > 0; z--)
                    {
                        char west = grid[z - 1, y];
                        if (west == '.')
                        {
                            grid[z,y] = '.';
                            grid[z - 1, y] = 'O';
                        }
                        if (west == '#' || west == 'O')
                            break;
                    }
                }
            }
        }
    }

    static void South(char[,] grid)
    {
        for(int x = 0; x < grid.GetLength(0); x++)
        {   
            //walk up
            for(int y = grid.GetLength(1) - 1; y > 0 ; y--)
            {
                char cur = grid[x, y];
                char next = grid[x, y - 1];
                if (next == 'O' && cur == '.')
                {
                    // walk down
                    for (int z = y - 1; z < grid.GetLength(1) - 1; z++)
                    {
                        char up = grid[x,z + 1];
                        if (up == '.')
                        {
                            grid[x,z] = '.';
                            grid[x,z + 1] = 'O';
                        }
                        if (up == '#' || up == 'O')
                            break;
                    }
                }
            }
        }
    }

    static void East(char[,] grid)
    {
         for(int y = 0; y < grid.GetLength(1); y++)
        {   
            //walk left
            for(int x = grid.GetLength(0) - 1; x > 0 ; x--)
            {
                // ..[O.].#
                char cur = grid[x, y];
                char next = grid[x - 1, y];
                if (next == 'O' && cur == '.')
                {
                    // walk right
                    for (int z = x - 1; z < grid.GetLength(0) - 1; z++)
                    {
                        char now = grid[z,x];
                        char east = grid[z + 1, y];
                        if (east == '.')
                        {
                            grid[z,y] = '.';
                            grid[z + 1, y] = 'O';
                        }
                        if (east == '#' || east == 'O')
                            break;
                    }
                }
            }
        }
    }

    static int Hash(char[,] grid)
    {
        var sb = new StringBuilder();
        for (int x = 0; x < grid.GetLength(0); x++) 
        {
            for (int y = 0; y < grid.GetLength(1); y++) 
            {
                sb.Append(grid[x, y]);
            }
        }
        return sb.ToString().GetHashCode();
    }

    static int CountLoad(char[,] grid)
    {
        int sum = 0;
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x,y] == 'O')
                    sum += grid.GetLength(1) - y;
            }
        }
        return sum;
    }

    static bool IsCycle(List<int> hashes, int a, int b)
        => Enumerable.Range(0, b - a).All(j => hashes[a + j] == hashes[b + j]);
}

