namespace GeneticPlanning.Planning
{
    using System;
    using System.Collections.Generic;

    public static class Map
    {
        private static Dictionary<int, Place> Places;
        private static Dictionary<int, double> Distances;
        private static Dictionary<int, double> TimeCosts;

        public static void Init(string filePath)
        {
            throw new NotImplementedException();
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
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
