namespace GeneticPlanning
{
    public static class Utils
    {
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
    }
}
