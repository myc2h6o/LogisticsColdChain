namespace GeneticPlanning.Genetic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Planning;

    public class Distribution : ICloneable
    {
        public double Cost { get; private set; }
        public Dictionary<int, List<DistributionUnit>> distribution { get; private set; } = new Dictionary<int, List<DistributionUnit>>();

        public Distribution(bool empty = false)
        {
            if (empty)
            {
                return;
            }

            // test case
            distribution.Add(7, new List<DistributionUnit>());
            distribution.Add(1, new List<DistributionUnit>());
            distribution.Add(8, new List<DistributionUnit>());
            distribution.Add(9, new List<DistributionUnit>());
            distribution[7].Add(new DistributionUnit(1, 1));
            distribution[7].Add(new DistributionUnit(2, 2));
            distribution[1].Add(new DistributionUnit(3, 2));
            distribution[8].Add(new DistributionUnit(5, 3));
            distribution[8].Add(new DistributionUnit(4, 1));
            distribution[9].Add(new DistributionUnit(6, 4));

            /*
            var orders = Orders.orders;
            foreach (var order in orders)
            {
                int orderId = order.Value.Id;
                double orderWeight = order.Value.CargoWeight;
                while (orderWeight > 0)
                {
                    Car car = Cars.GetRandomCar();
                    double givenWeight = Math.Min(orderWeight, car.Tonnage);
                    if (!distribution.ContainsKey(car.Id))
                    {
                        distribution.Add(car.Id, new List<DistributionUnit>());
                    }
                    distribution[car.Id].Add(new DistributionUnit(orderId, givenWeight));
                    orderWeight -= givenWeight;
                }
            }
            */

            UpdateCost();
            // [TODO] remove validate in production
            ValidateDistribution();
        }

        public void Mutate() {
            int mutateType = Utils.Random(3);
            switch (mutateType)
            {
                case 0:
                    SwitchTwoCars();
                    break;
                case 1:
                    SwitchTwoUnitOfSameCar();
                    break;
                case 2:
                    TransformUnitThroughCars();
                    break;
                default:
                    break;
            }

            UpdateCost();
            // [TODO] remove validate in production
            ValidateDistribution();
        }

        private void SwitchTwoCars()
        {
            int carId_1 = Cars.GetRandomCar().Id;
            int carId_2 = Cars.GetRandomCar().Id;
            if (carId_2 != carId_1)
            {
                bool containId_1 = distribution.ContainsKey(carId_1);
                bool containId_2 = distribution.ContainsKey(carId_2);
                if (containId_1 && containId_2)
                {
                    var tmp = distribution[carId_1];
                    distribution[carId_1] = distribution[carId_2];
                    distribution[carId_2] = tmp;
                }
                else if (!containId_1 && containId_2)
                {
                    distribution.Add(carId_1, distribution[carId_2]);
                    distribution.Remove(carId_2);
                }
                else if (containId_1 && !containId_2)
                {
                    distribution.Add(carId_2, distribution[carId_1]);
                    distribution.Remove(carId_1);
                }
            }
        }

        private void SwitchTwoUnitOfSameCar()
        {
            var subDistribution = distribution.Where(_ => _.Value.Count > 1);
            int count = subDistribution.Count();
            if (count == 0)
            {
                return;
            }

            // get two position
            int pos = Utils.Random(count);
            var unitsPair = GetNthElementFrom(subDistribution, pos);
            var units = unitsPair.Value;
            int listCount = units.Count;
            int listPos1 = Utils.Random(listCount);
            int listPos2 = Utils.Random(listCount);
            if (listPos2 == listPos1)
            {
                listPos2 = (listPos2 + 1) % listCount;
            }

            // swap them
            var tmp = units[listPos1];
            units[listPos1] = units[listPos2];
            units[listPos2] = tmp;
            distribution[unitsPair.Key] = units;
        }

        private void TransformUnitThroughCars()
        {
            int count = distribution.Count;
            int pos = Utils.Random(count);
            var unitsPair = GetNthElementFrom(distribution, pos);
            int carId = unitsPair.Key;
            var units = unitsPair.Value;

            int listPos = Utils.Random(units.Count);
            DistributionUnit chosenUnit = units[listPos];
            distribution[carId].Remove(chosenUnit);
            if(distribution[carId].Count == 0)
            {
                distribution.Remove(carId);
            }
            Car car = Cars.GetRandomCar();
            if (distribution.ContainsKey(car.Id))
            {
                int i = 0;
                foreach(var unit in distribution[car.Id])
                {
                    if(unit.OrderId == chosenUnit.OrderId
                        && unit.Weight + chosenUnit.Weight <= car.Tonnage)
                    {
                        distribution[car.Id][i].AddWeight(chosenUnit.Weight);
                        return;
                    }
                    i++;
                }
                distribution[car.Id].Add(chosenUnit);
            }
            else
            {
                distribution.Add(car.Id, new List<DistributionUnit>());
                distribution[car.Id].Add(chosenUnit);
            }
        }

        private void ValidateDistribution()
        {
            //cars
            try {
                foreach (var carId in distribution.Keys)
                {
                    Cars.GetCar(carId);
                }
            }
            catch (InvalidIdException)
            {
                throw new InvalidDistributionException();
            }

            //orders
            var orders = new Dictionary<int, Order>();
            foreach(var unitsPair in this.distribution)
            {
                foreach (var unit in unitsPair.Value)
                {
                    if (!orders.ContainsKey(unit.OrderId))
                    {
                        orders.Add(unit.OrderId, new Order());
                        orders[unit.OrderId].CargoWeight = 0;
                    }
                    orders[unit.OrderId].CargoWeight += unit.Weight;
                }
            }

            foreach(var order in orders)
            {
                if(order.Value.CargoWeight != Orders.orders[order.Key].CargoWeight)
                {
                    throw new InvalidDistributionException();
                }
            }
        }

        private KeyValuePair<int, List<DistributionUnit>> GetNthElementFrom(IEnumerable<KeyValuePair<int, List<DistributionUnit>>> input, int pos)
        {
            int i = 0;
            foreach (var element in input)
            {
                if (i == pos)
                {
                    return element;
                }
                i++;
            }

            throw new InvalidDistributionUnits();
        }

        private void UpdateCost()
        {
            this.Cost = GetCost();
        }

        private double GetCost()
        {
            double cost = 0.0;
            foreach (var unitsPair in this.distribution)
            {
                int carId = unitsPair.Key;
                Car car = Cars.GetCar(carId);
                cost += GetUnitsCost(car, unitsPair.Value);
            }

            return cost;
        }

        private double GetUnitsCost(Car car, List<DistributionUnit> units)
        {
            switch (Constant.PlanType)
            {
                case ModelType.LONG:
                    return GetUnitsCostLong(car, units);
                case ModelType.SHORT:
                    return GetUnitsCostShort(car, units);
                default:
                    return 0.0;
            }
        }

        private double GetUnitsCostLong(Car car, List<DistributionUnit> units)
        {
            double cost = 0.0;
            Car tmpCar = new Car {
                Id = car.Id,
                CurrentPlaceId = car.CurrentPlaceId,
                RegisterPlaceId = car.RegisterPlaceId,
                Tonnage = car.Tonnage,
                OilCost = car.OilCost
            };
            foreach (var unit in units)
            {
                Order order = Orders.GetOrder(unit.OrderId);
                Order tmpOrder = new Order
                {
                    Id = order.Id,
                    SrcPlaceId = order.SrcPlaceId,
                    DstPlaceId = order.DstPlaceId,
                    RottenFine = order.RottenFine,
                    MinTime = order.MinTime,
                    MaxTime = order.MaxTime,
                    CargoWeight = unit.Weight
                };
                cost += GetUnitCostLong(tmpCar, tmpOrder);
                tmpCar.CurrentPlaceId = order.DstPlaceId;
            }

            return cost;
        }

        private double GetUnitCostLong(Car car, Order order)
        {
            double timeCostWithCargo = Map.GetTimeCost(order.SrcPlaceId, order.DstPlaceId);
            double timeCostTotal = Map.GetTimeCost(car.CurrentPlaceId, order.SrcPlaceId) + timeCostWithCargo;
            double distance = Map.GetDistance(car.CurrentPlaceId, order.SrcPlaceId) + Map.GetDistance(order.SrcPlaceId, order.DstPlaceId);
            return distance * (car.OilCost + Constant.RouteFee) + timeCostTotal * Constant.FreezeFeeLong + timeCostWithCargo * (order.RottenFine * order.CargoWeight * Constant.RottenRatio);
        }

        private double GetUnitsCostShort(Car car, List<DistributionUnit> units)
        {
            // tomorrowPlans: L A L B L C L
            // todayUnits: L A B L C D L
            var tomorrowPlans = new List<DistributionUnit>();
            var todayUnits = new List<DistributionUnit>();
            foreach(var unit in units)
            {
                todayUnits.Add(unit.Clone() as DistributionUnit);
            }

            List<DistributionUnit> todayPlans;
            double todayFine = 0.0;
            while (true) {
                todayPlans = GetTodayPlanFrom(car, todayUnits);
                todayFine = MoveInvalidToTomorrow(todayUnits, tomorrowPlans);
                if(todayFine >= 0)
                {
                    break;
                }
            }

            double tomorrowFine = tomorrowPlans.Count / 2 * Constant.FineNextDay;
            tomorrowPlans.Insert(0, new DistributionUnit(0, 0.0));
            return todayFine + tomorrowFine + GetFeeWithoutFineShort(car, todayPlans) + GetFeeWithoutFineShort(car, tomorrowPlans);
        }

        private List<DistributionUnit> GetTodayPlanFrom(Car car, List<DistributionUnit> todayUnits)
        {
            var result = new List<DistributionUnit>();
            result.Add(new DistributionUnit(0, 0.0));
            double currentWeight = 0.0;
            foreach(var unit in todayUnits)
            {
                if(currentWeight + unit.Weight > car.Tonnage)
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
        private double MoveInvalidToTomorrow(List<DistributionUnit> todayUnits, List<DistributionUnit> tomorrowPlans) {
            double result = 0.0;
            DateTime currentTime = Constant.CurrentTime;
            int currentPlaceId = Cars.GetRandomCar().RegisterPlaceId;
            for(int i= 0; i< todayUnits.Count; ++i)
            {
                var unit = todayUnits[i];
                Order order = Orders.GetOrder(unit.OrderId);
                if(unit.Weight == 0.0)
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

        private double GetFeeWithoutFineShort(Car car, List<DistributionUnit> units)
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
            if(currrentPlaceId == order.DstPlaceId)
            {
                return time;
            }

            double timeMinute = 0;
            if(currrentPlaceId != order.SrcPlaceId)
            {
                timeMinute = Map.GetTimeCost(currrentPlaceId, order.SrcPlaceId);
            }
            timeMinute += Map.GetTimeCost(order.SrcPlaceId, order.DstPlaceId);
            return time + new TimeSpan(0, (int)timeMinute, 0);
        }

        public object Clone()
        {
            Distribution copyDistribution = new Distribution(true)
            {
                distribution = new Dictionary<int, List<DistributionUnit>>()
            };

            foreach (var unitPair in this.distribution)
            {
                var copyUnits = new List<DistributionUnit>();
                foreach(var unit in unitPair.Value)
                {
                    copyUnits.Add(unit.Clone() as DistributionUnit);
                }
                copyDistribution.distribution.Add(unitPair.Key, copyUnits);
            }

            return copyDistribution;
        }
    }

    public class DistributionUnit : ICloneable
    {
        private KeyValuePair<int, double> unit;

        public DistributionUnit(int orderId, double weight)
        {
            unit = new KeyValuePair<int, double>(orderId, weight);
        }

        public int OrderId => unit.Key;

        public double Weight => unit.Value;

        public void SetWeight(double weight)
        {
            unit = new KeyValuePair<int, double>(OrderId, weight);
        }

        public void AddWeight(double weight)
        {
            unit = new KeyValuePair<int, double>(OrderId, Weight + weight);
        }

        public object Clone()
        {
            return new DistributionUnit(this.OrderId, this.Weight);
        }
    }
}
