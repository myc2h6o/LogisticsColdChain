namespace GeneticPlanning.Planning
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Orders
    {
        private static Dictionary<int, Order> orders = new Dictionary<int, Order>();

        public static void Init(string filePath)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);
                int count = int.Parse(reader.ReadLine());
                for (int i = 0; i < count; ++i)
                {
                    string line = reader.ReadLine();
                    int pos = 0;
                    Order order = new Order()
                    {
                        Id = int.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        SrcPlaceId = int.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        DstPlaceId = int.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        CargoWeight = double.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        RottenFine = double.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        MinTime = DateTime.Parse($"{Utils.SubStringAndMovePos(line, ref pos)} {Utils.SubStringAndMovePos(line, ref pos)}"),
                        MaxTime = DateTime.Parse($"{Utils.SubStringAndMovePos(line, ref pos)} {Utils.SubStringAndMovePos(line, ref pos)}")
                    };
                    orders.Add(order.Id, order);
                }
            }
            catch
            {
                throw new InvalidPlanInfoException("Order");
            }
        }

        public static Order GetOrder(int orderId)
        {
            CheckOrderId(orderId);
            return orders[orderId];
        }

        private static void CheckOrderId(int orderId)
        {
            if (!orders.ContainsKey(orderId))
            {
                throw new InvalidOrderIdException(orderId);
            }
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public int SrcPlaceId { get; set; }
        public int DstPlaceId { get; set; }
        public double CargoWeight { get; set; }
        public double RottenFine { get; set; } // per ton per rotten rate
        public DateTime MinTime { get; set; }
        public DateTime MaxTime { get; set; }
    }
}
