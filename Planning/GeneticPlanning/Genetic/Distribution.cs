﻿namespace GeneticPlanning.Genetic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Planning;

    public class Distribution
    {
        Dictionary<int, List<DistributionUnit>> distribution = new Dictionary<int, List<DistributionUnit>>();

        public Distribution()
        {
            /*
            // A test case
            distribution.Add(1, new List<DistributionUnit>());
            distribution.Add(2, new List<DistributionUnit>());
            distribution.Add(3, new List<DistributionUnit>());
            distribution.Add(4, new List<DistributionUnit>());
            distribution[1].Add(new DistributionUnit(1, 4.5));
            distribution[2].Add(new DistributionUnit(2, 7.5));
            distribution[2].Add(new DistributionUnit(4, 8.4));
            distribution[3].Add(new DistributionUnit(3, 7));
            distribution[4].Add(new DistributionUnit(5, 9));
            */

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

            // [TODO] remove this after test
            ValidateDistribution();
        }

        private void SwitchTwoCars()
        {
            int carId_1 = Cars.GetRandomCar().Id;
            int carId_2 = Cars.GetRandomCar().Id;
            if (carId_2 == carId_1)
            {
                carId_2 = (carId_2 + 1) % Cars.count;
            }

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

        public double GetCost()
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
            foreach (var unit in units)
            {
                Order order = Orders.GetOrder(unit.OrderId);
                order.CargoWeight = unit.Weight;
                cost += GetUnitCostLong(car, order);
                car.CurrentPlaceId = order.DstPlaceId;
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
            return 0.0;
        }

        private void ValidateDistribution()
        {
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
    }

    public class DistributionUnit
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
    }
}
