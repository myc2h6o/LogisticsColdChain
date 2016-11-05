namespace GeneticPlanning.Planning
{
    using System;
    using System.Collections.Generic;

    public static class Orders
    {
        private static Dictionary<int, Order> orders;

        public static void Init(string filePath)
        {
            throw new NotImplementedException();
        }

        public static Order GetOrder(int orderId)
        {
            throw new NotImplementedException();
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
