using NewuralNet.Interfaces;
using System;

namespace NewuralNet.Cells
{
    public class Ancestor : IAncestor
    {
        public float Weight { get; set; }

        public ISimpleCell Cell { get; set; }

        public float Value
        {
            set { Cell.Value = value; }
            get { return Cell.Value; }
        }

        private static Random generator = new Random((int)DateTime.Now.Ticks);

        public Ancestor(ISimpleCell cell = null)
        {
            Cell = cell ?? new SimpleCell();
            Weight = (float)generator.NextDouble();
        }

        public void DecreaseWeight()
        {
            Weight -= (float)0.05;
            Weight = Weight < 0 ? 0 : Weight;
        }

        public void IncreaseWeight()
        {
            Weight += (float)0.05;
            Weight = Weight > 1 ? 1 : Weight;
        }
    }
}