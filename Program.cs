#nullable disable
using System.Reflection;

class Program
{
    private static void Main(string[] args)
    {
        int day = 1;
        int part = 1;
        int currentDay = 20;
        if (args.Length == 1 && args[0] == "A")
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for(int i = 1; i <= currentDay; i++)
            {
                Type classType = Type.GetType($"Day{i}");
                for (int j = 1; j <= 2; j++) {
                    Console.WriteLine($"Running Day {i}-{j}");
                    MethodInfo method = classType.GetMethod($"Solve{j}");
                    var funcWatch = System.Diagnostics.Stopwatch.StartNew();
                    method.Invoke(null, null); 
                    Console.WriteLine($"Run time = {funcWatch.ElapsedMilliseconds}ms \n");
                }
            }
            watch.Stop();
            Console.WriteLine($"Total run time = {watch.ElapsedMilliseconds}ms");
        }
        else{

            if (args.Length != 2) {
                Console.WriteLine("Please select a day to run: [1..25]");
                day = int.Parse(Console.ReadLine());
                Console.WriteLine("Part 1 or Part 2?");
                part = int.Parse(Console.ReadLine());
            }
            else {
                day = int.Parse(args[0]);
                part = int.Parse(args[1]);
            }
            
            Console.WriteLine($"Running Day {day}-{part}");
            Type classType = Type.GetType($"Day{day}");
            MethodInfo method = classType.GetMethod($"Solve{part}");
            var funcWatch = System.Diagnostics.Stopwatch.StartNew();
            method.Invoke(null, null); 
            Console.WriteLine($"Run time = {funcWatch.ElapsedMilliseconds}ms \n");
        }
    }
}