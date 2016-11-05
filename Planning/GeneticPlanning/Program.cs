namespace GeneticPlanning
{
    using System;
    using Genetic;
    using Planning;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage [PROGRAM_NAME] [0(long)|1(short)] [INPUT_FOLDER_PATH] [YYYY/MM/DD HH:MM:SS]");
                return;
            }

            try
            {
                InitPlanType(args[0]);
                InitPlanInformation(args[1]);
                InitCurrentTime($"{args[2]} {args[3]}");
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
            catch (InvalidDateTimeException)
            {
                Console.WriteLine($"Wrong while parsing datetime.");
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

        static void InitCurrentTime(string timeStr)
        {
            try {
                Constant.CurrentTime = DateTime.Parse(timeStr);
            }
            catch
            {
                throw new InvalidDateTimeException();
            }
        }
    }
}
