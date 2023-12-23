public static class Day23
{
    public static void Solve1()
    {
        var lines = File.ReadAllLines("src/Day23/23b.in").ToArray();
        Tree mazeTree = MazeParser.ParseMaze(lines);
        // Console.WriteLine(mazeTree.ToString());
        // Console.WriteLine(MazeTraversal.DFS(mazeTree.Root));
        
    }



    public class Node((int,int) pos)
    {
        public Dictionary<(int,int),int> DistanceFromNode { get; set; } = [];
        public int DistanceFromRoot { get; set; }
        public int Neigbours;
        public List<Node> AdjacentNodes { get; } = [];

        public (int r,int c) Pos {get; set;} = pos;

        public void AddChild(Node node)
        {
            AdjacentNodes.Add(node);
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
                        nodes.Add((i,j), new Node((i, j))
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
                Console.WriteLine($"{junction.Key}: {string.Join(',',junction.Value.AdjacentNodes.Select(n => $"{n.Pos}"))}");
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
                    junction.Value.AddChild(node);
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
                    junction.Value.AddChild(node);
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
        public static int DFS(Node startNode)
        {
            if (startNode == null)
                return 0;

            int maxDistance = startNode.DistanceFromRoot; // = 0;

            foreach (var adjacentNode in startNode.AdjacentNodes)
            {
                adjacentNode.DistanceFromRoot = startNode.DistanceFromRoot + adjacentNode.DistanceFromNode[startNode.Pos];
                int distance = DFSLargestDistanceHelper(adjacentNode);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }

            return maxDistance;
        }

        private static int DFSLargestDistanceHelper(Node node)
        {
            if (node == null)
                return 0;

            int maxChildDistance = node.DistanceFromRoot;

            foreach (var adjacentNode in node.AdjacentNodes)
            {
                adjacentNode.DistanceFromRoot = node.DistanceFromRoot + adjacentNode.DistanceFromNode[node.Pos];
                int distance = DFSLargestDistanceHelper(adjacentNode);
                if (distance > maxChildDistance)
                {
                    maxChildDistance = distance;
                }
            }

            return maxChildDistance;
        }


        public static void BFS(Tree maze)
        {
            if (maze.Root == null)
            {
                Console.WriteLine("No entry point found.");
                return;
            }

            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            maze.Root.DistanceFromRoot = 0; // Set the root distance to 0
            queue.Enqueue(maze.Root);
            visited.Add(maze.Root);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                // Console.WriteLine($"Distance from previous: {current.DistanceFromPrevious}, Distance from root: {current.DistanceFromRoot}");

                foreach (Node neighbor in current.AdjacentNodes)
                {
                    if (!visited.Contains(neighbor))
                    {
                        neighbor.DistanceFromRoot = current.DistanceFromRoot + 1; // Increment distance from root
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }
    }

}

