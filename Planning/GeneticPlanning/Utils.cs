namespace GeneticPlanning
{
    using System;

    public static class Utils
    {
        private static Random random = new Random();

        public static string SubStringAndMovePos(string line, ref int pos, char sign = ' ')
        {
            int originPos = pos;
            pos = line.IndexOf(sign, pos) + 1;
            if(pos == 0)
            {
                return line.Substring(originPos);
            }
            return line.Substring(originPos, pos - originPos - 1);
        }

        public static int Random()
        {
            return random.Next();
        }

        public static int Random(int maxValue)
        {
            return random.Next(maxValue);
        }

        public static int Random(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}
