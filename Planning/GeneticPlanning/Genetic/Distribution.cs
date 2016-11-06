namespace GeneticPlanning.Genetic
{
    using System.Collections.Generic;
    using Planning;
    using System;

    public class Distribution
    {
        Dictionary<int, List<DistributionUnit>> distribution = new Dictionary<int, List<DistributionUnit>>();

        public Distribution()
        {
            /*
            // For test, sample distribution
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

        private bool IsValidDistribution()
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
                    return false;
                }
            }
            return true;
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
    }
}
