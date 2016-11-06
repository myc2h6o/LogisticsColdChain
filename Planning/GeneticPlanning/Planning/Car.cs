namespace GeneticPlanning.Planning
{
    using System.Collections.Generic;
    using System.IO;

    public static class Cars
    {
        public static int count { get; private set; }
        public static Dictionary<int, Car> cars { get; private set; } = new Dictionary<int, Car>();
        private static Dictionary<int, int> indexId = new Dictionary<int, int>();

        public static void Init(string filePath)
        {
            try {
                StreamReader reader = new StreamReader(filePath);
                count = int.Parse(reader.ReadLine());
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
                    indexId.Add(i, car.Id);
                }
            }
            catch
            {
                throw new InvalidPlanInfoException("Car");
            }
        }

        public static Car GetCar(int carId)
        {
            CheckCarId(carId);
            return cars[carId];
        }

        public static Car GetRandomCar()
        {
            return cars[indexId[Utils.Random(count)]];
        }

        private static void CheckCarId(int carId)
        {
            if (!cars.ContainsKey(carId))
            {
                throw new InvalidCarIdException(carId);
            }
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
