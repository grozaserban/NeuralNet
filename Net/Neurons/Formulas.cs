using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
    public static class Formulas
    {
        public static double Sigmoid(double value)
        {
            return 1 / (1 + Math.Pow(Math.E, -value));
        }
    }
}
