namespace GeneticPlanning.Planning
{
    using System;

    public enum ModelType { LONG, SHORT}

    public static class Constant
    {
        public static double FreezeFee { get; private set; } = 0.13;    //per hour
        public static double RouteFee { get; private set; } = 2.35;     //per km
        public static double RottenRatio { get; private set; } = 0.15;  //multipled by hour is RottenRate
        public static int FineNextDay { get; private set; } = 50;
        public static int FineHourTon { get; private set; } = 20;       //per hour per ton
        public static ModelType PlanType { get; set; }
        public static DateTime CurrentTime { get; set; }
    }
}
