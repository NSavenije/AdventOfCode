#nullable disable
static class Day6
{
    public static void Solve1()
    {
        string filePath = "src/Day6/6.in";
        Console.WriteLine(Solve(filePath, true));
    }

    public static void Solve2()
    {
        string filePath = "src/Day6/6c.in";
        Console.WriteLine(Solve(filePath, false));
    }

    public static string Solve(string filePath, bool solveA)
    {
        using StreamReader sr = new(filePath);
        long[] times = sr.ReadLine().Split(':')[1].Trim().Split(' ').Select(long.Parse).ToArray();
        long[] dists = sr.ReadLine().Split(':')[1].Trim().Split(' ').Select(long.Parse).ToArray();
        long[] wins = new long[dists.Length];
        long total = 1;
        for(int i = 0; i < times.Length; i++)
        {
            long time = times[i];
            long dist = dists[i];
            for(int j = 0; j < time; j++)
            {
                // Distance for each thing -> release at 0, dist is 0 * time - 0
                long d = j * (time - j);
                // Console.WriteLine($"Req {dist} our {d}");
                if (d > dist) wins[i]++;
            }
            // Console.WriteLine(wins[i]);
            total *= wins[i];
        }
        if(solveA)
            return total.ToString();
        
        long firstWin = BinarySearch(times[0], dists[0], 0, times[0] / 2);
        long lastWin = times[0] - firstWin + 1;
        long winCount = lastWin - firstWin;
        return winCount.ToString();
    }

    public static long BinarySearch(long time, long goal, long min, long max)
    {
        if (min > max)
        {
            return -1;
        }
        else
        {
            // goal is their dist, so that insane long
            // d is our dist

            long mid = (min+max)/2;
            long d = mid * (time - mid);
            long _d = (mid - 1) * (time - (mid - 1));
            if (d >= goal && _d < goal)
            {
                return mid;
            }
            else if (goal < d)
            {
                return BinarySearch(time, goal, min, mid - 1);
            }
            else
            {
                return BinarySearch(time, goal, mid + 1, max);
            }
        }
    }
}