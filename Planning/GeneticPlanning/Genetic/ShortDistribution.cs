namespace GeneticPlanning.Genetic
{
    using System;
    using System.Collections.Generic;
    using Planning;

    class ShortDistribution : Distribution
    {
        protected override double GetUnitsCost(Car car, List<DistributionUnit> units)
        {
            // tomorrowPlans: L A L B L C L
            // todayUnits: L A B L C D L
            var tomorrowPlans = new List<DistributionUnit>();
            var todayUnits = new List<DistributionUnit>();
            foreach (var unit in units)
            {
                if (todayUnits.Count != 0 && unit.Weight != 0 && unit.OrderId == todayUnits[todayUnits.Count - 1].OrderId && unit.Weight + todayUnits[todayUnits.Count - 1].Weight <= car.Tonnage)
                {
                    todayUnits[todayUnits.Count - 1].AddWeight(unit.Weight);
                }
                else
                {
                    todayUnits.Add(unit.Clone() as DistributionUnit);
                }
            }

            List<DistributionUnit> todayPlans;
            double todayFine = 0.0;
            while (true)
            {
                todayPlans = GetTodayPlanFrom(car, todayUnits);
                todayFine = MoveInvalidToTomorrow(todayUnits, tomorrowPlans);
                if (todayFine >= 0)
                {
                    break;
                }
            }

            double tomorrowFine = tomorrowPlans.Count / 2 * Constant.FineNextDay;
            tomorrowPlans.Insert(0, new DistributionUnit(0, 0.0));
            return todayFine + tomorrowFine + GetFeeWithoutFine(car, todayPlans) + GetFeeWithoutFine(car, tomorrowPlans);
        }

        private List<DistributionUnit> GetTodayPlanFrom(Car car, List<DistributionUnit> todayUnits)
        {
            var result = new List<DistributionUnit>();
            result.Add(new DistributionUnit(0, 0.0));
            double currentWeight = 0.0;
            foreach (var unit in todayUnits)
            {
                if (currentWeight + unit.Weight > car.Tonnage)
                {
                    // use weight = 0.0 to indicate back to freezer
                    result.Add(new DistributionUnit(0, 0.0));
                    currentWeight = 0.0;
                }
                result.Add(unit.Clone() as DistributionUnit);
                currentWeight += unit.Weight;
            }

            result.Add(new DistributionUnit(0, 0.0));
            return result;
        }

        // return total fine at the same time
        // return a negative number if invalid
        private double MoveInvalidToTomorrow(List<DistributionUnit> todayUnits, List<DistributionUnit> tomorrowPlans)
        {
            double result = 0.0;
            DateTime currentTime = Constant.CurrentTime;
            int currentPlaceId = Cars.GetRandomCar().RegisterPlaceId;
            for (int i = 0; i < todayUnits.Count; ++i)
            {
                var unit = todayUnits[i];
                Order order = Orders.GetOrder(unit.OrderId);
                if (unit.Weight == 0.0)
                {
                    currentPlaceId = Cars.GetRandomCar().RegisterPlaceId;
                    currentTime = GetReachTime(currentTime, currentPlaceId, Orders.GetOrder(todayUnits[i - 1].OrderId));
                    currentTime += Constant.LoadTime;
                    continue;
                }

                DateTime reachTime = GetReachTime(currentTime, currentPlaceId, order);
                if (reachTime == currentTime)
                {
                    continue;
                }

                double fineToday = (reachTime > order.MaxTime) ? (Math.Ceiling((reachTime - order.MaxTime).TotalHours) * unit.Weight * Constant.FineHourTon) : 0.0;
                if (reachTime > order.MaxTime + Constant.MaxExceedTime
                    || fineToday > Constant.FineNextDay)
                {
                    todayUnits.Remove(unit);
                    tomorrowPlans.Add(unit.Clone() as DistributionUnit);
                    tomorrowPlans.Add(new DistributionUnit(0, 0.0));
                    return -1;
                }
                result = fineToday;

                currentTime = ((reachTime > order.MinTime) ? reachTime : order.MinTime) + Constant.LoadTime;
                currentPlaceId = order.DstPlaceId;
            }
            return result;
        }

        private double GetFeeWithoutFine(Car car, List<DistributionUnit> units)
        {
            int count = units.Count;
            if (count < 2)
            {
                return 0.0;
            }

            double distance = 0.0;
            double timeCost = 0.0;
            int placeId_1 = (units[0].Weight == 0.0) ? Cars.GetRandomCar().RegisterPlaceId : Orders.GetOrder(units[0].OrderId).DstPlaceId;
            int placeId_2 = placeId_1;
            for (int i = 1; i < count; ++i)
            {
                placeId_1 = placeId_2;
                placeId_2 = (units[i].Weight == 0.0) ? Cars.GetRandomCar().RegisterPlaceId : Orders.GetOrder(units[i].OrderId).DstPlaceId;
                distance += Map.GetDistance(placeId_1, placeId_2);
                timeCost += Map.GetTimeCost(placeId_1, placeId_2);
            }

            return distance * (car.OilCost + Constant.RouteFee) + timeCost * Constant.FreezeFeeShort;
        }

        private DateTime GetReachTime(DateTime time, int currrentPlaceId, Order order)
        {
            if (currrentPlaceId == order.DstPlaceId)
            {
                return time;
            }

            double timeMinute = 0;
            if (currrentPlaceId != order.SrcPlaceId)
            {
                timeMinute = Map.GetTimeCost(currrentPlaceId, order.SrcPlaceId);
            }
            timeMinute += Map.GetTimeCost(order.SrcPlaceId, order.DstPlaceId);
            return time + new TimeSpan(0, (int)timeMinute, 0);
        }

        public override Distribution Copy()
        {
            ShortDistribution copyDistribution = new ShortDistribution()
            {
                distribution = new Dictionary<int, List<DistributionUnit>>()
            };

            foreach (var unitPair in this.distribution)
            {
                var copyUnits = new List<DistributionUnit>();
                foreach (var unit in unitPair.Value)
                {
                    copyUnits.Add(unit.Clone() as DistributionUnit);
                }
                copyDistribution.distribution.Add(unitPair.Key, copyUnits);
            }

            return copyDistribution;
        }
    }
}
