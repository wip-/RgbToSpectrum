using System;
using System.Collections.Generic;
using System.Drawing;
using WilCommon;

namespace RgbToSpectrum
{
    public class ColorSpectrumFilter : Filter
    {
        public override double LambdaMin { get { return spectrum.LambdaMin; } }
        public override double LambdaMax { get { return spectrum.LambdaMax; } }
        public override double LambdaStep { get { return spectrum.LambdaStep; } }

        SimpleSpectrum spectrum = new SimpleSpectrum(1.0, 1.0, 1.0);
        
        public override IEnumerable<double> Lambdas 
        {
            get 
            {
                for (int i = 0; i < SimpleSpectrum.BinsCount; ++i)
                    yield return SimpleSpectrum.Lambdas[i]; 
            } 
        }

        public override IEnumerable<double> Values 
        {
            get 
            {
                for (int i = 0; i < SimpleSpectrum.BinsCount; ++i)
                    yield return spectrum.values[i]; 
            } 
        }

        public ColorSpectrumFilter()
        {
        }

        // R,  G,  B must be between [0,  1]
        public ColorSpectrumFilter(double r, double g, double b)
        {
            SetColor(r, g, b);
        }

        public override double Sample(double lambda) 
        { 
            return spectrum.Sample(lambda); 
        }
        
        public override Bitmap ToBitmap(bool spline)
        {
            return spectrum.ToBitmap(spline);
        }

        // R,  G,  B must be between [0,  1]
        public void SetColor(double r, double g, double b)
        {
            spectrum = new SimpleSpectrum(r, g, b);
        }

    }
}
