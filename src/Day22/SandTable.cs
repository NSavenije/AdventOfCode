using System.Text;

public static class Day22
{
    public static void Solve1() => Solve(buildTree: false);
    public static void Solve2() => Solve(buildTree: true);
    public static void Solve(bool buildTree)
    {
        string filePath = "src/Day22/22.in";
        List<(int[] p1,int[] p2)> bricks = ParseInput(filePath);
        Dictionary<(int x,int y), (int height,int brick)> highest = [];
        HashSet<int> unsupported = [];
        Dictionary<int,HashSet<int>> supportSet = [];
        List<List<int>> supportTree = [];
        for (int i = 0; i < bricks.Count; i++)
        {
            supportTree.Add([]);
            int maxHeight = -1;
            
            for (int x = bricks[i].p1[0]; x <= bricks[i].p2[0]; x++)
            {
                for (int y = bricks[i].p1[1]; y <= bricks[i].p2[1]; y++)
                {
                    if (!highest.TryGetValue((x,y), out _))
                    {
                        highest[(x,y)] = (0,-1);
                    }
                    if (highest[(x,y)].height + 1 > maxHeight)
                    {
                        maxHeight = highest[(x,y)].height + 1;
                        supportSet[i] = [highest[(x,y)].brick];
                    }
                    else if (highest[(x,y)].height + 1 == maxHeight)
                    {
                        supportSet[i].Add(highest[(x,y)].brick);
                    }
                }
            }

            if (!buildTree && supportSet[i].Count == 1)
            {
                unsupported.Add(supportSet[i].First());
            }
            if (buildTree)
            {
                foreach (int x in supportSet[i])
                {
                    if (x != -1)
                    {
                        supportTree[x].Add(i);
                    }
                }
            }

            int fall = bricks[i].p1[2] - maxHeight;
            if (fall > 0)
            {
                bricks[i].p1[2] -= fall;
                bricks[i].p2[2] -= fall;
            }

            for (int x = bricks[i].p1[0]; x <= bricks[i].p2[0]; x++)
            {
                for (int y = bricks[i].p1[1]; y <= bricks[i].p2[1]; y++)
                {
                    highest[(x, y)] = (bricks[i].p2[2], i);
                }
            }
        }
        if (!buildTree)
            Console.WriteLine(bricks.Count - unsupported.Count + 1);
        else
        {
            Console.WriteLine(supportTree.Select((x, i) => CountDestruction(i, supportTree)).Sum());
        }
    }

    static int CountDestruction(int brick, List<List<int>> tree)
    {
        int[] parents = new int[tree.Count];

        // Calculate parent counts for each node
        tree.SelectMany(childNodes => childNodes).ToList().ForEach(child => parents[child]++);

        Queue<int> queue = [];
        queue.Enqueue(brick);
        int destructionCount = -1;

        // Process the removal of bricks and their dependencies
        while (queue.Count > 0)
        {
            destructionCount++;
            int currentBrick = queue.Dequeue();
            foreach (var child in tree[currentBrick])
            {
                parents[child]--;
                if (parents[child] == 0)
                {
                    queue.Enqueue(child);
                }
            }
        }

        return destructionCount;
    }

    static List<(int[],int[])> ParseInput(string filePath)
    {
        List<string> lines = File.ReadAllLines(filePath).ToList();
        List<(int[] p1,int[] p2)> bricks = [];
        foreach (var line in lines)
        {
            string[] parts = line.Split("~");
            int[] p1 = parts[0].Split(",").Select(int.Parse).ToArray();
            int[] p2 = parts[1].Split(",").Select(int.Parse).ToArray();
            bricks.Add((p1, p2));
        }
        bricks.Sort((x, y) => x.p1[2].CompareTo(y.p1[2]));
        return bricks;
    }

    public static string ToString(List<(int[] p1, int[] p2)> bricks)
    {
        int maxHeight = bricks.Max(b => b.p2[2]);
        int maxWidth = bricks.Max(b => b.p2[0]) + 1;
        int maxDepth = bricks.Max(b => b.p2[1]) + 1;

        char[, ,] stack = new char[maxWidth, maxDepth, maxHeight + 1];

        foreach (var (p1, p2) in bricks)
        {
            for (int x = p1[0]; x <= p2[0]; x++)
            {
                for (int y = p1[1]; y <= p2[1]; y++)
                {
                    int z = p2[2];
                    for (int k = p1[2]; k <= z; k++)
                    {
                        stack[x, y, k] = '#';
                    }
                }
            }
        }

        string result = "";
        for (int z = 1; z <= maxHeight; z++)
        {
            result += $"Layer {z} \n";
            for (int y = 0; y < maxDepth; y++)
            {
                for (int x = 0; x < maxWidth; x++)
                {
                    result += (stack[x, y, z] == '\0') ? '.' : stack[x, y, z];
                }
                result += "\n";
            }
            result += "\n";
        }

        return result;
    }
}