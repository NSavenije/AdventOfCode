#nullable disable
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

public static class Day3
{
    public static void Solve1()
    {
        List<string> input = ParseInput();
        int result = 0;
        for(int l = 0; l < input.Count; l++)
        {
            //All numbers from 1 line
            List<(int,int)> nums = ExtractNumbers(input[l]);
            foreach((int,int) n in nums)
            {
                int numLength = n.Item2.ToString().Length;
                // Console.WriteLine(n.Item1 + ": " + n.Item2 + ", " + numLength);
                for (int i = 0; i < numLength; i++)
                {
                    char c;
                    //Check row above only if not on top row
                    if (l > 0)
                    {
                        //TOP LEFT? Only if first digit, and not on home column
                        if (i == 0 && n.Item1 > 0)
                        {
                            c = input[l-1][n.Item1 - 1];
                            if ((!Char.IsDigit(c)) && (c != '.'))
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                result += n.Item2;
                                break;
                            }
                        }
                        // TOP
                        c = input[l-1][n.Item1 + i];
                        if ((!Char.IsDigit(c)) && (c != '.'))
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            result += n.Item2;
                            break;
                        }
                        //TOP RIGHT> Only if last digit and not on end column
                        if (i == numLength - 1 && n.Item1 + i + 1 < input[l].Length)
                        {
                            c = input[l-1][n.Item1 + 1 + i];
                            if ((!Char.IsDigit(c)) && (c != '.'))
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                result += n.Item2;
                                break;
                            }
                        }
                    }
                    // else skip
                    // Check row below
                    if (l < input[l].Length - 1)
                    {
                        //BOT LEFT? Only if first digit, and not on home column
                        if (i == 0 && n.Item1 > 0)
                        {
                            c = input[l+1][n.Item1 - 1];
                            if ((!Char.IsDigit(c)) && (c != '.'))
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                result += n.Item2;
                                break;
                            }
                        }
                        // Bot
                        c = input[l+1][n.Item1 + i];
                        if ((!Char.IsDigit(c)) && (c != '.'))
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            result += n.Item2;
                            break;
                        }
                        //BOT RIGHT> Only if last digit and not on end column
                        if (i == numLength - 1 && n.Item1 + i + 1 < input[l].Length)
                        {
                            c = input[l+1][n.Item1 + 1 + i];
                            if ((!Char.IsDigit(c)) && (c != '.'))
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                result += n.Item2;
                                break;
                            }
                        }
                    }
                    // Left or right
                    //LEFT? Only if first digit, and not on home column
                    if (i == 0 && n.Item1 > 0)
                    {
                        c = input[l][n.Item1 - 1];
                        if ((!Char.IsDigit(c)) && (c != '.'))
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            result += n.Item2;
                            break;
                        }
                    }
                    //RIGHT> Only if last digit and not on end column
                    if (i == numLength - 1 && n.Item1 + i + 1 < input[l].Length)
                    {
                        c = input[l][n.Item1 + 1 + i];
                        if ((!Char.IsDigit(c)) && (c != '.'))
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            result += n.Item2;
                            break;
                        }
                    }
                }
            }
        }

        static List<(int,int)> ExtractNumbers(string input)
        {
            List<(int, int)> numbersList = new();
            
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(input);
            
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Value, out int number))
                {
                    numbersList.Add((match.Index, number));
                }
            }

            return numbersList;
        }
        Console.WriteLine(result.ToString());
    }

    public static void Solve2()
    {
        List<string> input = ParseInput();
        Dictionary<(int,int), int> gearDict = new Dictionary<(int, int),int>();
        int result = 0;
        for(int l = 0; l < input.Count; l++)
        {
            // Console.WriteLine(l);
            //All numbers from 1 line
            List<(int,int)> nums = ExtractNumbers(input[l]);
            foreach((int,int) n in nums)
            {
                int numLength = n.Item2.ToString().Length;
                // Console.WriteLine(n.Item1 + ": " + n.Item2 + ", " + numLength);
                for (int i = 0; i < numLength; i++)
                {
                    char c;
                    //Check row above only if not on top row
                    if (l > 0)
                    {
                        //TOP LEFT? Only if first digit, and not on home column
                        if (i == 0 && n.Item1 > 0)
                        {
                            c = input[l-1][n.Item1 - 1];
                            if (c == '*')
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                (int,int) coord = (l-1,n.Item1-1);
                                if (gearDict.ContainsKey(coord))
                                    gearDict[coord] *= -n.Item2;
                                else 
                                    gearDict.Add(coord, -n.Item2);
                                break;
                            }
                        }
                        // TOP
                        c = input[l-1][n.Item1 + i];
                        if (c == '*')
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            (int,int) coord = (l-1,n.Item1+i);
                            if (gearDict.ContainsKey(coord)){
                                gearDict[coord] *= -n.Item2;}
                            else 
                                gearDict.Add(coord, -n.Item2);
                            break;
                        }
                        //TOP RIGHT> Only if last digit and not on end column
                        if (i == numLength - 1 && n.Item1 + i + 1 < input[l].Length)
                        {
                            c = input[l-1][n.Item1 + 1 + i];
                            if (c == '*')
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                (int,int) coord = (l-1,n.Item1+i+1);
                                if (gearDict.ContainsKey(coord))
                                    gearDict[coord] *= -n.Item2;
                                else 
                                    gearDict.Add(coord, -n.Item2);
                                break;
                            }
                        }
                    }
                    // else skip
                    // Check row below
                    if (l < input[l].Length - 1)
                    {
                        //BOT LEFT? Only if first digit, and not on home column
                        if (i == 0 && n.Item1 > 0)
                        {
                            c = input[l+1][n.Item1 - 1];
                            if (c == '*')
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                (int,int) coord = (l+1,n.Item1-1);
                                if (gearDict.ContainsKey(coord))
                                    gearDict[coord] *= -n.Item2;
                                else 
                                    gearDict.Add(coord, -n.Item2);
                                break;
                            }
                        }
                        // Bot
                        c = input[l+1][n.Item1 + i];
                        if (c == '*')
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            (int,int) coord = (l+1,n.Item1+i);
                            if (gearDict.ContainsKey(coord))
                                gearDict[coord] *= -n.Item2;
                            else 
                                gearDict.Add(coord, -n.Item2);
                            break;
                        }
                        //BOT RIGHT> Only if last digit and not on end column
                        if (i == numLength - 1 && n.Item1 + i + 1 < input[l].Length)
                        {
                            c = input[l+1][n.Item1 + 1 + i];
                            if (c == '*')
                            {
                                // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                                (int,int) coord = (l+1,n.Item1+i+1);
                                if (gearDict.ContainsKey(coord))
                                    gearDict[coord] *= -n.Item2;
                                else 
                                    gearDict.Add(coord, -n.Item2);
                                break;
                            }
                        }
                    }
                    // Left or right
                    //LEFT? Only if first digit, and not on home column
                    if (i == 0 && n.Item1 > 0)
                    {
                        c = input[l][n.Item1 - 1];
                        if (c == '*')
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            (int,int) coord = (l,n.Item1-1);
                            if (gearDict.ContainsKey(coord))
                                gearDict[coord] *= -n.Item2;
                            else 
                                gearDict.Add(coord, -n.Item2);
                            break;
                        }
                    }
                    //RIGHT> Only if last digit and not on end column
                    if (i == numLength - 1 && n.Item1 + i + 1 < input[l].Length)
                    {
                        c = input[l][n.Item1 + 1 + i];
                        if (c == '*')
                        {
                            // Console.WriteLine($"Added {n.Item2}@{n.Item1} to results");
                            (int,int) coord = (l,n.Item1+1+i);
                            if (gearDict.ContainsKey(coord))
                                gearDict[coord] *= -n.Item2;
                            else 
                                gearDict.Add(coord, -n.Item2);
                            break;
                        }
                    }
                }
            }
        }

        static List<(int,int)> ExtractNumbers(string input)
        {
            List<(int, int)> numbersList = new();
            
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(input);
            
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Value, out int number))
                {
                    numbersList.Add((match.Index, number));
                }
            }

            return numbersList;
        }

        result = gearDict.Values.Where(value => value > 0).Sum();
        foreach (var key in gearDict.Keys)
        {
            // Console.WriteLine($"Key:{key}; ${gearDict[key]}");
        }
        Console.WriteLine(result.ToString());
    }

    private static List<string> ParseInput()
    {
        string filePath = "src/Day3/3.in";
        using StreamReader sr = new(filePath);
        string line;
        List<string> input = new();
        while ((line = sr.ReadLine()) != null)
        {        
            input.Add(line);
        }
        return input;
    }       
}