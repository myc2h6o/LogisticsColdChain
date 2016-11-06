namespace GeneticPlanning.Planning
{
    using System.Collections.Generic;
    using System.IO;

    public static class Map
    {
        public static int count { get; private set; }
        public static Dictionary<int, Place> places { get; private set; } = new Dictionary<int, Place>();
        private static double[,] distances;
        private static double[,] timeCosts;

        public static void Init(string filePath)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);

                // place
                count = int.Parse(reader.ReadLine());
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
                for (int i = 0; i < count; ++i)
                {
                    distances[i, i] = 0;
                    timeCosts[i, i] = 0;
                }

                int relationCount = count * (count - 1) / 2;
                for (int i = 0; i < relationCount; ++i)
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
            CheckPlaceId(placeId);
            return places[placeId];
        }

        public static double GetDistance(int placeId1, int placeId2)
        {
            CheckPlaceId(placeId1);
            CheckPlaceId(placeId2);
            return distances[places[placeId1].Index, places[placeId2].Index];
        }

        public static double GetTimeCost(int placeId1, int placeId2)
        {
            CheckPlaceId(placeId1);
            CheckPlaceId(placeId2);
            return timeCosts[places[placeId1].Index, places[placeId2].Index];
        }

        private static void CheckPlaceId(int placeId)
        {
            if (!places.ContainsKey(placeId))
            {
                throw new InvalidPlaceIdException(placeId);
            }
        }
    }

    public class Place
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }
}
