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

        for(int i = 1; i < input.Count; i++)
        {
            if(CompareString(input[i], input[i-1]) <= (smudge ? 1 : 0) && TestMirror(input, i, smudge))
            {
                result += i * 100;
                break;
            }
        }
        input = Transpose(input);
        for(int i = 1; i < input.Count; i++)
        {
            if(CompareString(input[i], input[i-1]) <= (smudge ? 1 : 0) && TestMirror(input, i, smudge))
            {
                result += i;
                break;
            }
        }
        return result;
    }

    static int CompareString(string a, string b) =>
        a.Zip(b, (x, y) => x == y).Count(z => !z);

    static bool TestMirror(List<string> ss, int mirrorIndex, bool smudge)
    {
        // Console.WriteLine(string.Join('\n', ss));
        int smudges = CompareString(ss[mirrorIndex - 1], ss[mirrorIndex]);
        if (smudge && smudges > 1)
        {
            return false;
        }
        // Move Mirror to first index
        mirrorIndex--;
        if (ss.Count - 1 > mirrorIndex * 2)
        {
            for(int i = 0; i < mirrorIndex; i++)
            {
                smudges += CompareString(ss[i], ss[2 * mirrorIndex + 1 - i]);
                if (smudges > (smudge ? 1 : 0))
                    return false;
            }
        }
        else
        {  
            for(int i = ss.Count - 1; i > mirrorIndex + 1; i--)
            {
                smudges += CompareString(ss[i], ss[2 * mirrorIndex + 1 - i]);
                if (smudges > (smudge ? 1 : 0))
                    return false;
            }
        }
        return !smudge || smudges == 1;
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