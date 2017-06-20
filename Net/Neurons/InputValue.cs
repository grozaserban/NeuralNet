using System;
using System.Collections.Generic;

namespace Net
{
    internal class InputValue : INeuron
    {
        public InputValue(double value)
        {
            Value = value;
        }

        public double Derrivate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<Link> Inputs
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

        public List<Link> Outputs { get; set; } = new List<Link>();

        public double Value { get; set; }

        public double GetDerrivateV(double w, double value)
        {
            throw new NotImplementedException();
        }

        public double GetDerrivateW(double w, double value)
        {
            throw new NotImplementedException();
        }

        public void ResetDerrivates()
        {
        }

        public void ResetValues()
        {
        }
    }
}