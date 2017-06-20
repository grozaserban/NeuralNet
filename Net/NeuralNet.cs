using Net.Neurons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Net
{
    public class NeuralNet
    {
        private int _depth;
        private int _width;

        private List<InputValue> _inputs;
        private List<Result> _results;
        private List<List<INeuron>> _neurons;
        private List<List<Link>> _links;

        public NeuralNet(int depth, int width, List<double> inputValues, List<double> expectedResult)
        {
            _depth = depth;
            _width = width;
            CreateNet(depth, width, inputValues, expectedResult);
        }

        public bool TrainIteration(double performanceThreshold)
        {
            var performanceBeforeIteration = CalculatePerformance();
            //if (performanceBeforeIteration < performanceThreshold/4) //added
            //{
                Learn();

                var performance = CalculatePerformance();
            //   Debug.WriteLine("Iteration changed performance from: " + performanceBeforeIteration + " to: " + performance + " for values " + _inputs[1].Value + "  "+ _results[0].Value);
            //    return false;
            //}
            //else
            //{
            //    Debug.WriteLine("Performance " + performanceBeforeIteration + " is under treshold");
            //    return true;
            //}
            //if (performance < performanceBeforeIteration)
            //    Debug.Write("notTraining");
            return performanceBeforeIteration >= performanceThreshold;
        }

        private void Learn()
        {
            AdjustWeights();
            ResetValues();
            ResetDerrivates();
        }

        public int Train(List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
             //   Debug.WriteLine("Iteration: " + iteration++ + " performance:" + CalculatePerformance());
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    ChangeData(inputs[i], expectedResults[i]);
                    trained &= TrainIteration(performanceThreshold);
                }
                iteration++;
                if (iteration % 100 == 0)
                    Debug.WriteLine("Performance: " + CalculatePerformance());
            } while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
//PrintWeights();
        }


        public int TrainSlow(List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
                //   Debug.WriteLine("Iteration: " + iteration++ + " performance:" + CalculatePerformance());
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    ChangeData(inputs[i], expectedResults[i]);
                    trained &= CalculatePerformance() >= performanceThreshold;
                    UpdateAdjustment();
                }
                AdjustCumulatedWeights();
                ResetValues();
                ResetDerrivates();
                iteration++;
                if (iteration%100==0)
                    Debug.WriteLine("Performance: " + CalculatePerformance());
            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public int TrainAdaptive(List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            double performanceBefore = -1;
            double performance = -1;
            do
            {
                //   Debug.WriteLine("Iteration: " + iteration++ + " performance:" + CalculatePerformance());
                trained = true;
                performanceBefore = performance;
                for (int i = 0; i < inputs.Count; i++)
                {
                    ChangeData(inputs[i], expectedResults[i]);
                    trained &= CalculatePerformance() >= performanceThreshold;
                    UpdateAdjustment();
                }
                AdjustCumulatedWeights();
                ResetValues();
                ResetDerrivates();
                performance = CalculatePerformance();
                if (performance - performanceBefore < 0.0000001)
                    Link.RenewalFactor = 0.000003;
                else
                    Link.RenewalFactor = 0.00000000003;

                if (iteration % 100 == 0)
                    Debug.WriteLine("Performance: " + CalculatePerformance());
                iteration++;

            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public int TrainParallel(List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
                //   Debug.WriteLine("Iteration: " + iteration++ + " performance:" + CalculatePerformance());
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    ChangeData(inputs[i], expectedResults[i]);
                    trained &= CalculatePerformance() >= performanceThreshold;
                    UpdateAdjustmentParallel();
                }
                AdjustCumulatedWeightsParallel();
                ResetValuesParallel();
                ResetDerrivatesParallel();
                iteration++;

            }
            while (!trained);
            Debug.WriteLine("Iteration: " + iteration);
            return iteration;
        }

        public void PrintWeights(string path, int iterations)
        {
            File.AppendAllText(path,
                " Iterations: " + iterations +
                " Step: " + Link.Step +
                " Renewal: " + Link.RenewalFactor + Environment.NewLine);
            foreach (var column in _links)
            {
                string createText = "";
                Debug.WriteLine("");
                foreach (var link in column)
                {
                    createText += link.Weight + " ";
                    Debug.Write(link.Weight + " ");
                }

                createText += Environment.NewLine;
                File.AppendAllText(path, createText);
            }
            File.AppendAllText(path, Environment.NewLine);
        }

        public void TrainExtrainfo(List<List<double>> inputs, List<List<double>> expectedResults, double performanceThreshold)
        {
            Contract.Equals(inputs.Count, expectedResults.Count);
            bool trained;
            int iteration = 0;
            do
            {
                var setsTrained = 0;
                Debug.WriteLine("Iteration: " + iteration++ + " performance:" + CalculatePerformance());
                trained = true;
                for (int i = 0; i < inputs.Count; i++)
                {
                    ChangeData(inputs[i], expectedResults[i]);
                    bool setTrained = false;

                    setTrained = TrainIteration(performanceThreshold);


                    trained &= setTrained;
                    if (setTrained)
                        setsTrained++;
                }
                Debug.WriteLine("Iteration: " + iteration++ + " performance:" + CalculatePerformance() + "Sets trained:" + setsTrained);

            } while (!trained);
        }

        public void ChangeData(List<double> inputValues, List<double> expectedResult)
        {
            Contract.Equals(_inputs.Count - 1, inputValues); // because added -1 control
            Contract.Equals(_results.Count, expectedResult.Count);

            for (int i = 1; i < _inputs.Count; i++)
            {
                _inputs[i].Value = inputValues[i - 1];
            }
            for (int i = 0; i < _results.Count; i++)
            {
                _results[i].ExpectedValue = expectedResult[i];
            }
            ResetDerrivates();
            ResetValues();

        }

        /***
         * Negative number, should be as close to 0 as possible
         * ***/
        public double CalculatePerformance()
        {
            //var results = _neurons.Last();
            var performance = 0d;
            for (int i = 0; i < _results.Count; i++)
            {
                performance -= Math.Pow((_results[i].ExpectedValue - _results[i].Value), 2)/2;
            }
            performance /= _results.Count;
            return performance;
        }

        public void ResetValues()
        {
            foreach (var column in _neurons)
            {
                foreach (var neuron in column)
                {
                    neuron.ResetValues();
                }
            }
            foreach (var column in _links)
            {
                foreach (var link in column)
                {
                    link.ResetValue();
                }
            }
        }

        public void ResetValuesParallel()
        {
            Parallel.ForEach(_neurons, column =>
            {
                foreach (var neuron in column)
                {
                    neuron.ResetValues();
                }
            });
            Parallel.ForEach(_links, column =>
            {
                foreach (var link in column)
                {
                    link.ResetValue();
                }
            });
        }

        public void ResetDerrivates()
        {
            foreach (var column in _neurons)
            {
                foreach (var neuron in column)
                {
                    neuron.ResetDerrivates();
                }
            }
            foreach (var column in _links)
            {
                foreach (var link in column)
                {
                    link.ResetDerrivate();
                }
            }
        }

        public void ResetDerrivatesParallel()
        {
            Parallel.ForEach(_neurons, column => 
            {
                foreach (var neuron in column)
                {
                    neuron.ResetDerrivates();
                }
            });
            Parallel.ForEach(_links, column =>
            {
                foreach (var link in column)
                {
                    link.ResetDerrivate();
                }
            });
        }

        public void AdjustWeights()
        {
            foreach (var column in _links)
            {
                foreach (var link in column)
                {
                    link.AdjustWeight();
                }
            }
        }

        public void UpdateAdjustment()
        {
            foreach (var column in _links)
            {
                foreach (var link in column)
                {
                    link.CumulateAdjustment();
                }
            }
        }

        public void UpdateAdjustmentParallel()
        {
            Parallel.ForEach(_links, column =>
            {
                foreach (var link in column)
                {
                    link.CumulateAdjustment();
                }
            });
        }

        public void AdjustCumulatedWeights()
        {
            foreach (var column in _links)
            {
                foreach (var link in column)
                {
                    link.ApplyAdjustment();
                }
            }
        }

        public void AdjustCumulatedWeightsParallel()
        {
            Parallel.ForEach(_links, column =>
            {
                foreach (var link in column)
                {
                    link.ApplyAdjustment();
                }
            });
        }

        private void CreateNet(int depth, int width, List<double> inputValues, List<double> expectedValues)
        {
            _neurons = new List<List<INeuron>>();
            _links = new List<List<Link>>();

           // var iNeuronInputs = ;
            _inputs = CreateInputs(inputValues); // iNeuronInputs.Select(neuron => neuron as InputValue).ToList();
            _neurons.Add(_inputs.Select(neuron => neuron as INeuron).ToList());

            for (int i = 1; i <= depth; i++)
            {
                var linkColumn = new List<Link>();
                _neurons.Add(CreateNeuronColumn(width));
                _links.Add(LinkLayers(_neurons[i - 1], _neurons[i]));
            }

            _results = CreateResultsColumn(expectedValues);
            _neurons.Add(_results.ToList<INeuron>());
            _links.Add(LinkLayers(_neurons[depth], _neurons[depth + 1]));
        }

        private List<Link> LinkLayers(List<INeuron> firstLayer, List<INeuron> secondLayer)
        {
            var linkList = new List<Link>();
            foreach (var firstNeuron in firstLayer)
            {
                foreach (var secondNeuron in secondLayer.Where(neuron => !(neuron is InputValue)))
                {
                    var link = new Link(firstNeuron, secondNeuron);
                    firstNeuron.Outputs.Add(link);
                    secondNeuron.Inputs.Add(link);
                    linkList.Add(link);
                }
            }
            return linkList;
        }

        private List<INeuron> CreateNeuronColumn(int width)
        {
            var list = new List<INeuron>();
            list.Add(new InputValue(-1));
            for (int i = 0; i < width; i++)
            {
                list.Add(new Neuron());
            }
            return list;
        }

        private List<Result> CreateResultsColumn(List<double> expectedValues)
        {
            var list = new List<Result>();
            for (int i = 0; i < expectedValues.Count; i++)
            {
                list.Add(new Result(expectedValues[i]));
            }
            return list;
        }

        private List<InputValue> CreateInputs(List<double> values)
        {
            var list = new List<InputValue>();
            list.Add(new InputValue(-1));
            foreach (var value in values)
            {
                list.Add(new InputValue(value));
            }
            return list;
        }
    }
}