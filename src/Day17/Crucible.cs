using System.Collections.Generic;
using System.Text;

public static class Day17
{
    public static void Solve1()
    {
        int[,] graph = ParseInput("src/Day17/17.in");
        Console.WriteLine(Solve(graph, 1, 3));
    }

    public static void Solve2()
    {
        int[,] graph = ParseInput("src/Day17/17.in");
        Console.WriteLine(Solve(graph, 4, 10));
    }

    private static int Solve(int[,] graph, int min, int max)
    {
        // R D L U
        (int x,int y)[] dirs = [(1,0),(0,1),(-1,0),(0,-1)];
        int xs = graph.GetLength(0), ys = graph.GetLength(1);
        int[,,] dists = InitDists(xs, ys);
        Queue<Node> queue = new();
        List<int> outputs = [];
        queue.Enqueue(new Node(0,0,0,[0,1]));
        while (queue.Count > 0)
        {
            Node node = queue.Dequeue();

            // Last node reached!
            if (node.X == xs - 1 && node.Y == ys - 1)
                outputs.Add(node.Dist);

            foreach(int dir in node.Dirs)
            {
                int dist = 0;
                for (int distance = 1; distance <= max; distance++)
                {
                    int x = node.X + dirs[dir].x * distance;
                    int y = node.Y + dirs[dir].y * distance;
                    if (x < xs && x >= 0 && y < ys && y >= 0)
                    {
                        dist += graph[x,y];
                        int totalDist = node.Dist + dist;
                        if (totalDist < dists[x,y,dir] && distance >= min)
                        {
                            dists[x,y,dir] = totalDist;
                            queue.Enqueue(new Node(X: x, Y: y, Dist: totalDist, Dirs: [(dir + 1) % 4, (dir + 3) % 4]));
                        }
                    }
                }
            }
        }
        return outputs.Min();
    }


    private static int[,] ParseInput(string filePath)
    {
        string[] strings = File.ReadAllLines(filePath);
        int[,] result = new int[strings[0].Length,strings.Length];
        for(int y = 0; y < strings.Length; y++)
            for(int x = 0; x < strings[0].Length; x++)
                result[x,y] = strings[y][x] - 48;
        return result;
    }

    private static int[,,] InitDists(int xs, int ys)
    {
        int[,,] result = new int[xs,ys,4];
        for(int y = 0; y < ys; y++)
            for(int x = 0; x < xs; x++)
                for(int dir = 0; dir < 4; dir++)
                    result[x,y,dir] = int.MaxValue;
        result[0,0,0] = 0;
        return result;
    }

    readonly record struct Node(int X, int Y, int Dist, HashSet<int> Dirs)
    {
        public int X { get; } = X;
        public int Y { get; } = Y;
        public int Dist { get;} = Dist;
        public HashSet<int> Dirs { get; } = Dirs;


        public override string ToString() => $"({X}, {Y}): {Dist}";
    }
}