using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Helpers;

namespace RgbToSpectrum
{
    public class SimpleSpectrum
    {
        // values read from brian smits paper curve images pixels - approximative for the least
        static readonly double[] Rspectrum = { 0.09559, 0.09559, 0.08824, 0.07353, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.08824, 0.69853, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000 };
        static readonly double[] Gspectrum = { 0.00000, 0.00000, 0.00000, 0.00000, 0.03676, 0.38971, 0.78676, 1.00000, 1.00000, 0.77941, 0.31618, 0.00735, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000 };
        static readonly double[] Bspectrum = { 0.99265, 1.00000, 1.00000, 1.00000, 0.86765, 0.61029, 0.30882, 0.08088, 0.00000, 0.00000, 0.00000, 0.02941, 0.05147, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618 };

        static readonly double[] Cspectrum = { 0.99265, 0.99265, 0.98529, 0.92647, 0.98529, 1.00000, 1.00000, 1.00000, 1.00000, 0.91912, 0.30147, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00735, 0.00735, 0.00735, 0.00735, 0.00735 };
        static readonly double[] Mspectrum = { 1.00000, 1.00000, 1.00000, 1.00000, 0.97794, 0.61765, 0.19853, 0.00000, 0.00000, 0.24265, 0.68382, 0.98529, 1.00000, 1.00000, 1.00000, 0.99265, 0.97794, 0.97794, 0.98529, 0.98529, 0.98529 };
        static readonly double[] Yspectrum = { 0.00735, 0.00735, 0.00000, 0.00000, 0.30147, 0.39706, 0.69853, 0.92647, 1.00000, 1.00000, 1.00000, 0.97059, 0.95588, 0.95588, 0.95588, 0.96324, 0.97059, 0.98529, 0.99265, 1.00000, 1.00000 };

      //static readonly double[] Wspectrum = { 0.98750, 0.99554, 1.05714, 1.07589, 0.89643, 0.99554, 1.11607, 1.07857, 1.00089, 0.84286, 1.01964, 1.04643, 1.05179, 1.06250, 1.06518, 1.06518, 1.06250, 1.06518, 1.06786, 1.06786, 1.06518 };
        static readonly double[] Wspectrum = { 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000 };
        
        public double[] values;


        // R,  G,  B must be between [0,  1]
        SimpleSpectrum(double r,  double g,  double b)
        {
            double Rweigth = 0;
            double Gweigth = 0;
            double Bweigth = 0;
            double Cweigth = 0;
            double Mweigth = 0;
            double Yweigth = 0;
            double Wweigth = 0;

            // TODO implement formula at bottom of http://www.cs.utah.edu/~bes/papers/color/paper-node2.html

            //if( r <= g && r <= b)
            //{
            //    Wweigth = r;
            //    if (r <= g && r <= b)
            //    {
            //        Cweigth = g - r;
            //        Bweigth = b - g;
            //    }
            //    else
            //    {
            //        Cweigth = b - r;
            //        Bweigth = g - b;
            //    }

            //}

            //values = Rspectrum.Zip(Cspectrum, (x, y) => x + y).ToArray();
        }
    }
}
