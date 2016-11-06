namespace GeneticPlanning.Genetic
{
    using System;

    public static class GeneticDistributions
    {
        private static int GroupCount;
        private static int IterCount;
        private static Distribution[] distributions;

        public static void Solve(int groupCount, int iterCount) {
            GroupCount = groupCount;
            IterCount = iterCount;
            InitSolutionSet();
            Reproduce();
        }

        public static void PrintResult() { }

        private static void InitSolutionSet()
        {
            distributions = new Distribution[GroupCount];
            for (int i = 0; i < GroupCount; ++i)
            {
                distributions[i] = new Distribution();
            }
        }

        private static void Reproduce()
        {
            int iterPrintCount = IterCount / 10;
            for(int i = 0; i < IterCount; ++i)
            {
                for(int j = 0; j < GroupCount; ++j)
                {
                    distributions[j].Mutate();
                }

                if(i % iterPrintCount == 0)
                {
                    PrintProgress(10 * (i / iterPrintCount));
                }
            }
            PrintProgress(100);
        }

        private static void PrintProgress(int percent)
        {
            if (percent != 0)
            {
                Console.WriteLine($"{percent}%");
            }
        }
    }
}
