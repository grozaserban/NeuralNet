using System;
using System.Collections.Generic;
using System.Linq;

namespace Net
{
    public class Neuron : INeuron
    {
        private double? value = null;
        private double? derrivate = null;
        private double lastInputsValuesSum = 0;
        private double lastInputsWeightsSum = 0;

        public List<Link> Inputs { get; set; } = new List<Link>();

        public List<Link> Outputs { get; set; } = new List<Link>();

        public double Value
        {
            get
            {
                if (!value.HasValue)
                {
                    lastInputsValuesSum = Inputs.Select(link => link.Value()).Sum();
                    lastInputsWeightsSum = Inputs.Select(link => link.Weight).Sum();
                    value = Formulas.Sigmoid(lastInputsValuesSum / lastInputsWeightsSum);
                }
                return value.Value;
            }
        }

        public double Derrivate
        {
            get
            {
                if (!derrivate.HasValue)
                    derrivate = Outputs.Select(l => l.Derrivate()).Sum() * (1 - Value) * Value;

                return derrivate.Value;
            }
        }

        public double GetDerrivateW(double w, double value)
        {
            var d = lastInputsWeightsSum - w;
            var c = lastInputsValuesSum - value;
            return (value * d - c) / Math.Pow(lastInputsWeightsSum, 2);
//            return (lastInputsWeightsSum * value - lastInputsValuesSum) / Math.Pow(lastInputsWeightsSum, 2);
        }

        public double GetDerrivateV(double w, double value)
        {
            return w / lastInputsWeightsSum;
        }

        public void ResetValues()
        {
            value = null;
        }

        public void ResetDerrivates()
        {
            derrivate = null;
        }
    }
}