using System.Collections.Generic;

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
                Console.WriteLine(hash);
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
        List<(string lab, int pow)>[] boxes = new List<(string, int)>[256];
        for(int i = 0; i < 256; i++)
        {
            boxes[i] = [];
        }
        for(int i = 0; i < inputs.Length; i++)
        {
            byte c = inputs[i];
            // Remove
            if ((char)c == '-')
            {
                // Find my label, remove, move everything else forwards
                hash = GetHash(inputs[i - 2], inputs[i - 1]);
                
                var box = boxes[hash];
                string label = (char)inputs[i - 2] + "" + (char)inputs[i - 1];
                for(int j = 0; j < box.Count; j++)
                {
                    var lens = box[j];
                    if (label == lens.lab)
                    {
                        box.Remove(lens);
                        break;
                    }
                }
            }
            else if ((char)c == '=')
            {
                // Find my label, remove, move everything else forwards
                hash = GetHash(inputs[i - 2], inputs[i - 1]);
                var box = boxes[hash];
                string label = (char)inputs[i - 2] + "" + (char)inputs[i - 1];
                bool added = false;
                for(int j = 0; j < box.Count; j++)
                {
                    var (lab, _) = box[j];
                    if (label == lab)
                    {
                        box[j] = (label,(char)inputs[i + 1] - 48);
                        added = true;
                        break;
                    }
                }
                if (!added)
                    box.Add((label, (char)inputs[i + 1] - 48));
            }
        }

        long total = 0;
        for (int i = 0; i < 256; i++)
        {
            var box = boxes[i];
            for (int j = 0; j < box.Count; j++)
            {
                long add = box[j].pow * (j + 1) * (i + 1) ;
                total += add;
                // Console.WriteLine($"box {i}:{box[j].lab},{box[j].pow},{j + 1} ADD {add}");
            }
        }
        Console.WriteLine(total);
    }

    static byte GetHash(byte fst, byte snd)
    {
        byte hash = 0;
        hash = (byte)((byte)((hash + fst) * 17) % 256);
        hash = (byte)((byte)((hash + snd) * 17) % 256);
        return hash;
    }

    static byte[] ParseInput()
    {
        string filePath = "src/Day15/15b.in";
        byte[] input = File.ReadAllBytes(filePath);
        return input;
    }
}