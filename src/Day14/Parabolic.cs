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
        LinkedList<int> hashes = new();

        while (round < 1000) // How do I know when a cycle is possible
        {
            Cycle(grid);
            hashes.AddLast(Hash(grid));
            loads.Add(CountLoad(grid));
            round++;
        }

        (int tortoise, int hare) = Floyd(hashes);
        
        // FLoyd's tortoise and hare without a linked list improv...
        
        int length = hare - tortoise;
        int offset = (1_000_000_000 - hare) % length - 1;
        Console.WriteLine(loads[tortoise + offset]);

    }

    static (int,int) Floyd(LinkedList<int> x0) {
        // """Floyd's cycle detection algorithm."""
        // # Main phase of algorithm: finding a repetition x_i = x_2i.
        // # The hare moves twice as quickly as the tortoise and
        // # the distance between them increases by 1 at each step.
        // # Eventually they will both be inside the cycle and then,
        // # at some point, the distance between them will be
        // # divisible by the period λ.
        var tortoise = x0.Head.Next; // # f(x0) is the element/node next to x0.
        var hare = tortoise.Next;
        while (tortoise != hare)
        {
            tortoise = tortoise.Next;
            hare = hare.Next.Next;
        }
    
        // # At this point the tortoise position, ν, which is also equal
        // # to the distance between hare and tortoise, is divisible by
        // # the period λ. So hare moving in cycle one step at a time, 
        // # and tortoise (reset to x0) moving towards the cycle, will 
        // # intersect at the beginning of the cycle. Because the 
        // # distance between them is constant at 2ν, a multiple of λ,
        // # they will agree as soon as the tortoise reaches index μ.

        // # Find the position μ of first repetition.    
        int mu = 0;
        tortoise = x0.Head;
        while (tortoise != hare)
        {
            tortoise = tortoise.Next;
            hare = hare.Next;  // # Hare and tortoise move at same speed
            mu += 1;
        }
    
        // # Find the length of the shortest cycle starting from x_μ
        // # The hare moves one step at a time while tortoise is still.
        // # lam is incremented until λ is found.
        int lam = 1;
        hare = tortoise.Next;
        while (tortoise != hare){
            hare = hare.Next;
            lam += 1;
        }

        return (lam, mu);
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
}

public class Node<T>
{
    public T Data { get; set; }
    public Node<T> Next { get; set; }

    public Node(T data)
    {
        Data = data;
        Next = null;
    }
}

public class LinkedList<T>
{
    public Node<T> Head { get; private set; }
    public int Count { get; private set; }

    public LinkedList()
    {
        Head = null;
        Count = 0;
    }

    public void AddLast(T data)
    {
        Count++;
        Node<T> newNode = new Node<T>(data);

        if (Head == null)
        {
            Head = newNode;
            Head.Next = Head; 
        }
        else
        {
            Node<T> current = Head;
            while (current.Next != Head) 
            {
                current = current.Next;
            }
            current.Next = newNode;
            newNode.Next = Head; 
        }
    }
}

