#nullable disable

static class Day7{

    public static void Solve1()
    {
        string filePath = "src/Day7/7.in";
        using StreamReader sr = new(filePath);
        string line;
        List<Hand> hands2 = new();
        while ((line = sr.ReadLine()) != null)
        {
            hands2.Add(new Hand(line));
        }
        hands2.Sort();
        long sum = 0;
        for (int i = 0; i < hands2.Count; i++)
        {
            sum += hands2[i].Bid * (i + 1);
            // Console.WriteLine($"Sum {sum} after {hands2[i]}");
        }
        Console.WriteLine(sum.ToString());
    }
    public static void Solve2()
    {
        string filePath = "src/Day7/7.in";
        using StreamReader sr = new(filePath);
        string line;
        List<Hand2> hands2 = new();
        while ((line = sr.ReadLine()) != null)
        {
            hands2.Add(new Hand2(line));
        }
        hands2.Sort();
        long sum = 0;
        for (int i = 0; i < hands2.Count; i++)
        {
            sum += hands2[i].Bid * (i + 1);
            // Console.WriteLine($"Sum {sum} after {hands2[i]}");
        }
        Console.WriteLine(sum.ToString());
    }

    public class Hand : IComparable<Hand>
    {
        public Hand(string line)
        {
            var input = line.Split(' ');
            Id  = Convert(input[0]);
            Bid = int.Parse(input[1]);
            Type = GetType();
        }

        private int[] Convert(string input)
        {
            int[] result = new int[5];
            for(int i = 0; i < 5; i++)
            {
                switch (input[i])
                {
                    case 'A': result[i] = 14; break;
                    case 'K': result[i] = 13; break;
                    case 'Q': result[i] = 12; break;
                    case 'J': result[i] = 11; break;
                    case 'T': result[i] = 10; break;
                    case '9': result[i] = 9; break;
                    case '8': result[i] = 8; break;
                    case '7': result[i] = 7; break;
                    case '6': result[i] = 6; break;
                    case '5': result[i] = 5; break;
                    case '4': result[i] = 4; break;
                    case '3': result[i] = 3; break;
                    case '2': result[i] = 2; break;
                    default: break;
                }
            }
            return result;
        }

        private new int GetType()
        {
            // Five of a kind
            if (Id.All(_id => _id == Id[0]))
            {
                return 7;
            }
            int[] distinct = Id.Distinct().ToArray();
            if (distinct.Length == 2)
            {
                //Four of a kind or Full House
                int cnt = Id.Count(_id => _id == distinct[0]);
                if (cnt == 1 || cnt == 4)
                {
                    // Four of a kind
                    return 6;
                }
                else if (cnt == 2 || cnt == 3)
                {
                    // Full House
                    return 5;
                }
            }
            if (distinct.Length == 3)
            {
                //Three of a kind || Two pair
                int[] id = new int[5];
                Id.CopyTo(id,0);
                Array.Sort(id);
                int current = 1;
                for (int i = 1; i < Id.Length; i++)
                {
                    if (id[i] == id[i-1])
                    {
                        current++;
                        if(current == 3)
                        {
                            return 4;
                        }
                    }
                    else{
                        current = 1;
                    }
                }
                return 3;
            }
            if (distinct.Length == 4)
            {
                //Pair
                return 2;
            }
            // High Card
            return 1;

// AAAAA 1
// AAAAK 2
// AAAKK 3
// AAAKQ 4
// AAKKQ 5
// AAKQJ 6
// AKQJT 7 REversed
        }

        public int[] Id { get; }
        public int Bid { get; }
        public int Type { get; }

        public override string ToString() => $"({String.Join(',', Id)}, Type {Type}, Bid {Bid})";

        public int CompareTo(Hand other)
        {
            if (other.Type > Type)
                return -1;
            else if (other.Type < Type)
                return 1;
            else
            {
                for(int i = 0; i < Id.Length; i++)
                {
                    if (Id[i] == other.Id[i]) continue;     
                    if(other.Id[i] > Id[i])
                        return -1;
                    else if(other.Id[i] < Id[i])
                        return 1;
                }
                return 0;
            }
        }
    }
    public class Hand2 : IComparable<Hand2>
    {
        public Hand2(string line)
        {
            var input = line.Split(' ');
            Id  = Convert(input[0]);
            Bid = int.Parse(input[1]);
            Type = GetType();
        }

        private int[] Convert(string input)
        {
            int[] result = new int[5];
            for(int i = 0; i < 5; i++)
            {
                switch (input[i])
                {
                    case 'A': result[i] = 14; break;
                    case 'K': result[i] = 13; break;
                    case 'Q': result[i] = 12; break;
                    case 'T': result[i] = 11; break;
                    case '9': result[i] = 10; break;
                    case '8': result[i] = 9; break;
                    case '7': result[i] = 8; break;
                    case '6': result[i] = 7; break;
                    case '5': result[i] = 6; break;
                    case '4': result[i] = 5; break;
                    case '3': result[i] = 4; break;
                    case '2': result[i] = 3; break;
                    case 'J': result[i] = 2; break;
                    default: break;
                }
            }
            return result;
        }

        private new int GetType()
        {
            int jCount = Id.Where(x => x == 2).Select(x => x).Count();
            // Five of a kind
            if (Id.All(_id => _id == Id[0]))
            {
                return 7;
            }
            int[] distinct = Id.Distinct().ToArray();
            if (distinct.Length == 2)
            {
                if (jCount >= 1)
                        return 7;
                //Four of a kind or Full House
                int cnt = Id.Count(_id => _id == distinct[0]);
                if (cnt == 1 || cnt == 4)
                {
                    // Four of a kind
                    return 6;
                }
                else if (cnt == 2 || cnt == 3)
                {
                    // Full House
                    return 5;
                }
            }
            if (distinct.Length == 3)
            {
                //Three of a kind || Two pair
                int[] id = new int[5];
                Id.CopyTo(id,0);
                Array.Sort(id);
                int current = 1;
                for (int i = 1; i < Id.Length; i++)
                {
                    if (id[i] == id[i-1])
                    {
                        current++;
                        if(current == 3)
                        {
                            // Four of a Kind
                            if (jCount >= 1)
                                return 6;
                            //Three of a kind
                            return 4;
                        }
                    }
                    else{
                        current = 1;
                    }
                }
                if (jCount >= 1)
                {
                    // Two pair becomes fullhouse OR four of a kind
                    if (jCount >= 2)
                        return 6;
                    return 5;
                }
                return 3;
            }
            if (distinct.Length == 4)
            {
                if (jCount >= 1)
                    return 4;
                //Pair
                return 2;
            }
            // High Card
            if (jCount >= 1)
                return 2;
            return 1;
        }

        public int[] Id { get; }
        public int Bid { get; }
        public int Type { get; }

        public override string ToString() => $"({String.Join(',', Id)}, Type {Type}, Bid {Bid})";

        public int CompareTo(Hand2 other)
        {
            if (other.Type > Type)
                return -1;
            else if (other.Type < Type)
                return 1;
            else
            {
                for(int i = 0; i < Id.Length; i++)
                {
                    if (Id[i] == other.Id[i]) continue;     
                    if(other.Id[i] > Id[i])
                        return -1;
                    else if(other.Id[i] < Id[i])
                        return 1;
                }
                return 0;
            }
        }
    }
}
