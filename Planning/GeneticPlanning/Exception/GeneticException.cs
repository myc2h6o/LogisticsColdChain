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

    public class InvalidIdException : Exception
    {
        public int Id;
        public InvalidIdException(int id)
        {
            this.Id = id;
        }
    }

    public class InvalidOrderIdException : InvalidIdException
    {
        public InvalidOrderIdException(int id) : base(id) { }
    }

    public class InvalidCarIdException : InvalidIdException
    {
        public InvalidCarIdException(int id) : base(id) { }
    }

    public class InvalidPlaceIdException : InvalidIdException
    {
        public InvalidPlaceIdException(int id) : base(id) { }
    }
}
