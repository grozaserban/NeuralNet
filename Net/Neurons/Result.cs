using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Neurons
{
    public class Result : INeuron
    {
        private double? value = null;
        private double? derrivate = null;

        public List<Link> Inputs { get; set; } = new List<Link>();

        public Result(double expectedValue)
        {
            ExpectedValue = expectedValue;
        }

        public double ExpectedValue { get; set; }

        public double Derrivate
        {
            get
            {
                if (!derrivate.HasValue)
                    derrivate = Value * (1 - Value) * (ExpectedValue - Value);

                return derrivate.Value;
            }
        }

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

        public List<Link> Outputs
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
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