namespace GeneticPlanning.Planning
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Cars
    {
        private static Dictionary<int, Car> cars = new Dictionary<int, Car>();

        public static void Init(string filePath)
        {
            try {
                StreamReader reader = new StreamReader(filePath);
                int count = int.Parse(reader.ReadLine());
                for (int i = 0; i < count; ++i)
                {
                    string line = reader.ReadLine();
                    int pos = 0;
                    Car car = new Car()
                    {
                        Id = int.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        RegisterPlaceId = int.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        CurrentPlaceId = int.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        Tonnage = int.Parse(Utils.SubStringAndMovePos(line, ref pos)),
                        OilCost = double.Parse(Utils.SubStringAndMovePos(line, ref pos))
                    };
                    cars.Add(car.Id, car);
                }
            }
            catch
            {
                throw new InvalidPlanInfoException("Car");
            }
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
