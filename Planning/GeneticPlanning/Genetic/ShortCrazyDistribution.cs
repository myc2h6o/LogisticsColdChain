namespace GeneticPlanning.Genetic
{
    using System.Collections.Generic;

    class ShortCrazyDistribution : ShortDistribution
    {
        protected override bool IsFineExceedNextDay(double fineToday)
        {
            return false;
        }

        public override Distribution Copy()
        {
            ShortCrazyDistribution copyDistribution = new ShortCrazyDistribution()
            {
                distribution = new Dictionary<int, List<DistributionUnit>>(),
                TodayPlans = new Dictionary<int, List<DistributionUnit>>(),
                TomorrowPlans = new Dictionary<int, List<DistributionUnit>>()
            };

            foreach (var unitPair in this.distribution)
            {
                var copyUnits = GetUnitsCopy(unitPair.Value);
                copyDistribution.distribution.Add(unitPair.Key, copyUnits);
            }

            foreach (var unitPair in this.TodayPlans)
            {
                var copyUnits = GetUnitsCopy(unitPair.Value);
                copyDistribution.TodayPlans.Add(unitPair.Key, copyUnits);
            }

            foreach (var unitPair in this.TomorrowPlans)
            {
                var copyUnits = GetUnitsCopy(unitPair.Value);
                copyDistribution.TomorrowPlans.Add(unitPair.Key, copyUnits);
            }

            return copyDistribution;
        }
    }
}
