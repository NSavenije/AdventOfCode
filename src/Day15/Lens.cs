using System.Collections.Specialized;
#nullable disable

public static class Day15
{
    public static void Solve1()
    {
        byte[] inputs = ParseInput();
        int total = 0;
        byte hash = 0;
        for(int i = 0; i < inputs.Length; i++)
        {
            byte c = inputs[i];
            
            if ((char)c == ',')
            {
                total += hash;  
                // Console.WriteLine(hash);
                hash = 0;
                continue;
            }

            hash = (byte)((byte)((hash + c) * 17) % 256);
        }
        Console.WriteLine(total + hash);
    }

    public static void Solve2()
    {
        byte[] inputs = ParseInput();
        byte hash = 0;
        string label = "";
        OrderedDictionary[] boxes = new OrderedDictionary[256];
        for(int i = 0; i < 256; i++) { boxes[i] = []; }
        for(int i = 0; i < inputs.Length; i++)
        {
            byte c = inputs[i];
            switch ((char)c)
            {
                case ',': // next lens
                    hash = 0;
                    label = "";
                    break;
                case '-': // remove
                    boxes[hash].Remove(label);
                    break;
                case '=': // add
                    if (boxes[hash].Contains(label))
                        boxes[hash][label] = (char)inputs[i + 1] - 48;  
                    else
                        boxes[hash].Add(label, (char)inputs[i + 1] - 48);   
                    break;
                default: // get hash
                    hash = (byte)((byte)((hash + c) * 17) % 256);
                    label += (char)c;
                    break;
            }
        }

        long total = 0;
        for (int i = 0; i < 256; i++)
        {
            OrderedDictionary box = boxes[i];
            for(int j = 0; j < box.Count; j++)
            {
                int add = (i + 1) * (j + 1) * (int)box[j];
                total += add;
            }
        }
        Console.WriteLine(total);
    }

    static byte[] ParseInput() =>
        File.ReadAllBytes("src/Day15/15.in");
}