#nullable disable
using System.Globalization;
using System.Text.RegularExpressions;

static class Day1 {
    public static void Solve1()
    {
        ParseInput(false);
    }
    public static void Solve2()
    {
        ParseInput(true);
    }

    static void ParseInput(bool numbersAsText)
    {
        int n = 0;
        string filePath = "src/Day1/1.in";

        Dictionary<int,string> dict = new()
        {
            {1, "one"  },
            {2, "two"  },
            {3, "three"},
            {4, "four" },
            {5, "five" },
            {6, "six"  },
            {7, "seven"},
            {8, "eight"},
            {9, "nine" }
        };

        Dictionary<int,string> reverse = new()
        {
            {1, "eno"  },
            {2, "owt"  },
            {3, "eerht"},
            {4, "ruof" },
            {5, "evif" },
            {6, "xis"  },
            {7, "neves"},
            {8, "thgie"},
            {9, "enin" }
        };
        
        if (File.Exists(filePath))
        {
            using StreamReader sr = new(filePath);
            string line;
            int result = 0;
            
            while ((line = sr.ReadLine()) != null)
            {
                n++;
                result += CalculateLineValue(line);
            }
            Console.WriteLine(result.ToString());
        }
        else throw new FileNotFoundException("File not found.");

        int CalculateLineValue(string line)
        {
            if (!numbersAsText){
                string allNumbers = new(line.Where(char.IsDigit).ToArray());
                if (allNumbers.Length == 0) return 0;
                // Console.WriteLine(allNumbers);
                string result = allNumbers.First() + "" + allNumbers.Last();
                // Console.WriteLine($"{n}: {result}");
                return int.Parse(result);
            }
            else
            { 
                // Console.WriteLine($"{n}: {line}");
                int res = int.Parse(FindFirst(line) + FindLast(line));
                // Console.WriteLine(res);
                return res;
            }
            
        }

        string FindFirst(string line) 
        {
            int[] scores = new int[10];
            for (int i = 1; i < scores.Length ; i++)
            {
                scores[i] = dict[i].Length;
            }
            for (int i = 0; i < line.Length; i++){
                char currentLetter = line[i];
                if (Char.IsNumber(currentLetter))
                    return currentLetter.ToString();
                for(int j = 1; j <= 9; j++) {
                    char requiredLetter = dict[j][dict[j].Length - scores[j]];
                    if (currentLetter == requiredLetter)
                    {
                        if (scores[j] == 1)
                        {
                            //SUCCESS
                            return j.ToString();
                        }
                        scores[j]--;   
                    }
                    else 
                    {
                        scores[j] = dict[j].Length;
                        requiredLetter = dict[j][0];
                        if (currentLetter == requiredLetter)
                        {
                            scores[j]--;
                        }
                    }
                }
            }
            throw new Exception($"Line {line} contains no letter or numbers");
        }

        string FindLast(string line) 
        {
            int[] scores = new int[10];
            for (int i = 1; i < scores.Length ; i++)
            {
                scores[i] = dict[i].Length;
            }
            for (int i = line.Length - 1; i >= 0; i--){
                char currentLetter = line[i];
                if (Char.IsNumber(currentLetter))
                    return currentLetter.ToString();
                for(int j = 1; j <= 9; j++) {
                    char requiredLetter = reverse[j][reverse[j].Length - scores[j]];
                    if (currentLetter == requiredLetter)
                    {
                        if (scores[j] == 1)
                        {
                            //SUCCESS
                            return j.ToString();
                        }
                        scores[j]--;   
                    }
                    else 
                    {
                        scores[j] = dict[j].Length;
                        requiredLetter = reverse[j][0];
                        if (currentLetter == requiredLetter)
                        {
                            scores[j]--;
                        }
                    }
                }
            }
            throw new Exception($"Line {line} contains no letter or numbers");
        }
    } 
}