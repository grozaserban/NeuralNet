using System;
using System.Collections.Generic;
using System.Linq;

namespace Net
{
    public class Reading
    {

        public List<List<double>> ExpectedResults { get; private set; } = new List<List<double>>();

        public List<List<double>> InputValues { get; set; } = new List<List<double>>();

        public Reading(string path)
        {
            Read(path);
        }

        private void Read(string path) //@"C:\Users\Serban\Pictures\LeafsVeins\maxPoints.txt"
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            int categoriesCount = int.Parse(lines[0]);

            var linesList = lines.ToList();
            linesList.RemoveAt(0);
            foreach (var line in linesList)
            {
                List<double> inputs = line
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => double.Parse(s))
                    .ToList();

                var category = (int)inputs[0];
                inputs.RemoveAt(0);

                ExpectedResults.Add(CreateExpectedResults(categoriesCount, category)); // should totally be read
                InputValues.Add(inputs);
            }
        }

   //     private List<double> Adjust(List<double> inputs)
   //     {
   ////         return new List<double>
   //     }

        private List<double> CreateExpectedResults(int size, int category)
        {
            List<double> expectedResults = new List<double>();

            for (int i = 0; i < size; i++)
            {
                expectedResults.Add(0.1);
            }
            expectedResults[category - 1] = 0.9;

            return expectedResults;
        }
    }
}