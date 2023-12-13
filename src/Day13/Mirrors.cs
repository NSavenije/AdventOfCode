#nullable disable
using System.Text;

public static class Day13
{
    public static void Solve1()
    {
        List<List<string>> input = ParseInput("src/Day13/13.in");
        long sum = 0;
        foreach(List<string> ss in input)
        {
            sum += FindMirrorValues(ss, false);
        }
        Console.WriteLine(sum);

    }
    public static void Solve2()
    {
        List<List<string>> input = ParseInput("src/Day13/13.in");
        long sum = 0;
        foreach(List<string> ss in input)
        {
            sum += FindMirrorValues(ss, true);
        }
        Console.WriteLine(sum);
    }

    static long FindMirrorValues(List<string> input, bool smudge)
    {
        long result = 0;
        if (!smudge)
        {
            for(int i = 1; i < input.Count; i++)
            {
                if(input[i - 1] == input[i] && TestMirror(input, i, smudge))
                {
                    result += i * 100;
                    break;
                }
            }
            input = Transpose(input);
            for(int i = 1; i < input.Count; i++)
            {
                if(input[i - 1] == input[i] && TestMirror(input, i, smudge))
                {
                    result += i ;
                    break;
                }
            }
        }
        else
        {
            for(int i = 1; i < input.Count; i++)
            {
                if(CompareString(input[i], input[i-1]) < 2 && TestMirror2(input, i, smudge))
                {
                    result += i * 100;
                    break;
                }
            }
            input = Transpose(input);
            for(int i = 1; i < input.Count; i++)
            {
                if(CompareString(input[i], input[i-1]) < 2 && TestMirror2(input, i, smudge))
                {
                    result += i ;
                    break;
                }
            }
        }
        return result;
    }

    static int CompareString(string a, string b)
    {
        return a.Zip(b, (x, y) => x == y).Count(z => !z);
    }

    static bool TestMirror(List<string> ss, int mirrorIndex, bool smudge)
    {
        // Console.WriteLine(string.Join('\n', ss));
        
        mirrorIndex--;
        // Check left
        if (ss.Count - 1 > mirrorIndex * 2)
        {
            for(int i = 0; i < mirrorIndex; i++)
            {
                // 0123456 // mi 4 - 4 items - rightIndex should be 6789
                int rightIndex = 2 * mirrorIndex + 1;
                // Console.WriteLine($"mi {mirrorIndex} i {i} ri {rightIndex - i} - {ss.Count}");
                if (ss[i] != ss[rightIndex - i])
                {
                    return false;
                }
            }
        }
        else
        {  //01234567
            for(int i = ss.Count - 1; i > mirrorIndex + 1; i--)
            {
                //0123456 - 3 65 == 12
                // 01234567 - count 8 - 2 iteration only 6 and 7 -> 8
                int leftIndex = ss.Count - 1 - (mirrorIndex + 1); // 7 - 4 - 1 = check 2 indices
                // Console.WriteLine($"mi {mirrorIndex} i {i} li {mirrorIndex - leftIndex + (ss.Count - 1 - i)} - {ss.Count}");
                if (ss[i] != ss[mirrorIndex - leftIndex + (ss.Count - 1 - i)])
                {
                    return false;
                }
            }
        }
        return true;
    }

    static bool TestMirror2(List<string> ss, int mirrorIndex, bool smudge)
    {
        // Console.WriteLine(string.Join('\n', ss));
        int smudges = CompareString(ss[mirrorIndex - 1], ss[mirrorIndex]);
        if (smudge && smudges > 1)
        {
            return false;
        }
        mirrorIndex--;
        // Check left
        if (ss.Count - 1 > mirrorIndex * 2)
        {
            for(int i = 0; i < mirrorIndex; i++)
            {
                // 0123456 // mi 4 - 4 items - rightIndex should be 6789
                int rightIndex = 2 * mirrorIndex + 1;
                // Console.WriteLine($"mi {mirrorIndex} i {i} ri {rightIndex - i} - {ss.Count}");
                smudges += CompareString(ss[i], ss[rightIndex - i]);
                if (smudge && smudges > 1)
                {
                    return false;
                }
            }
        }
        else
        {  //01234567
            for(int i = ss.Count - 1; i > mirrorIndex + 1; i--)
            {
                //0123456 - 3 65 == 12
                // 01234567 - count 8 - 2 iteration only 6 and 7 -> 8
                int leftIndex = ss.Count - 1 - (mirrorIndex + 1); // 7 - 4 - 1 = check 2 indices
                // Console.WriteLine($"mi {mirrorIndex} i {i} li {mirrorIndex - leftIndex + (ss.Count - 1 - i)} - {ss.Count}");
                smudges += CompareString(ss[i], ss[mirrorIndex - leftIndex + (ss.Count - 1 - i)]);
                if (smudge && smudges > 1)
                {
                    return false;
                }
            }
        }
        return smudges == 1 ? true : false;
    }
    

    static List<string> Transpose(List<string> rows)
    {
        List<string> result = [];

        for (int columnIndex = 0; columnIndex < rows[0].Length; columnIndex++)
        {
            StringBuilder columnBuilder = new();
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                columnBuilder.Append(rows[rowIndex][columnIndex]);
            }
            result.Add(columnBuilder.ToString());
        }
        return result;
    }

    static List<List<string>> ParseInput(string filePath)
    {
        StreamReader sr = new StreamReader(filePath);
        List<List<string>> input = [];
        input.Add([]);
        int index = 0;
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            if(line == "")
            {
                index++;
                input.Add([]);
            }
            else
            {
                input[index].Add(line);
            }
        }
        return input;
    }
}