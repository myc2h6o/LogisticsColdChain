namespace GeneticPlanning.Genetic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Planning;

    public class Distribution
    {
        public double Cost { get; private set; }
        public Dictionary<int, List<DistributionUnit>> distribution { get; protected set; } = new Dictionary<int, List<DistributionUnit>>();

        public Distribution(bool empty = false)
        {
            if (empty)
            {
                return;
            }

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


            UpdateCost();
            // [TODO] remove validate in production
            ValidateDistribution();
        }

        public void Mutate()
        {
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
                SwitchTwoCars(carId_1, carId_2);
                double tonnage_1 = Cars.GetCar(carId_1).Tonnage;
                double tonnage_2 = Cars.GetCar(carId_2).Tonnage;
                int smallerTonnageId;
                double smallerTonnage;
                if (tonnage_1 == tonnage_2)
                {
                    return;
                }
                else if (tonnage_1 < tonnage_2)
                {
                    smallerTonnageId = carId_1;
                    smallerTonnage = tonnage_1;
                }
                else
                {
                    smallerTonnageId = carId_2;
                    smallerTonnage = tonnage_2;
                }

                if (!distribution.ContainsKey(smallerTonnageId))
                {
                    return;
                }
                foreach (var unitPair in this.distribution[smallerTonnageId])
                {
                    if(smallerTonnage < unitPair.Weight)
                    {
                        SwitchTwoCars(carId_1, carId_2);
                        return;
                    }
                }
            }
        }

        private void SwitchTwoCars(int carId_1, int carId_2)
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

            Car car = Cars.GetRandomCar();
            int listPos = Utils.Random(units.Count);
            DistributionUnit chosenUnit = units[listPos];
            if(chosenUnit.Weight > car.Tonnage)
            {
                return;
            }
            distribution[carId].Remove(chosenUnit);
            if (distribution[carId].Count == 0)
            {
                distribution.Remove(carId);
            }
            if (distribution.ContainsKey(car.Id))
            {
                int i = 0;
                foreach (var unit in distribution[car.Id])
                {
                    if (unit.OrderId == chosenUnit.OrderId
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
            try
            {
                foreach (var carId in distribution.Keys)
                {
                    Car car = Cars.GetCar(carId);
                }
            }
            catch (InvalidIdException)
            {
                throw new InvalidDistributionException();
            }

            //orders
            var orders = new Dictionary<int, Order>();
            foreach (var unitsPair in this.distribution)
            {
                double tonnage = Cars.GetCar(unitsPair.Key).Tonnage;
                foreach (var unit in unitsPair.Value)
                {
                    if(unit.Weight > tonnage)
                    {
                        throw new InvalidDistributionException();
                    }
                    if (!orders.ContainsKey(unit.OrderId))
                    {
                        orders.Add(unit.OrderId, new Order());
                        orders[unit.OrderId].CargoWeight = 0;
                    }
                    orders[unit.OrderId].CargoWeight += unit.Weight;
                }
            }

            foreach (var order in orders)
            {
                if (order.Value.CargoWeight != Orders.orders[order.Key].CargoWeight)
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

        protected virtual void UpdateCost()
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

        protected virtual double GetUnitsCost(Car car, List<DistributionUnit> units) { return 0.0; }

        public virtual Distribution Copy()
        {
            Distribution copyDistribution = new Distribution(true)
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
