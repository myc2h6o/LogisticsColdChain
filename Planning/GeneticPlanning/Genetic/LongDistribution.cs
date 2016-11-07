namespace GeneticPlanning.Genetic
{
    using System.Collections.Generic;
    using Planning;

    public class LongDistribution : Distribution
    {
        protected override double GetUnitsCost(Car car, List<DistributionUnit> units)
        {
            double cost = 0.0;
            Car tmpCar = new Car
            {
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
                cost += GetUnitCost(tmpCar, tmpOrder);
                tmpCar.CurrentPlaceId = order.DstPlaceId;
            }

            return cost;
        }

        private double GetUnitCost(Car car, Order order)
        {
            double timeCostWithCargo = Map.GetTimeCost(order.SrcPlaceId, order.DstPlaceId);
            double timeCostTotal = Map.GetTimeCost(car.CurrentPlaceId, order.SrcPlaceId) + timeCostWithCargo;
            double distance = Map.GetDistance(car.CurrentPlaceId, order.SrcPlaceId) + Map.GetDistance(order.SrcPlaceId, order.DstPlaceId);
            return distance * (car.OilCost + Constant.RouteFee) + timeCostTotal * Constant.FreezeFeeLong + timeCostWithCargo * (order.RottenFine * order.CargoWeight * Constant.RottenRatio);
        }

        public override Distribution Copy()
        {
            LongDistribution copyDistribution = new LongDistribution()
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
