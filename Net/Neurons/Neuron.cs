using System;
using System.Collections.Generic;
using System.Linq;

namespace Net
{
    public class Neuron : INeuron
    {
        private double? value = null;
        private double? derrivate = null;

        public List<Link> Inputs { get; set; } = new List<Link>();

        public List<Link> Outputs { get; set; } = new List<Link>();

        public double Value
        {
            get
            {
                if (!value.HasValue)
                {
                    value = Formulas.Sigmoid(GetSumOfInputs);
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

        public void ResetValues()
        {
            value = null;
        }

        public void ResetDerrivates()
        {
            derrivate = null;
        }

        private double GetSumOfInputs => Inputs.Select(link => link.Value()).Sum();
    }
}