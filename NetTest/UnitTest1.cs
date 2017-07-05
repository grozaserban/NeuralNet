using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace NetTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var inputs0 = new List<double>();
            inputs0.Add(0);
            var expectedResults0 = new List<double>();
            expectedResults0.Add(0.1);
            var inputs1 = new List<double>();
            inputs1.Add(1);
            var expectedResults1 = new List<double>();
            expectedResults1.Add(0.9);
            var sut = new NeuralNet(3, 2, inputs1, expectedResults1);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            sut.TrainIteration(0);
            sut.TrainIteration(0);
            sut.TrainIteration(0);
            sut.TrainIteration(0);
            sut.TrainIteration(0);
            sut.TrainIteration(0);
            sut.TrainIteration(0);
            sut.TrainIteration(0);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var inputs = new List<double>();
            inputs.Add(1);
            var expectedResults = new List<double>();
            expectedResults.Add(0.9);
            var sut = new NeuralNet(3, 2, inputs, expectedResults);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            sut.Train(new List<List<double>>() { inputs }, new List<List<double>>() { expectedResults }, -0.15);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var inputs = new List<List<double>>()
            {
                new List<double>(){ 0.3, 0, 0 },
                new List<double>(){ 0.3, 0.3, 0 },
                new List<double>(){ 0.3, 0.3, 0.3 }
            };

            var expectedResults = new List<List<double>>()
            {
                new List<double>(){ 0.9, 0.1, 0.1 },
                new List<double>(){ 0.1, 0.9, 0.1 },
                new List<double>(){ 0.1, 0.1, 0.9 },
            };
            Link.RenewalFactor = 0.00001;
            Link.Step = 5;
            var sut = new NeuralNet(1, 10, inputs[0], expectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.Train(inputs, expectedResults, -0.01);

            sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafsVeins\Test3.txt", iterations);
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestMethodDecimal()
        {
            var inputs = new List<List<double>>()
            {
                new List<double>(){ 0.5, 0.2, 0.9 },
                new List<double>(){ 0.9, 0.5, 0.2 },
                new List<double>(){ 0.2, 0.9, 0.5 }
            };

            var expectedResults = new List<List<double>>()
            {
                new List<double>(){ 0.9, 0.1, 0.1 },
                new List<double>(){ 0.1, 0.9, 0.1 },
                new List<double>(){ 0.1, 0.1, 0.9 },
            };

            for (int i = 1; i < 4; i++)
            {
                Link.Step = 5 * i;
                //  Link.RenewalFactor *= 10;
                var sut = new NeuralNet(2, 30, inputs[0], expectedResults[0]);
                var iterations = sut.TrainSlow(inputs, expectedResults, -0.01);

                var path = @"C:\Users\Serban\Pictures\LeafsVeins\" + nameof(TestMethodDecimal) + ".txt";
                File.AppendAllText(path,
                    " Iterations: " + iterations +
                    " Step: " + Link.Step +
                    " Renewal: " + Link.RenewalFactor + Environment.NewLine);
                sut.PrintWeights(path, iterations);

                sut = new NeuralNet(2, 30, inputs[0], expectedResults[0]);
                iterations = sut.TrainSlow(inputs, expectedResults, -0.01);

                path = @"C:\Users\Serban\Pictures\LeafsVeins\" + nameof(TestMethodDecimal) + ".txt";

                sut.PrintWeights(path, iterations);
            }
        }

        [TestMethod]
        public void TestMethodDecimalAdaptive()
        {
            var inputs = new List<List<double>>()
            {
                new List<double>(){ 0.5, 0.2, 0.9 },
                new List<double>(){ 0.9, 0.5, 0.2 },
                new List<double>(){ 0.2, 0.9, 0.5 }
            };

            var expectedResults = new List<List<double>>()
            {
                new List<double>(){ 0.9, 0.1, 0.1 },
                new List<double>(){ 0.1, 0.9, 0.1 },
                new List<double>(){ 0.1, 0.1, 0.9 },
            };

            for (int i = 1; i < 4; i++)
            {
                Link.Step = 5 * i;
                //  Link.RenewalFactor *= 10;
                var sut = new NeuralNet(2, 30, inputs[0], expectedResults[0]);
                var iterations = sut.TrainAdaptive(inputs, expectedResults, -0.01);

                var path = @"C:\Users\Serban\Pictures\LeafsVeins\" + nameof(TestMethodDecimalAdaptive) + ".txt";
                File.AppendAllText(path,
                    " Iterations: " + iterations +
                    " Step: " + Link.Step +
                    " Renewal: " + Link.RenewalFactor + Environment.NewLine);
                sut.PrintWeights(path, iterations);

                sut = new NeuralNet(2, 30, inputs[0], expectedResults[0]);
                iterations = sut.TrainAdaptive(inputs, expectedResults, -0.01);

                path = @"C:\Users\Serban\Pictures\LeafsVeins\" + nameof(TestMethodDecimalAdaptive) + ".txt";

                sut.PrintWeights(path, iterations);
            }
        }

        [TestMethod]
        public void TestMethod32()
        {
            var inputs = new List<List<double>>()
            {
                new List<double>(){ 1, 0, 0 },
                new List<double>(){ 1, 1, 0 },
                new List<double>(){ 1, 1, 1 }
            };

            var expectedResults = new List<List<double>>()
            {
                new List<double>(){ 0.9, 0.1, 0.1 },
                new List<double>(){ 0.1, 0.9, 0.1 },
                new List<double>(){ 0.1, 0.1, 0.9 },
            };

            //     var sut = new NeuralNet(2, 30, inputs[0], expectedResults[0]);

            //      Debug.WriteLine("Performance is: " + sut.CalculatePerformance());

            // for (int i = -1; i < 4; i++)
            for (int j = -2; j < 2; j++)
            {
                Link.Step = 5 * Math.Pow(10, 0);
                Link.RenewalFactor = 0.000003 * Math.Pow(10, j);
                var iterations = new NeuralNet(2, 30, inputs[0], expectedResults[0]).TrainSlow(inputs, expectedResults, -0.01);

                File.AppendAllText(@"C:\Users\Serban\Pictures\LeafsVeins\Test32.txt",
                    "Iterations: " + iterations +
                    " Step: " + Link.Step +
                    " Renewal: " + Link.RenewalFactor + Environment.NewLine);

                Link.Step *= 5;
                Link.RenewalFactor *= 5;
                iterations = new NeuralNet(2, 30, inputs[0], expectedResults[0]).TrainSlow(inputs, expectedResults, -0.01);

                File.AppendAllText(@"C:\Users\Serban\Pictures\LeafsVeins\Test32.txt",
                    "Iterations: " + iterations +
                    " Step: " + Link.Step +
                    " Renewal: " + Link.RenewalFactor + Environment.NewLine);
            }
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestMethod33()
        {
            var inputs = new List<List<double>>()
            {
                new List<double>(){ 1, 0, 0 },
                new List<double>(){ 1, 1, 0 },
                new List<double>(){ 1, 1, 1 }
            };

            var expectedResults = new List<List<double>>()
            {
                new List<double>(){ 0.9, 0.1, 0.1 },
                new List<double>(){ 0.1, 0.9, 0.1 },
                new List<double>(){ 0.1, 0.1, 0.9 },
            };

            var iterations = new NeuralNet(2, 30, inputs[0], expectedResults[0])
                .TrainParallel(inputs, expectedResults, -0.01);

            File.AppendAllText(@"C:\Users\Serban\Pictures\LeafsVeins\Test33.txt",
                "Iterations: " + iterations +
                " Step: " + Link.Step +
                " Renewal: " + Link.RenewalFactor + Environment.NewLine);

            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestMethod4()
        {
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                var inputs = new Random().NextDouble();
                Debug.WriteLine("Random is: " + RandomNumberProvider.Next());
            }
            Debug.WriteLine("End");
        }

        [TestMethod]
        public void TotallyNotTest()
        {
            var data = new Reading(@"C:\Users\Serban\Pictures\LeafsVeins\maxPoints.txt");

            var sut = new NeuralNet(3, 30, data.InputValues[0], data.ExpectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.TrainSlow(data.InputValues, data.ExpectedResults, -0.02);

            sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafsVeins\Test5.txt", iterations);
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TotallyNotTest21()
        {
            var data = new Reading(@"C:\Users\Serban\Pictures\LeafsVeins\maxPoints.txt");
            data.InputValues = data.InputValues.GetRange(0, 22);

            Link.RenewalFactor = 0.00001;
            Link.Step = 5;
            var sut = new NeuralNet(3, 30, data.InputValues[0], data.ExpectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.TrainAdaptive(data.InputValues, data.ExpectedResults, -0.02);

            sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafsVeins\TestItems21.txt", iterations);
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestHogAverages()
        {
            var data = new Reading(@"C:\Users\Serban\Pictures\LeafHogs\HistogramsAveragesDouble.txt");

            //     Link.RenewalFactor = 0.000003;
            //    Link.Step = 0.2;
            Link.RenewalFactor = 0.00001;
            Link.Step = 5;
            var sut = new NeuralNet(2, 10, data.InputValues[0], data.ExpectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.Train(data.InputValues, data.ExpectedResults, -0.02);

            sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafHogs\TestHistogramsAveragesDouble.txt", iterations);
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestHog()
        {
            var data = new Reading(@"C:\Users\Serban\Pictures\LeafHogs\HistogramsDouble.txt");

            Link.RenewalFactor = 0.00001;
            Link.Step = 5;
            var sut = new NeuralNet(3, 20, data.InputValues[0], data.ExpectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.Train(data.InputValues, data.ExpectedResults, -0.02);

            sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafHogs\TestHistogramsDouble.txt", iterations);
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestHogAux()
        {
            var data = new Reading(@"C:\Users\Serban\Pictures\LeafHogs\testData\data0.txt");

            Link.RenewalFactor = 0.00001;
            Link.Step = 0.05;
            var sut = new NeuralNet(2, 20, data.InputValues[0], data.ExpectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.TrainAdaptive(data.InputValues, data.ExpectedResults, -0.02);

            //        sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafHogs\TestHistogramsDouble.txt", iterations);
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestHogOnAllAverages()
        {
            var testResultsPath = @"C:\Users\Serban\Pictures\LeafHogs\testData\zAverageTestsPerformance.txt";
            var confidencePath = @"C:\Users\Serban\Pictures\LeafHogs\testData\zAverageTestsConfidence.txt";
            for (int i = 0; i < 10; i++) // number of data sets
            {
                var path = @"C:\Users\Serban\Pictures\LeafHogs\testData\average" + i + ".txt";
                var data = new Reading(path);

                Link.RenewalFactor = 0.00001;
                Link.Step = 0.5;
                var sut = new NeuralNet(2, 10, data.InputValues[0], data.ExpectedResults[0]);

                var iterations = sut.TrainAdaptive(data.InputValues, data.ExpectedResults, -0.005); //put iteration limit?

                sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafHogs\testData\trainedOnAverageWeights" + i + ".txt", iterations);

                var testDataPath = @"C:\Users\Serban\Pictures\LeafHogs\testData\test" + i + ".txt";
                var testData = new Reading(testDataPath);

                string results = (i + 1).ToString() + " ";
                string confidence = (i + 1).ToString() + " ";

                for (int testSet = 0; testSet < testData.InputValues.Count; testSet++)
                {
                    sut.ChangeData(testData.InputValues[testSet], testData.ExpectedResults[testSet]);
                    results += sut.CalculatePerformance() + " ";
                    confidence += sut.CalculateConfidence() + " ";
                }

                results += Environment.NewLine;
                confidence += Environment.NewLine;

                File.AppendAllText(testResultsPath, results);
                File.AppendAllText(confidencePath, confidence);
            }
        }

        [TestMethod]
        public void TestHogOnAllData()
        {
            var testResultsPath = @"C:\Users\Serban\Pictures\LeafHogs\testData\zDataTestsPerformance.txt";
            var confidencePath = @"C:\Users\Serban\Pictures\LeafHogs\testData\zDataTestsConfidence.txt";
            for (int i = 0; i < 10; i++) // number of data sets
            {
                var path = @"C:\Users\Serban\Pictures\LeafHogs\testData\data" + i + ".txt";
                var data = new Reading(path);
                Link.RenewalFactor=0;
                Link.Step = 0.1;
                var sut = new NeuralNet(2, 10, data.InputValues[0], data.ExpectedResults[0]);

                var iterations = sut.TrainAdaptive(data.InputValues, data.ExpectedResults, -0.00005, 250000); //put iteration limit?

                sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafHogs\testData\trainedOnData" + i + ".txt", iterations);

                var testDataPath = @"C:\Users\Serban\Pictures\LeafHogs\testData\test" + i + ".txt";
                var testData = new Reading(testDataPath);

                string results = (i + 1).ToString() + " ";
                string confidence = (i + 1).ToString() + " ";

                for (int testSet = 0; testSet < testData.InputValues.Count; testSet++)
                {
                    sut.ChangeData(testData.InputValues[testSet], testData.ExpectedResults[testSet]);
                    results += sut.CalculatePerformance() + " ";
                    confidence += sut.CalculateConfidence() + " ";
                }

                results += Environment.NewLine;
                confidence += Environment.NewLine;

                File.AppendAllText(testResultsPath, results);
                File.AppendAllText(confidencePath, confidence);
            }
        }

        [TestMethod]
        public void TestHogOnAllNewData()
        {
            var testResultsPath = @"C:\Users\Serban\Pictures\20+\testData\zDataTestsPerformance.txt";
            var confidencePath = @"C:\Users\Serban\Pictures\20+\testData\zDataTestsConfidence.txt";
            var folds = 5;

            for (int i = 4; i < folds; i++)
            {
                var allData = ReadAllFiles(folds);
                var testData = allData[i];
                allData.RemoveAt(i);
                var data = Reading.MergeReadings(allData);

                Link.RenewalFactor = 0.000003; // maybe decrease
                Link.Step = 0.05;
                var sut = new NeuralNet(2, 10, data.InputValues[0], data.ExpectedResults[0]);

                var iterations = sut.TrainAdaptive(data.InputValues, data.ExpectedResults, -0.00005, 1000000);

                sut.PrintWeights(@"C:\Users\Serban\Pictures\20+\testData\trainedOnData" + i + ".txt", iterations);

                string results = (i + 1).ToString() + " ";
                string confidence = (i + 1).ToString() + " ";

                for (int testSet = 0; testSet < testData.InputValues.Count; testSet++)
                {
                    sut.ChangeData(testData.InputValues[testSet], testData.ExpectedResults[testSet]);
                    results += sut.CalculatePerformance() + " ";
                    confidence += sut.CalculateConfidence() + " ";
                }

                results += Environment.NewLine;
                confidence += Environment.NewLine;

                File.AppendAllText(testResultsPath, results);
                File.AppendAllText(confidencePath, confidence);
            }
        }
        [TestMethod]
        public void TestHogOnAllNewDataWithPerformanceAndConfidence()
        {

            var folds = 5;

            for (int i = 0; i < folds; i++)
            {
                var allData = ReadAllFiles(folds);
                var testData = allData[i];
                allData.RemoveAt(i);
                var data = Reading.MergeReadings(allData);

                Link.RenewalFactor = 0.000003; // maybe decrease
                Link.Step = 0.05;
                var sut = new NeuralNet(2, 10, data.InputValues[0], data.ExpectedResults[0]);

                var iterations = sut.TrainAdaptiveWithConfidenceAndPerformance(data.InputValues, data.ExpectedResults, -0.00005, 1000000, testData);

                sut.PrintWeights(@"C:\Users\Serban\Pictures\20+\testData\OneLayertrainedOnData" + i + ".txt", iterations);
            }
        }

        private List<Reading> ReadAllFiles(int filesCount)
        {
            List<Reading> readingList = new List<Reading>();
            for (int i = 0; i < filesCount; i++)
            {
                var path = @"C:\Users\Serban\Pictures\20+\testData\data" + i + ".txt";
                readingList.Add(new Reading(path));
            }
            return readingList;
        }

        [TestMethod]
        public void TestCreateTestData()
        {
            var data = new Reading(@"C:\Users\Serban\Pictures\LeafHogs\HistogramsDouble10.txt");

            data.CreateTestData(@"C:\Users\Serban\Pictures\LeafHogs\testData\");
        }

        [TestMethod]
        public void TestCreateTestDataSubsets()
        {
            Reading.SplitDataInFiles(@"C:\Users\Serban\Pictures\20+\HistogramsDouble.txt", @"C:\Users\Serban\Pictures\20+\testData\", 5);
        }
    }
}