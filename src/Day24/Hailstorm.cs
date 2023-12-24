using Equation = (long dx, long x, long dy ,long y);

public static class Day24
{
    public static void Solve1()
    {
        string filepath = "src/Day24/24.in";
        List<Equation> equations = ParseInput(filepath);
        long lower = 200000000000000;
        // long lower = 7;
        long upper = 400000000000000;
        // long upper = 27;
        int counter = 0;
        for (int i = 0; i < equations.Count - 1; i++)
        {
            Equation e1 = equations[i];
            for (int j = i + 1; j < equations.Count; j++)
            {
                Equation e2 = equations[j];
                if (Colliding(e1,e2,lower,upper))
                {
                    counter++;
                }
            }
        }
        Console.WriteLine(counter);

        
    }

    public static bool Colliding(Equation e1, Equation e2, long lower, long upper)
    {
        double a = (double)(e1.dy / (double)e1.dx);

        double denominator = a * e2.dx - e2.dy;
        if (denominator == 0) 
            return false;

        double t1 = (e2.y - e1.y + a * e1.x - a * e2.x) / denominator;
        if (t1 < 0)
            return false;

        double t2 = (e2.x + e2.dx * t1 - e1.x) / e1.dx;
        if (t2 < 0)
            return false;

        double x = e1.x + e1.dx * t2;
        double y = e1.y + e1.dy * t2;

        // Console.WriteLine($"{e1.x} {e1.y} - {e2.x} {e2.y}:");

        if (lower <= x && x <= upper && lower <= y && y <= upper)
        {
            // Console.WriteLine($"{x}   {y}");
            return true;
        }
        return false;
    }

    public static List<Equation> ParseInput(string filepath)
    {
        List<string> input = File.ReadAllLines(filepath).ToList();
        List<Equation> equations = [];
        foreach(string line in input)
        {
            string[] parts = line.Split('@').Select(s => s.Trim()).ToArray();
            string[] constants = parts[0].Split(',').Select(s => s.Trim()).ToArray();
            string[] coefficients = parts[1].Split(',').Select(s => s.Trim()).ToArray();
            Equation equation = new(long.Parse(coefficients[0]),long.Parse(constants[0]),long.Parse(coefficients[1]),long.Parse(constants[1]));
            equations.Add(equation);
        }
        return equations;
    }
}