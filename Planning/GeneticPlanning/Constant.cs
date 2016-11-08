namespace GeneticPlanning
{
    using System;

    public enum ModelType { LONG, SHORT, SHORT_CRAZY }

    public static class Constant
    {
        public static double FreezeFeeShort { get; private set; } = 0.13;    //per minute
        public static double FreezeFeeLong { get; private set; } = 8;    //per hour
        public static double RouteFee { get; private set; } = 2.35;     //per km
        public static double RottenRatio { get; private set; } = 0.015;  //multipled by hour is RottenRate
        public static int FineNextDay { get; private set; } = 50;
        public static int FineHourTon { get; private set; } = 20;       //per hour per ton
        public static ModelType PlanType { get; set; }
        public static DateTime CurrentTime { get; set; }
        public static TimeSpan LoadTime { get; set; } = new TimeSpan(0, 30, 0);
        public static TimeSpan MaxExceedTime { get; set; } = new TimeSpan(6, 0, 0);
    }
}
