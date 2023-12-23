public static class Day23
{
    public static void Solve1()
    {
        var lines = File.ReadAllLines("src/Day23/23.in").ToArray();
        Tree mazeTree = MazeParser.ParseMaze(lines);
        // Console.WriteLine(mazeTree.ToString());
        // Console.WriteLine(MazeTraversal.DFS(mazeTree.Root));
        MazeTraversal mazeTraversal = new();
        Console.WriteLine(mazeTraversal.DFS(mazeTree.Root));
    }



    public class Node((int,int) pos, int cols)
    {
        public Dictionary<(int,int),int> DistanceFromNode { get; set; } = [];
        public int DistanceFromRoot { get; set; }
        public int Neigbours;
        public Dictionary<Node, int> AdjacentNodes { get; } = [];

        public (int r,int c) Pos {get; set;} = pos;

        public List<Node> Path = [];
        public int Cols = cols;

        public void AddChild(Node node, int distance)
        {
            AdjacentNodes.Add(node, distance);
        }
    }

    public class Tree(Node node)
    {
        public Node Root = node;

        // public override string ToString()
        // {
        //     if (Root == null)
        //     {
        //         return "Maze is empty.";
        //     }

        //     return PrintNode(Root);
        // }

        // private static string PrintNode(Node node)
        // {
        //     string result = $"Node: Distance from Previous = {node.DistanceFromPrevious}, Distance from Root = {node.DistanceFromRoot}, Adjacent Nodes: ";
        //     foreach (var adjacentNode in node.AdjacentNodes)
        //     {
        //         result += $"{adjacentNode.DistanceFromPrevious} ";
        //     }
        //     result += "\n";

        //     foreach (var adjacentNode in node.AdjacentNodes)
        //     {
        //         result += PrintNode(adjacentNode);
        //     }

        //     return result;
        // }
    }

    public class MazeParser
    {
        public static Tree ParseMaze(string[] mazeRows)
        {
            int rows = mazeRows.Length;
            int cols = mazeRows[0].Length;

            Dictionary<(int,int), Node> nodes = [];

            // Check if a position is a junction (has more than 2 accessible paths)
            int CountNeighbours(int x, int y)
            {
                int count = 0;
                if (x > 0 && mazeRows[x - 1][y] != '#') count++;
                if (x < rows - 1 && mazeRows[x + 1][y] != '#') count++;
                if (y > 0 && mazeRows[x][y - 1] != '#') count++;
                if (y < cols - 1 && mazeRows[x][y + 1] != '#') count++;
                return count;
            }

            // Create nodes for each traversable position
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int neighbours = CountNeighbours(i,j);
                    if (mazeRows[i][j] != '#' && neighbours != 2)
                    {
                        nodes.Add((i,j), new Node((i, j), cols)
                        {
                            Neigbours = neighbours
                        });
                    }
                }
            }

            foreach(var junction in nodes)
            {
                HashSet<(int,int)> visitedNodes = [];
                FloodFill(junction.Key,  junction, 0, visitedNodes);
                Console.WriteLine($"{junction.Key}: {string.Join(',',junction.Value.AdjacentNodes.Select(n => $"{n.Key.Pos}[{n.Value}]"))}");
            }

            void FloodFillSteepSlope((int i, int j) pos, KeyValuePair<(int,int),Node> junction, int depth, HashSet<(int,int)> visitedNodes)
            {
                // Console.WriteLine(pos);
                int i = pos.i;
                int j = pos.j;
                if (!visitedNodes.Add((i,j)))
                {
                    // Console.WriteLine("stopped: backtracking");
                    return;
                }

                if (pos != junction.Value.Pos && nodes.TryGetValue((i,j), out var node))
                {
                    // If I found another junction. STOP looking further
                    node.DistanceFromNode[junction.Key] = depth;
                    junction.Value.AddChild(node, depth);
                    // Console.WriteLine("stopped: node found");
                    return;
                }
                if (i + 1 <= mazeRows.Length - 1 && (mazeRows[i + 1][j] == '.' || mazeRows[i + 1][j] == 'v'))
                {
                    // Console.WriteLine("track: down");
                    FloodFillSteepSlope((i + 1, j), junction, depth + 1, visitedNodes);
                }
                if (i - 1 >= 0 && (mazeRows[i - 1][j] == '.' || mazeRows[i - 1][j] == '^'))
                {
                    // Console.WriteLine("track: up");
                    FloodFillSteepSlope((i - 1, j), junction, depth + 1, visitedNodes);
                }
                if (mazeRows[i][j + 1] == '.' || mazeRows[i][j + 1] == '>')
                {
                    // Console.WriteLine("track: right");
                    FloodFillSteepSlope((i, j + 1), junction, depth + 1, visitedNodes);
                }
                if (mazeRows[i][j - 1] == '.' || mazeRows[i][j - 1] == '<')
                {
                    // Console.WriteLine("track: left");
                    FloodFillSteepSlope((i, j - 1), junction, depth + 1, visitedNodes);
                }
            }

            void FloodFill((int i, int j) pos, KeyValuePair<(int,int),Node> junction, int depth, HashSet<(int,int)> visitedNodes)
            {
                // Console.WriteLine(pos);
                int i = pos.i;
                int j = pos.j;
                if (!visitedNodes.Add((i,j)))
                {
                    // Console.WriteLine("stopped: backtracking");
                    return;
                }

                if (pos != junction.Value.Pos && nodes.TryGetValue((i,j), out var node))
                {
                    // If I found another junction. STOP looking further
                    node.DistanceFromNode[junction.Key] = depth;
                    junction.Value.AddChild(node, depth);
                    // Console.WriteLine("stopped: node found");
                    return;
                }
                if (i + 1 <= mazeRows.Length - 1 && mazeRows[i + 1][j] != '#')
                {
                    // Console.WriteLine("track: down");
                    FloodFill((i + 1, j), junction, depth + 1, visitedNodes);
                }
                if (i - 1 >= 0 && mazeRows[i - 1][j] != '#')
                {
                    // Console.WriteLine("track: up");
                    FloodFill((i - 1, j), junction, depth + 1, visitedNodes);
                }
                if (mazeRows[i][j + 1] != '#')
                {
                    // Console.WriteLine("track: right");
                    FloodFill((i, j + 1), junction, depth + 1, visitedNodes);
                }
                if (mazeRows[i][j - 1] != '#')
                {
                    // Console.WriteLine("track: left");
                    FloodFill((i, j - 1), junction, depth + 1, visitedNodes);
                }
            }


            Tree maze = new(nodes[(0,1)]);

            return maze;
        }
    }

    public class MazeTraversal
    {
        List<Node> Path = [];
        List<List<Node>> Paths = [];
        public int DFS(Node startNode)
        {
            if (startNode == null)
                return 0;

            HashSet<(int,int)> visited = [];
            DFSLargestDistanceHelper(startNode, visited);
            int maxDistance = int.MinValue;
            foreach(List<Node> path in Paths)
            {
                // Console.WriteLine($"{string.Join(',', Path.Select(n => n.Pos))}");
                if (path.Count != 0 && path.Last().Pos.r == startNode.Cols - 1)
                {
                    int distance = GetDistance(path);
                    maxDistance = Math.Max(distance, maxDistance);
                }
            }
            return maxDistance;
        }

        private int DFSLargestDistanceHelper(Node node, HashSet<(int,int)> visited)
        {
            if (node == null)
                return 0;

            if(!visited.Add(node.Pos))
            {
                return 0;
            }
            else
            {
                Path.Add(node);
                Paths.Add(Path.ToArray().Select(n => n).ToList());
            }

            int maxDistance = GetDistance(); // Will always be replaced by child.
            // if (maxDistance == 6681)
                // Console.WriteLine($"{maxDistance}-{string.Join(',', Path.Select(n => n.Pos))}");

            foreach (var adjacentNode in node.AdjacentNodes)
            {

                int distance = DFSLargestDistanceHelper(adjacentNode.Key, visited);
                maxDistance = Math.Max(maxDistance, distance);
            }

            visited.Remove(node.Pos);
            Path.Remove(node);

            return maxDistance;
        }

        private int GetDistance()
        {
            int distance = 0;
            for(int i = 1; i < Path.Count; i++)
            {
                distance += Path[i].DistanceFromNode[Path[i - 1].Pos];
            }
            return distance;
        }

        private int GetDistance(List<Node> path)
        {
            int distance = 0;
            for(int i = 1; i < path.Count; i++)
            {
                distance += path[i].DistanceFromNode[path[i - 1].Pos];
            }
            return distance;
        }

    }

}

