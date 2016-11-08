namespace GeneticPlanning.Genetic
{
    using System;
    using Planning;

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
            Distribution bestDistribution = GetBestDistribution();
            var bestUnits = bestDistribution.distribution;
            int i = 0;
            foreach (var unitPair in bestUnits)
            {
                i++;
                int currentPlaceId = Cars.GetCar(unitPair.Key).CurrentPlaceId;
                Console.Write($"Car_{i}: ");
                bool isFirst = true;
                foreach (var unit in unitPair.Value)
                {
                    Order order = Orders.GetOrder(unit.OrderId);
                    if (!isFirst)
                    {
                        Console.Write("->");
                    }

                    if(order.SrcPlaceId != currentPlaceId || isFirst)
                    {
                        Console.Write($"{Map.GetPlaceName(order.SrcPlaceId)}->");
                    }
                    Console.Write($"{Map.GetPlaceName(order.DstPlaceId)}");
                    currentPlaceId = order.DstPlaceId;
                    isFirst = false;
                }
                Console.WriteLine();
            }
            Console.WriteLine($"total cost: {bestDistribution.Cost.ToString("F2")}");
        }

        private static void PrintResultShort()
        {
            Distribution bestDistribution = GetBestDistribution();
            var bestUnits = bestDistribution.distribution;
            int i = 0;
            foreach (var unitPair in bestUnits)
            {
                i++;
                int currentPlaceId = unitPair.Key;
                Console.Write($"Car_{i}: ");
                bool isFirst = true;
                foreach (var unit in unitPair.Value)
                {
                    Order order = Orders.GetOrder(unit.OrderId);
                    if (!isFirst)
                    {
                        Console.Write("->");
                    }
                    isFirst = false;

                    if (order.SrcPlaceId != currentPlaceId)
                    {
                        Console.Write($"{Map.GetPlaceName(order.SrcPlaceId)}->");
                    }
                    Console.Write($"{Map.GetPlaceName(order.DstPlaceId)}");
                    currentPlaceId = order.DstPlaceId;
                }
                Console.WriteLine();
            }
            Console.WriteLine($"total cost: {bestDistribution.Cost.ToString("F2")}");
        }

        private static void InitSolutionSet()
        {
            distributions = new Distribution[GroupCount];
            for (int i = 0; i < GroupCount; ++i)
            {
                switch (Constant.PlanType)
                {
                    case ModelType.LONG:
                        distributions[i] = new LongDistribution();
                        break;
                    case ModelType.SHORT:
                        distributions[i] = new ShortDistribution();
                        break;
                    default:
                        throw new InvalidModelTypeException();
                }
            }
        }

        private static void Reproduce()
        {
            int iterPrintCount = IterCount / 10;
            for(int i = 0; i < IterCount; ++i)
            {
                for(int j = 0; j < GroupCount; ++j)
                {
                    Distribution sonDistribution = distributions[j].Copy();
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

        private static Distribution GetBestDistribution()
        {
            int minIndex = 0;
            for(int i = 1; i < GroupCount; ++i)
            {
                if(distributions[i].Cost < distributions[minIndex].Cost)
                {
                    minIndex = i;
                }
            }

            return distributions[minIndex];
        }
    }
}
