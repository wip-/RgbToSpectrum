using System.Collections.Generic;
using System.Drawing;

namespace RgbToSpectrum
{
    public class Identity : Filter
    {
        public override double LambdaMin { get {return 300;} }
        public override double LambdaMax { get { return 800; } }
        public override double LambdaStep { get { return 20; } }

        public override IEnumerable<double> Lambdas 
        {
            get 
            { 
                for(double l = LambdaMin; l<=LambdaMax; l+=LambdaStep)
                    yield return l; 
            } 
        }
        public override IEnumerable<double> Values { get { yield return 1.0; } }

        public override double Sample(double lambda) { return 1.0; }
        
        public override Bitmap ToBitmap(bool spline)
        {
            return base.ToBitmap(false);
        }

    }
}
