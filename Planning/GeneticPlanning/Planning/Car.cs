using System;
using System.IO;

namespace GeneticPlanning.Planning
{
    public static class Cars
    {
        public static void Init(string filePath) {
            StreamReader reader = new StreamReader(filePath);
            string line = reader.ReadLine();
            Console.WriteLine(line);
        }
    }
}
