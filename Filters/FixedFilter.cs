using System;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using WilCommon;
using System.Collections.Generic;

namespace RgbToSpectrum
{
    public abstract class FixedFilter : Filter
    {
        protected double[] lambdas;
        protected double[] values;

        public override double LambdaMin { get { return lambdas[0]; } }
        public override double LambdaMax { get { return lambdas[BinsCount - 1]; } }
        public override double LambdaStep { get { return lambdas[1] - lambdas[0]; } }

        public override IEnumerable<double> Lambdas 
        { 
            get
            {
                for (int i = 0; i < BinsCount; ++i )
                    yield return lambdas[i];
            }
        }
        public override IEnumerable<double> Values
                { 
            get
            {
                for (int i = 0; i < BinsCount; ++i )
                    yield return values[i];
            }
        }

        public int BinsCount { get; private set; }


        public FixedFilter()
        {
            Initialize();

            if (lambdas.Length != values.Length)
                throw new Exception(String.Format("{0}:lambdas.Length != values.Length", GetType().Name));
            BinsCount = lambdas.Length;
        }

        public override double Sample(double lambda)
        {
            // snake curve sample - find closest smaller lambda
            int i;
            for(i = 0; i < BinsCount-1; ++i)
            {
                if (Lambdas.ElementAt(i + 1) > lambda)
                    break;
            }
            if (lambda >= Lambdas.ElementAt(BinsCount - 1))
                i = BinsCount - 1;

            return Values.ElementAt(i);
        }

        public abstract void Initialize();

    }

    public class WarmingFilter85 : FixedFilter
    {
        public override void Initialize()
        {
            // Transmittance curves according to wavelength
            // based on values available at http://www.karmalimbo.com/aro/pics/filters/transmision%20of%20wratten%20filters.pdf
            lambdas = new double[]{ 400, 410, 420, 430, 440, 450, 460, 470, 480, 490, 500, 510, 520, 530, 540, 550, 560, 570, 580, 590, 600, 610, 620, 630, 640, 650, 660, 670, 680, 690, 700 };
            values = new double[] { .060, 0.180, 0.284, 0.334, 0.362, 0.381, 0.404, 0.430, 0.453, 0.472, 0.489, 0.492, 0.482, 0.483, 0.492, 0.510, 0.558, 0.645, 0.750, 0.830, 0.872, 0.889, 0.900, 0.905, 0.907, 0.909, 0.910, 0.910, 0.910, 0.910, 0.910 };
        }
    }



}
