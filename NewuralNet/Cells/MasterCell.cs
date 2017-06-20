using NewuralNet.Interfaces;
using System;
using System.Collections.Generic;

namespace NewuralNet.Cells
{
    public class MasterCell
    {
        public ISimpleCell Cell;

        public List<Ancestor> Ancestors;

        public float Value
        {
            set { Cell.Value = value; }
            get { return Cell.Value; }
        }

        public MasterCell(IEnumerable<ISimpleCell> ancestorCells)
        {
            Cell = new SimpleCell();

            Ancestors = new List<Ancestor>();

            foreach (var cell in ancestorCells)
            {
                Ancestors.Add(new Ancestor(cell));
            }
        }

        public void UpdateValue()
        {
            var previousValue = Value;
            Value = 0;
            float weights = 0;
            foreach (var ancestor in Ancestors)
            {
                Value += ancestor.Value * ancestor.Weight;
                weights += ancestor.Weight;
            }

            Value /= weights;
            Value *= 4;
            Value = Value / (float)(Math.Sqrt(1 + Math.Pow((2 * Value), 2)));
        }

        public void UpdateAncestorWeights()
        {
            if (Value >= 0)
                foreach (var ancestor in Ancestors)
                {
                    if (ancestor.Value > 0)
                        ancestor.IncreaseWeight();
                    else
                        ancestor.DecreaseWeight();
                }
            else
                foreach (var ancestor in Ancestors)
                {
                    if (ancestor.Value < 0)
                        ancestor.IncreaseWeight();
                    else
                        ancestor.DecreaseWeight();
                }
        }
    }
}