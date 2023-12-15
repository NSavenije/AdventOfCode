#nullable disable

static class Day1 {
    public static void Solve1() =>
        ParseInput(false);
    public static void Solve2() =>
        ParseInput(true);

    static void ParseInput(bool numbersAsText)
    {
        Dictionary<string,string> repl = new()
        {
            {"one",   "o1e"},
            {"two",   "t2o"},
            {"three", "t3e"},
            {"four",  "f4r"},
            {"five",  "f5e"},
            {"six",   "s6x"},
            {"seven", "s7n"},
            {"eight", "e8t"},
            {"nine",  "n9e"}
        };

        string filePath = "src/Day1/1.in";
        using StreamReader sr = new(filePath);
        string line;
        int res = 0;
        while ((line = sr.ReadLine()) != null)
        {
            if (numbersAsText)
            {
                foreach (var r in repl)
                {
                    line = line.Replace(r.Key, r.Value);
                }
            }
            string s = new(line.Where(char.IsDigit).ToArray());
            res += int.Parse(s.First() + "" + s.Last());
        }
        Console.WriteLine(res);
    }
}