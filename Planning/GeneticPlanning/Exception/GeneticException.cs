namespace GeneticPlanning
{
    using System;

    public class InvalidModelTypeException : Exception{ }

    public class InvalidPlanInfoException : Exception
    {
        public string InfoType { get; set; } = "Unknown";
        public InvalidPlanInfoException(string infoType)
        {
            this.InfoType = infoType;
        }
    }

    public class InvalidDateTimeException : Exception { }
}
