using System.Text.RegularExpressions;
using System.Xml.XPath;
#nullable disable
static class Day4
{
    public static void Solve1(){
        string filePath = "src/Day4/4.in";
        using StreamReader sr = new(filePath);
        string line;
        int result = 0;
        int i = 0;
        while ((line = sr.ReadLine()) != null)
        {        
            List<int> winning = ExtractNumbers(line.Split('|')[0].Split(':')[1]);
            List<int> card = ExtractNumbers(line.Split('|')[1]);
            int intersect = winning.Intersect(card).Count();
            result += (int)Math.Pow(2.0, intersect - 1.0);
            i++;
        }
        Console.WriteLine(result.ToString());
    }

    public static void Solve2(){
        string filePath = "src/Day4/4.in";
        using StreamReader sr = new(filePath);
        string line;
        int result = 0;
        int i = 0;
        Dictionary<int,int> cards = new();
        Queue<int> queue = new();
        while ((line = sr.ReadLine()) != null)
        {        
            List<int> winning = ExtractNumbers(line.Split('|')[0].Split(':')[1]);
            List<int> card = ExtractNumbers(line.Split('|')[1]);
            int intersect = winning.Intersect(card).Count();
            cards.Add(i,intersect);
            queue.Enqueue(i);

            result += (int)Math.Pow(2.0, intersect - 1.0);
            i++;
        }
        // return result.ToString();
        result = 0;
        while(queue.Count > 0)
        {
            int card = queue.Dequeue();
            result++;
            for(int j = card + 1; j < cards[card] + card + 1; j++)
                queue.Enqueue(j);
        }
        Console.WriteLine(result.ToString());
    }

    static List<int> ExtractNumbers(string input)
    {
        List<int> nums = new();
        
        Regex regex = new Regex(@"\d+");
        MatchCollection matches = regex.Matches(input);
        
        foreach (Match match in matches)
        {
            if (int.TryParse(match.Value, out int number))
            {
                nums.Add(number);
            }
        }

        return nums;
    }
}