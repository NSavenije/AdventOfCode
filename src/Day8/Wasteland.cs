#nullable disable
public static class Day8
{
    public static void Solve1()
    {
        string fileName = "src/Day8/8.in"; 
        Dictionary<string,(string,string)> input = ParseInput(fileName);
        string root = "AAA";
        string path = File.ReadLines(fileName).First();
        int counter = 0;
        while(root != "ZZZ")
        {
            char p = path[counter % path.Length]; 
            // Console.WriteLine($"{counter}: {root}, {p}");
            counter++;
            root = p == 'L' ? input[root].Item1 : input[root].Item2;
        }
        Console.WriteLine(counter);
    }

    public static void Solve2()
    {
        string fileName = "src/Day8/8.in";
        var input = ParseInput(fileName);
        string path = File.ReadLines(fileName).First();
        List<string> starts = input.Keys.Where(key => key.EndsWith('A')).ToList();
        List<int> counts = new();
        foreach(string start in starts)
        {
            string curr = start;
            int counter = 0;
            while(!curr.EndsWith('Z'))
            {
                char p = path[counter % path.Length]; 
                counter++;
                curr = p == 'L' ? input[curr].Item1 : input[curr].Item2;
            }
            counts.Add(counter);
        }
        Console.WriteLine(lcm_of_array_elements(counts.ToArray()));
    }

    private static Dictionary<string,(string,string)> ParseInput(string filePath)
    {
        using StreamReader sr = new(filePath);
        string line;
        Dictionary<string,(string,string)> res = new();
        while ((line = sr.ReadLine()) != null)
        {
            // if line contains only letters
            if (line.All(char.IsLetter)) { continue; }

            // if empty
            if (line == "") { continue; }

            // else
            string[] inp = line.Split('=');
            string root = inp[0].Trim();
            string[] nodes = inp[1].Split(',');
            nodes[0] = nodes[0].Split('(')[1].Trim();
            nodes[1] = nodes[1].Split(')')[0].Trim();
            res.Add(root, (nodes[0], nodes[1]));
        }
        return res;
    }


    // https://www.geeksforgeeks.org/lcm-of-given-array-elements/
    public static long lcm_of_array_elements(int[] element_array)
    {
        long lcm_of_array_elements = 1;
        int divisor = 2;
         
        while (true) {
             
            int counter = 0;
            bool divisible = false;
            for (int i = 0; i < element_array.Length; i++) {
 
                // lcm_of_array_elements (n1, n2, ... 0) = 0.
                // For negative number we convert into
                // positive and calculate lcm_of_array_elements.
                if (element_array[i] == 0) {
                    return 0;
                }
                else if (element_array[i] < 0) {
                    element_array[i] = element_array[i] * (-1);
                }
                if (element_array[i] == 1) {
                    counter++;
                }
 
                // Divide element_array by devisor if complete
                // division i.e. without remainder then replace
                // number with quotient; used for find next factor
                if (element_array[i] % divisor == 0) {
                    divisible = true;
                    element_array[i] = element_array[i] / divisor;
                }
            }
 
            // If divisor able to completely divide any number
            // from array multiply with lcm_of_array_elements
            // and store into lcm_of_array_elements and continue
            // to same divisor for next factor finding.
            // else increment divisor
            if (divisible) {
                lcm_of_array_elements = lcm_of_array_elements * divisor;
            }
            else {
                divisor++;
            }
 
            // Check if all element_array is 1 indicate 
            // we found all factors and terminate while loop.
            if (counter == element_array.Length) {
                return lcm_of_array_elements;
            }
        }
    }
}