namespace GeneticPlanning.Planning
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Cars
    {
        private static Dictionary<int, Car> cars;

        public static void Init(string filePath)
        {
            throw new NotImplementedException();
            //sample
            //StreamReader reader = new StreamReader(filePath);
            //string line = reader.ReadLine();
            //Console.WriteLine(line);
        }

        public static Car GetCar(int carId)
        {
            throw new NotImplementedException();
        }
    }

    public class Car
    {
        public int Id { get; set; }
        public int RegisterPlaceId { get; set; }
        public int CurrentPlaceId { get; set; }
        public int Tonnage { get; set; }
        public double OilCost { get; set; }
    }
}
