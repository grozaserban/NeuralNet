using System;
using System.Collections.Generic;
using System.IO;
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

        private Reading()
        { }

        public static Reading MergeReadings(List<Reading> readings)
        {
            var result = new Reading();
            foreach(var reading in readings)
            {
                result.InputValues.AddRange(reading.InputValues);
                result.ExpectedResults.AddRange(reading.ExpectedResults);
            }
            return result;
        }

        private void Read(string path) //@"C:\Users\Serban\Pictures\LeafsVeins\maxPoints.txt"
        {
            string[] lines = File.ReadAllLines(path);
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

        public void CreateTestData(string path)
        {
            var noOfCategories = ExpectedResults[0].Count;
            var itemsInCategory = 10;

            for (int itemToRemove = 0; itemToRemove < itemsInCategory; itemToRemove++)
            {
                List<List<double>> expectedResults = new List<List<double>>();
                List<List<double>> inputValues = new List<List<double>>();

                for (int i = noOfCategories-1; i >= 0; i--)
                {
                    var index = i * itemsInCategory + itemToRemove;

                    try
                    {
                        inputValues.Add(InputValues[index]);
                        expectedResults.Add(ExpectedResults[index]);

                        InputValues.RemoveAt(index);
                        ExpectedResults.RemoveAt(index);
                    }
                    catch { }
                }

                var trainDataPath = path + "data" + itemToRemove + ".txt";
                var trainDataAveragePath = path + "average" + itemToRemove + ".txt";
                var testDataPath = path + "test" + itemToRemove + ".txt";

                string dataToWrite = noOfCategories.ToString() + Environment.NewLine;
                for (int i = 0; i < noOfCategories; i++)
                {
                    var averages = new List<double>();
                    int averagesNo = 1;
                    averages = InputValues[i * itemsInCategory].ToList();
                    for (int itemInCategory = 1; itemInCategory < itemsInCategory; itemInCategory++)
                    {
                        var index = i * itemsInCategory + itemInCategory;
 
                        for (int x = 0; x < InputValues[0].Count; x++)
                        {
                            try
                            {
                                averages[x] += InputValues[index][x];
                                averagesNo++;
                            }
                            catch { }
                        }
                    }
                    averages = averages.Select(average => average / averagesNo).ToList();
                    dataToWrite += GetExpectedCategory(ExpectedResults[i * itemsInCategory + 2]) +
                                   " " +
                                   string.Join(" ", string.Join(" ",averages)) +
                                   Environment.NewLine;
                }
                File.AppendAllText(trainDataAveragePath, dataToWrite);


                dataToWrite = noOfCategories.ToString() + Environment.NewLine;
                for (int itemIndex = 0; itemIndex < InputValues.Count; itemIndex++)
                {
                    dataToWrite +=
                        GetExpectedCategory(ExpectedResults[itemIndex]) +
                         " " +
                         string.Join(" ", InputValues[itemIndex]) +
                         Environment.NewLine;
                }
                File.AppendAllText(trainDataPath, dataToWrite);

                dataToWrite = noOfCategories + Environment.NewLine;
                for (int itemIndex = 0; itemIndex < inputValues.Count; itemIndex++)
                {
                    dataToWrite +=
                        GetExpectedCategory(expectedResults[itemIndex]) +
                         " " +
                         string.Join(" ", inputValues[itemIndex]) +
                         Environment.NewLine;
                }
                File.AppendAllText(testDataPath, dataToWrite);

                for (int i = 0; i < noOfCategories; i++)
                {
                    var index = i * itemsInCategory + itemToRemove;

                    try
                    {
                        inputValues.Add(InputValues[index]);
                        expectedResults.Add(ExpectedResults[index]);

                        InputValues.Insert(index, inputValues.Last());
                        ExpectedResults.Insert(index, expectedResults.Last());

                        inputValues.RemoveAt(inputValues.Count - 1);
                        expectedResults.RemoveAt(expectedResults.Count - 1);
                    }
                    catch { }
                }
            }
        }

        public static void SplitDataInFiles(string sourcePath, string resultFolderPath, int fold)
        {
            var linesList = File.ReadAllLines(sourcePath).ToList();
            int noOfCategories = int.Parse(linesList[0]);
            linesList.RemoveAt(0);

            var itemsInCategory = 25;

            for (int i = 0; i < fold; i++)
            {
                File.AppendAllText(resultFolderPath + "data" + i + ".txt", noOfCategories + Environment.NewLine);
            }

            for (int indexInCategory = 0; indexInCategory < itemsInCategory; indexInCategory++)
            {
                var path = resultFolderPath + "data" + indexInCategory % fold + ".txt";

                for (int category = 0; category < noOfCategories; category++)
                {
                    File.AppendAllText(path, linesList[indexInCategory + category * itemsInCategory] + Environment.NewLine);
                }
            }
        }

        private int GetExpectedCategory(List<double> expectedResults)
        {
            for (int i = 0; i < expectedResults.Count; i++)
            {
                if (expectedResults[i] > 0.5)
                    return i+1;
            }
            return 0;
        }

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