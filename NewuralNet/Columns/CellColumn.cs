using NewuralNet.Cells;
using NewuralNet.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NewuralNet.Columns
{
    public class CellColumn
    {
        private List<MasterCell> _cells;

        public List<ISimpleCell> Cells => _cells.Select(c => c.Cell).ToList();

        public int Size { get; private set; }

        public List<float> Values => _cells.Select(c => c.Value).ToList();

        public CellColumn(int size, List<ISimpleCell> ancestors)
        {
            _cells = new List<MasterCell>();
            for (int i = 0; i < size; i++)
            {
                _cells.Add(new MasterCell(ancestors));
            }
        }

        public CellColumn(int size, List<ISimpleCell> ancestors, float[] results)
        {
            Contract.Assert(Equals(size, results.Count()));

            _cells = new List<MasterCell>();
            for (int i = 0; i < size; i++)
            {
                var cell = new MasterCell(ancestors);
                cell.Cell.Value = results[i];
                _cells.Add(cell);
            }
        }

        public void UpdateValues()
        {
            foreach (var cell in _cells)
            {
                cell.UpdateValue();
            }
        }

        public void UpdateAncestorWeights()
        {
            foreach (var cell in _cells)
            {
                cell.UpdateAncestorWeights();
            }
        }
    }
}