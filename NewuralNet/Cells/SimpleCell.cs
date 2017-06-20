using NewuralNet.Interfaces;
using System.Diagnostics.Contracts;

namespace NewuralNet.Cells
{
    public class SimpleCell : ISimpleCell
    {
        public float Value { get; set; }

        public SimpleCell()
        {
            Value = float.MaxValue;
        }

        public SimpleCell(float value)
        {
            Contract.Assert((value <= 0.5) && (value >= -0.5));

            Value = value;
        }
    }
}