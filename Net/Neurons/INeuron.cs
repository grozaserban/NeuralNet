using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
    public interface INeuron
    {
        double Value { get; }
        double Derrivate { get; }
        void ResetValues();
        void ResetDerrivates();
        List<Link> Inputs { get; set; }
        List<Link> Outputs { get; set; }
        double GetDerrivateW(double w, double value);
        double GetDerrivateV(double w, double value);
    }
}
