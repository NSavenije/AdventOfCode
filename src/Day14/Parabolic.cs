#nullable disable

using System.Diagnostics.SymbolStore;
using System.Text;

public static class Day14
{
    public static void Solve1()
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
            {
                // Reverse
                grid[j,i] = line[j];
            }
        }
        long sum = 0;
        // we only go up
        for(int x = 0; x < grid.GetLength(0); x++)
        {
            
            //now we are checking 1 column
            for(int y = 0; y < grid.GetLength(1) - 1 ; y++)
            {
                char cur = grid[x, y];
                char next = grid[x, y + 1];
                if (next == 'O' && cur == '.')
                {
                    for (int z = y + 1; z > 0; z--)
                    {
                        char up = grid[x,z - 1];
                        if (up == '.')
                        {
                            grid[x,z] = '.';
                            grid[x,z - 1] = 'O';
                        }
                        if (up == '#' || up == 'O')
                            break;
                    }
                }
            }
            for(int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x,y] == 'O')
                    sum += grid.GetLength(1) - y;
            }
        }
        Console.WriteLine(sum);
    }

    public static void Solve2()
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
            {
                // Reverse
                grid[j,i] = line[j];
            }
        }

        int round = 0;
        List<int> loads = [];
        List<int> hashes = [];

        while (round < 1100) 
        {
            Cycle(grid);
            hashes.Add(Hash(grid));
            loads.Add(CountLoad(grid));
            round++;
        } 
        
        // FLoyd's tortoise and hare without a linked list improv...
        int tortoise = 0;
        int hare = 1;
        bool foundCycle = false;

        while (!foundCycle)
        {
            for (hare = tortoise + 1; hare < hashes.Count; hare++)
            {
                if (hashes[tortoise] == hashes[hare] && IsCycle(hashes, tortoise, hare))
                {
                    foundCycle = true;
                    break;
                }
            }
            if (foundCycle)
                break;
            tortoise++;
        }

        int length = hare - tortoise;
        int offset = (1_000_000_000 - hare) % length - 1;
        Console.WriteLine(loads[tortoise + offset]);
        
    }

    static void Cycle(char[,] grid)
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
        // West rows
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
        // South 
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
        // East rows
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

