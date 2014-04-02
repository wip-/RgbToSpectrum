using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using WilCommon;

namespace RgbToSpectrum
{
    public abstract class Filter
    {
        public String Name { get { return GetType().Name; } }

        public abstract double LambdaMin { get; }
        public abstract double LambdaMax { get; }
        public abstract double LambdaStep { get; }

        public abstract IEnumerable<double> Lambdas { get; }
        public abstract IEnumerable<double> Values { get; }

        
        public abstract double Sample(double lambda);

        public virtual Bitmap ToBitmap(bool spline)
        {
            if (spline)
                return Values.ToArray<double>().ToCatmullRomSpline(512, new Helpers.WavelengthRange { Start = LambdaMin, End = LambdaMax }).ToBitmap();
            else
                return Values.ToArray<double>().ToSnakeCurve(512, new Helpers.WavelengthRange { Start = LambdaMin, End = LambdaMax }).ToBitmap();
        }


    }
}
