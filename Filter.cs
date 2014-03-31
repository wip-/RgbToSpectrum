using System;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using WilCommon;

namespace RgbToSpectrum
{
    public class Filter
    {
        // Transmittance curves according to wavelength
        // based on values available at http://www.karmalimbo.com/aro/pics/filters/transmision%20of%20wratten%20filters.pdf
        static readonly int BinsCount = 31;
        static readonly double[] Lambdas =   {  400,   410,   420,   430,   440,   450,   460,   470,   480,   490,   500,   510,   520,   530,   540,   550,   560,   570,   580,   590,   600,   610,   620,   630,   640,   650,   660,   670,   680,   690,   700 };
        static readonly double[] Warming85 = { .060, 0.180, 0.284, 0.334, 0.362, 0.381, 0.404, 0.430, 0.453, 0.472, 0.489, 0.492, 0.482, 0.483, 0.492, 0.510, 0.558, 0.645, 0.750, 0.830, 0.872, 0.889, 0.900, 0.905, 0.907, 0.909, 0.910, 0.910, 0.910, 0.910, 0.910 };

        public double LambdaMin  { get { return Lambdas[ 0]; } }
        public double LambdaMax { get { return Lambdas[BinsCount-1]; } }
        public double LambdaStep { get { return Lambdas[1] - Lambdas[0]; } }

        public Filter()
        {
        }

        public double Sample(double lambda)
        {
            // find closest smaller lambda
            int i;
            for(i = 0; i < BinsCount-1; ++i)
            {
                if (Lambdas[i + 1] > lambda)
                    break;
            }
            if (lambda >= Lambdas[BinsCount - 1])
                i = BinsCount - 1;

            return Warming85[i];
        }


        public Bitmap ToBitmap()
        {
            return Warming85.ToSnakeCurve(512).ToBitmap();
        }

    }
}
