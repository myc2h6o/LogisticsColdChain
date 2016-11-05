namespace GeneticPlanning.Planning
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Map
    {
        private static Dictionary<int, Place> places = new Dictionary<int, Place>();
        private static double[,] distances;
        private static double[,] timeCosts;

        public static void Init(string filePath)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);

                // place
                int count = int.Parse(reader.ReadLine());
                for (int i = 0; i < count; ++i)
                {
                    string line = reader.ReadLine();
                    int pos = 0;
                    int placeId = int.Parse(Utils.SubStringAndMovePos(line, ref pos));
                    Place place = new Place()
                    {
                        Index = i,
                        Name = Utils.SubStringAndMovePos(line, ref pos)
                    };
                    places.Add(placeId, place);
                }

                // distance and time cost
                distances = new double[count, count];
                timeCosts = new double[count, count];
                count = count * (count - 1) / 2;
                for (int i = 0; i < count; ++i)
                {
                    string line = reader.ReadLine();
                    int pos = 0;
                    int srcId = int.Parse(Utils.SubStringAndMovePos(line, ref pos));
                    int dstId = int.Parse(Utils.SubStringAndMovePos(line, ref pos));
                    double distance = double.Parse(Utils.SubStringAndMovePos(line, ref pos));
                    double timeCost = double.Parse(Utils.SubStringAndMovePos(line, ref pos));
                    distances[places[srcId].Index, places[dstId].Index] = distance;
                    distances[places[dstId].Index, places[srcId].Index] = distance;
                    timeCosts[places[srcId].Index, places[dstId].Index] = timeCost;
                    timeCosts[places[dstId].Index, places[srcId].Index] = timeCost;
                }
            }
            catch
            {
                throw new InvalidPlanInfoException("Map");
            }
        }

        public static Place GetPlace(int placeId)
        {
            throw new NotImplementedException();
        }

        public static double GetDistance(int placeId1, int placeId2)
        {
            throw new NotImplementedException();
        }

        public static double GetTimeCost(int placeId1, int placeId2)
        {
            throw new NotImplementedException();
        }
    }

    public class Place
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }
}
