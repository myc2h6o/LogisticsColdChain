namespace GeneticPlanning.Genetic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Planning;

    class ShortDistribution : Distribution
    {
        public Dictionary<int, List<DistributionUnit>> TomorrowPlans { get; protected set; } = new Dictionary<int, List<DistributionUnit>>();
        public Dictionary<int, List<DistributionUnit>> TodayPlans { get; protected set; } = new Dictionary<int, List<DistributionUnit>>();

        protected override void UpdateCost()
        {
            this.TomorrowPlans.Clear();
            this.TodayPlans.Clear();
            base.UpdateCost();
        }

        protected override double GetUnitsCost(Car car, List<DistributionUnit> units)
        {
            // tomorrowPlan: L A L B L C L
            // todayUnit: L A B L C D L
            var tomorrowPlan = new List<DistributionUnit>();
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

            List<DistributionUnit> todayPlan;
            double todayFine = 0.0;
            while (true)
            {
                todayPlan = GetTodayPlanFrom(car, todayUnits);
                todayFine = MoveInvalidToTomorrow(todayUnits, todayPlan, tomorrowPlan);
                if (todayFine >= 0)
                {
                    break;
                }
            }

            double tomorrowFine = tomorrowPlan.Count / 2 * Constant.FineNextDay;
            tomorrowPlan.Insert(0, new DistributionUnit(0, 0.0));
            this.TodayPlans.Add(car.Id, todayPlan);
            this.TomorrowPlans.Add(car.Id, tomorrowPlan);
            return todayFine + tomorrowFine + GetFeeWithoutFine(car, todayPlan) + GetFeeWithoutFine(car, tomorrowPlan);
        }

        // units in plan generated is same object from todayUnits
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
                result.Add(unit);
                currentWeight += unit.Weight;
            }

            result.Add(new DistributionUnit(0, 0.0));
            return result;
        }

        // return total fine at the same time
        // return a negative number if invalid
        private double MoveInvalidToTomorrow(List<DistributionUnit> todayUnits, List<DistributionUnit> todayPlan, List<DistributionUnit> tomorrowPlan)
        {
            if(todayPlan.Count < 3)
            {
                // L -> L
                return 0.0;
            }

            double result = 0.0;
            DateTime currentTime = Constant.CurrentTime;
            int currentPlaceId = Cars.GetRandomCar().RegisterPlaceId;
            for (int i = 1; i < todayPlan.Count; ++i)
            {
                var planUnit = todayPlan[i];
                if (planUnit.Weight == 0.0)
                {
                    currentPlaceId = Cars.GetRandomCar().RegisterPlaceId;
                    currentTime = GetReachTime(currentTime, currentPlaceId, Orders.GetOrder(todayPlan[i - 1].OrderId));
                    currentTime += Constant.LoadTime;
                    continue;
                }

                Order order = Orders.GetOrder(planUnit.OrderId);
                DateTime reachTime = GetReachTime(currentTime, currentPlaceId, order);
                if (reachTime == currentTime)
                {
                    continue;
                }

                double fineToday = (reachTime > order.MaxTime) ? (Math.Ceiling((reachTime - order.MaxTime).TotalHours) * planUnit.Weight * Constant.FineHourTon) : 0.0;
                if (reachTime > order.MaxTime + Constant.MaxExceedTime || IsFineExceedNextDay(fineToday))
                {
                    todayUnits.Remove(planUnit);
                    tomorrowPlan.Add(planUnit.Clone() as DistributionUnit);
                    tomorrowPlan.Add(new DistributionUnit(0, 0.0));
                    return -1;
                }
                result += fineToday;

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

        protected virtual bool IsFineExceedNextDay(double fineToday)
        {
            return fineToday > Constant.FineNextDay;
        }

        public override Distribution Copy()
        {
            ShortDistribution copyDistribution = new ShortDistribution()
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

        protected List<DistributionUnit> GetUnitsCopy(List<DistributionUnit> units)
        {
            var result = new List<DistributionUnit>();
            foreach (var unit in units)
            {
                result.Add(unit.Clone() as DistributionUnit);
            }
            return result;
        }
    }
}
