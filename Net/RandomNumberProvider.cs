using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net
{
    public static class RandomNumberProvider
    {
        static Random random = new Random();
        public static double Next()
        {
            return random.NextDouble();
        }
    }
}
