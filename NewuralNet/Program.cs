using NewuralNet.Cells;
using NewuralNet.Columns;
using NewuralNet.Interfaces;
using System;
using System.Collections.Generic;

namespace NewuralNet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputValues = new List<ISimpleCell>();

            CellColumn workerCells;
            var results = new float[3] { (float)0.5,
                0,
                0 };

            inputValues.Add(new SimpleCell((float)0.5));
            inputValues.Add(new SimpleCell((float)0));
            inputValues.Add(new SimpleCell((float)-0.5));

            workerCells = new CellColumn(6, inputValues);
            var expectedResults = new CellColumn(3, workerCells.Cells, results);
            var resultsColumn = new CellColumn(3, workerCells.Cells);

            for (int i = 0; i < 20; i++)
            {
                workerCells.UpdateValues();
                resultsColumn.UpdateValues();

                PrintNetwork(i, inputValues, workerCells, expectedResults,resultsColumn);

                expectedResults.UpdateAncestorWeights();
                workerCells.UpdateAncestorWeights();
            }

            Console.ReadKey();
        }

        public static void PrintNetwork(int index,
            List<ISimpleCell> inputValues,
            CellColumn workerCells,
            CellColumn expectedResultCells,
            CellColumn resultCells)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Generation {index}");

            foreach (var cell in inputValues)
            {
                Console.Write($"{cell.Value} ");
            }

            Console.WriteLine();

            foreach (var cell in workerCells.Cells)
            {
                Console.Write($"{cell.Value} ");
            }

            Console.WriteLine();

            foreach (var cell in resultCells.Cells)
            {
                Console.Write($"{cell.Value} ");
            }
            Console.WriteLine();

            foreach (var cell in expectedResultCells.Cells)
            {
                Console.Write($"{cell.Value} ");
            }
        }
    }
}