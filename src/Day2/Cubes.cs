#nullable disable
static class Day2{
    public static void Solve1() =>
        ParseInput(false);

    public static void Solve2() =>
        ParseInput(true);

    private static void ParseInput(bool count)
    {
        string filePath = "src/Day2/2.in";
        using StreamReader sr = new(filePath);
        string line;
        int result = 0;
        while ((line = sr.ReadLine()) != null)
        {
            bool possible = true;
            int id = int.Parse(line.Split(':')[0].Split(' ')[1]);
            line = line.Split(':')[1];
            List<string> hands = line.Split(';').ToList();
            int minR = 1;
            int minG = 1;
            int minB = 1;
            foreach (string hand in hands)
            {
                List<string> pairs = hand.Split(',').ToList();          
                foreach (string pair in pairs)
                {
                    // Console.WriteLine(pair);
                    int amount = int.Parse(pair.Split(' ')[1]);
                    string c = pair.Split(' ')[2];
                    switch (c)
                    {
                        case "red": // Max 12
                            minR = Math.Max(amount, minR);
                            if (amount > 12) possible = false;
                            break;
                        case "green": // Max 13
                            minG = Math.Max(amount, minG);
                            if (amount > 13) possible = false;
                            break;
                        case "blue": // Max 14
                            minB = Math.Max(amount, minB);
                            if (amount > 14) possible = false;
                            break;
                        default:
                            break;
                    }
                }
                //THIS IS ONE HAND IN ONE GAME
            }
            if (!count && possible)
                result += id;
            else if (count)    
                result += minR * minG * minB;
        }
        
        Console.WriteLine(result.ToString());
    }
}