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
            Link.Step = 0.2;
            var sut = new NeuralNet(2, 30, inputs[0], expectedResults[0]);

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

            var sut = new NeuralNet(3, 30, data.InputValues[0], data.ExpectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.TrainAdaptive(data.InputValues, data.ExpectedResults, -0.02);

            sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafsVeins\TestItems21.txt", iterations);
            Debug.WriteLine("Done");
        }

        [TestMethod]
        public void TestHogAveragesAdaptive()
        {
            var data = new Reading(@"C:\Users\Serban\Pictures\LeafHogs\HistogramsDouble.txt");

       //     Link.RenewalFactor = 0.000003;
            Link.Step = 0.2;
            var sut = new NeuralNet(2, 10, data.InputValues[0], data.ExpectedResults[0]);

            Debug.WriteLine("Performance is: " + sut.CalculatePerformance());
            var iterations = sut.TrainSlow(data.InputValues, data.ExpectedResults, -0.02);

            sut.PrintWeights(@"C:\Users\Serban\Pictures\LeafHogs\TestHogAveragesAdaptive.txt", iterations);
            Debug.WriteLine("Done");
        }
    }
}