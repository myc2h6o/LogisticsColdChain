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

        public static void PrintResult() {
            switch (Constant.PlanType)
            {
                case ModelType.LONG:
                    PrintResultLong();
                    break;
                case ModelType.SHORT:
                    PrintResultShort();
                    break;
                default:
                    break;
            }
        }

        private static void PrintResultLong()
        {
            Console.WriteLine("[TODO] Result for long situation.");
        }

        private static void PrintResultShort()
        {
            Console.WriteLine("[TODO] Result for short situation.");
        }

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
                    Distribution sonDistribution = distributions[j].Clone() as Distribution;
                    sonDistribution.Mutate();
                    if(sonDistribution.Cost <= distributions[j].Cost)
                    {
                        distributions[j] = sonDistribution;
                    }
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
