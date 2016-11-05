namespace GeneticPlanning
{
    using System;
    using Genetic;
    using Planning;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage [PROGRAM_NAME] [0|1] [INPUT_FOLDER_PATH]");
                return;
            }

            try
            {
                InitPlanType(args[0]);
                InitPlanInformation(args[1]);
            }
            catch (InvalidModelTypeException)
            {
                Console.WriteLine("Wrong while parsing plan type.");
                return;
            }
            catch(InvalidPlanInfoException e)
            {
                Console.WriteLine($"Wrong while parsing plan info: {e.InfoType}.");
                return;
            }

            GeneticDistributions.Solve();
            GeneticDistributions.PrintResult();
            Console.ReadLine();
        }

        static void InitPlanType(string typeStr)
        {
            try {
                int planType = int.Parse(typeStr);
                if (!Enum.IsDefined(typeof(ModelType), planType))
                {
                    throw new InvalidModelTypeException();
                }
                Constant.PlanType = (ModelType)planType;
            }
            catch
            {
                throw new InvalidModelTypeException();
            }
        }

        static void InitPlanInformation(string folderPath)
        {
            Map.Init(folderPath + "/map.txt");
            Orders.Init(folderPath + "/order.txt");
            Cars.Init(folderPath + "/car.txt");
        }
    }
}
