namespace GeneticPlanning.Genetic
{
    using System.Collections.Generic;
    using Planning;
    using System;

    public class Distribution
    {
        Dictionary<int, List<DistributionUnit>> distribution = new Dictionary<int, List<DistributionUnit>>();

        public Distribution() {
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
            return 0.0;
        }

        private bool IsValidDistribution()
        {
            var orders = new Dictionary<int, Order>();
            foreach(var units in this.distribution)
            {
                foreach (var unit in units.Value)
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
