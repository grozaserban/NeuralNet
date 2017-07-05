using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace Net
{
    public static class NetTrainer
    {
        public static int Train(this NeuralNet net, List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
                var untrained = 0;
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    net.ChangeData(inputs[i], expectedResults[i]);
                    var iterationSuccess = net.TrainIteration(performanceThreshold);
                    trained &= iterationSuccess;
                    if (!iterationSuccess)
                        untrained++;
                }
                iteration++;
                if (iteration % 100 == 0)
                    Debug.WriteLine("Performance: " + net.CalculatePerformance() + " untrained images" + untrained);
            } while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public static NeuralNet TrainLimited(this NeuralNet net, List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold, int maxIterations)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
                var untrained = 0;
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    net.ChangeData(inputs[i], expectedResults[i]);
                    var iterationSuccess = net.TrainIteration(performanceThreshold);
                    trained &= iterationSuccess;
                    if (!iterationSuccess)
                        untrained++;
                }
                iteration++;
                if (iteration % 100 == 0)
                    Debug.WriteLine("Performance: " + net.CalculatePerformance() + " untrained images" + untrained);
                if (iteration >= maxIterations)
                    break;
            } while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return net;
        }

        public static int TrainSlow(this NeuralNet net, List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    net.ChangeData(inputs[i], expectedResults[i]);
                    trained &= net.CalculatePerformance() >= performanceThreshold;
                    net.UpdateAdjustment();
                }
                net.AdjustCumulatedWeights();
                net.ResetValues();
                net.ResetDerrivates();
                iteration++;
                if (iteration % 100 == 0)
                    Debug.WriteLine("Performance: " + net.CalculatePerformance());
            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public static int TrainParallel(this NeuralNet net, List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    net.ChangeData(inputs[i], expectedResults[i]);
                    trained &= net.CalculatePerformance() >= performanceThreshold;
                    net.UpdateAdjustmentParallel();
                }
                net.AdjustCumulatedWeightsParallel();
                net.ResetValuesParallel();
                net.ResetDerrivatesParallel();
                iteration++;
                if (iteration % 1000 == 0)
                    Debug.WriteLine("Iteration: " + iteration++ + " performance:" + net.CalculatePerformance());
            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public static int TrainAdaptive(this NeuralNet net, List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            double performanceBefore = -1;
            double performance = -1;
            do
            {
                var setsTrained = inputs.Count;
                trained = true;
                performanceBefore = performance;
                for (int i = 0; i < inputs.Count; i++)
                {
                    net.ChangeData(inputs[i], expectedResults[i]);
                    var setTrained = net.CalculatePerformance() >= performanceThreshold;
                    trained &= setTrained;
                    if (setTrained)
                        setsTrained--;
                    net.UpdateAdjustment();
                }
                net.AdjustCumulatedWeights();
                net.ResetValues();
                net.ResetDerrivates();
                performance = net.CalculatePerformance();
                if (performance - performanceBefore < 0.0000001)
                    Link.RenewalFactor = 0.00003; 
                else
                    Link.RenewalFactor = 0.00000000003;

                if (iteration % 1000 == 0)
                    Debug.WriteLine("Iteration: " + iteration++ + " performance:" + net.CalculatePerformance() + "Sets trained:" + setsTrained);
                iteration++;
            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public static int TrainAdaptive(this NeuralNet net, List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold, int iterationLimit)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            double performanceOnOneBefore = -1;
            double performance = -1;
            List<double> performances;
            do
            {
                performances = new List<double>(inputs.Count);
                var setsTrained = inputs.Count;
                trained = true;
                performanceOnOneBefore = net.CalculatePerformance();
                for (int i = 0; i < inputs.Count; i++)
                {
                    performances.Add(net.CalculatePerformance());
                    net.ChangeData(inputs[i], expectedResults[i]);
                    var setTrained = performances[i] >= performanceThreshold;
                    trained &= setTrained;
                    if (setTrained)
                        setsTrained--;
                    net.UpdateAdjustment();
                }
                net.AdjustCumulatedWeights();
                net.ResetValues();
                net.ResetDerrivates();
                performance = net.CalculatePerformance();
                var performanceIncreased = performance > performanceOnOneBefore;
                var performanceIncreaseUnderThreshold = performance - performanceOnOneBefore < 0.0000000001;
                if (performanceIncreased && performanceIncreaseUnderThreshold) //+2
                    Link.RenewalFactor = 0.000001;
                else
                    Link.RenewalFactor = 0;

                iteration++;
                if (iteration % 1000 == 0)
                    Debug.WriteLine("Iteration: " + iteration++ + " performance:" + net.CalculatePerformance() + "Performance average" + performances.Sum() / performances.Count + " Sets untrained:" + setsTrained);

                if (iteration % iterationLimit == 0)
                    Link.RenewalFactor = 1;
            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public static int TrainAdaptiveWithConfidenceAndPerformance(this NeuralNet net, List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold, int iterationLimit, Reading testData)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            double performanceOnOneBefore = -1;
            double performance = -1;
            List<double> performances;
            do
            {
                performances = new List<double>(inputs.Count);
                var setsTrained = inputs.Count;
                trained = true;
                performanceOnOneBefore = net.CalculatePerformance();
                for (int i = 0; i < inputs.Count; i++)
                {
                    performances.Add(net.CalculatePerformance());
                    net.ChangeData(inputs[i], expectedResults[i]);
                    var setTrained = performances[i] >= performanceThreshold;
                    trained &= setTrained;
                    if (setTrained)
                        setsTrained--;
                    net.UpdateAdjustment();
                }
                net.AdjustCumulatedWeights();
                net.ResetValues();
                net.ResetDerrivates();
                performance = net.CalculatePerformance();
                var performanceIncreased = performance > performanceOnOneBefore;
                var performanceIncreaseUnderThreshold = performance - performanceOnOneBefore < 0.0000000001;
                if (performanceIncreased && performanceIncreaseUnderThreshold) //+2
                    Link.RenewalFactor = 0.000001;
                else
                    Link.RenewalFactor = 0; 

                iteration++;
                if (iteration % 1000 == 0)
                {
                    Debug.WriteLine("Iteration: " + iteration++ + " performance:" + net.CalculatePerformance() + "Performance average" + performances.Sum() / performances.Count + " Sets untrained:" + setsTrained);
                    ComputeResultsAndConfidence(iteration, net, testData);
                }

                if (iteration % iterationLimit == 0)
                    Link.RenewalFactor = 1;
            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public static void ComputeResultsAndConfidence(int iteration, NeuralNet net, Reading testData)
        {
            var testResultsPath = @"C:\Users\Serban\Pictures\20+\testData\zIntermediateDataTestsPerformance.txt";
            var confidencePath = @"C:\Users\Serban\Pictures\20+\testData\zIntermediateDataTestsConfidence.txt";
            string results = "Iteration: " + iteration + Environment.NewLine;
            string confidence = "Iteration: " + iteration + Environment.NewLine;
            double averagePerformance = 0;
            double averageConfidence = 0;

            for (int testSet = 0; testSet < testData.InputValues.Count; testSet++)
            {
                net.ChangeData(testData.InputValues[testSet], testData.ExpectedResults[testSet]);
                results += net.CalculatePerformance() + " ";
                confidence += net.CalculateConfidence() + " ";
                averagePerformance += net.CalculatePerformance();
                averageConfidence += net.CalculateConfidence();
            }
            averagePerformance /= testData.InputValues.Count;
            averageConfidence /= testData.InputValues.Count;

            results += Environment.NewLine +
                "Average performance: " + averagePerformance +
                Environment.NewLine;

            confidence += Environment.NewLine +
                "Average confidence: " + averageConfidence +
                Environment.NewLine;

            File.AppendAllText(testResultsPath, results);
            File.AppendAllText(confidencePath, confidence);
        }

        private static bool TrainIteration(this NeuralNet net, double performanceThreshold)
        {
            var performanceBeforeIteration = net.CalculatePerformance();
            if (!(performanceBeforeIteration > performanceThreshold / 100))
                net.Learn();

            var performance = net.CalculatePerformance();

            return performanceBeforeIteration >= performanceThreshold;
        }
    }
}