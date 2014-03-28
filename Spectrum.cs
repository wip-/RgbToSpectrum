using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Helpers;

namespace RgbToSpectrum
{
    public class Spectrum
    {
        static readonly double[] ls =  { 360, 365, 370, 375, 380, 385, 390, 395, 400, 405, 410, 415, 420, 425, 430, 435, 440, 445, 450, 455, 460, 465, 470, 475, 480, 485, 490, 495, 500, 505, 510, 515, 520, 525, 530, 535, 540, 545, 550, 555, 560, 565, 570, 575, 580, 585, 590, 595, 600, 605, 610, 615, 620, 625, 630, 635, 640, 645, 650, 655, 660, 665, 670, 675, 680, 685, 690, 695, 700, 705, 710, 715, 720, 725, 730, 735, 740, 745, 750, 755, 760, 765, 770, 775, 780, 785, 790, 795, 800 };
        static readonly double[] riX = { 0.000130, 0.000230, 0.000410, 0.000740, 0.001370, 0.002230, 0.004240, 0.007650, 0.014310, 0.023190, 0.043510, 0.077630, 0.134380, 0.214770, 0.283900, 0.328500, 0.348280, 0.348060, 0.336200, 0.318700, 0.290800, 0.251100, 0.195360, 0.142100, 0.095640, 0.057950, 0.032010, 0.014700, 0.004900, 0.002400, 0.009300, 0.029100, 0.063270, 0.109600, 0.165500, 0.225750, 0.290400, 0.359700, 0.433450, 0.512050, 0.594500, 0.678400, 0.762100, 0.842500, 0.916300, 0.978600, 1.026300, 1.056700, 1.062200, 1.045600, 1.002600, 0.938400, 0.854450, 0.751400, 0.642400, 0.541900, 0.447900, 0.360800, 0.283500, 0.218700, 0.164900, 0.121200, 0.087400, 0.063600, 0.046770, 0.032900, 0.022700, 0.015840, 0.011360, 0.008110, 0.005790, 0.004110, 0.002890, 0.002050, 0.001440, 0.001000, 0.000690, 0.000480, 0.000330, 0.000230, 0.000170, 0.000120, 0.000080, 0.000060, 0.000041, 0.000029, 0.000020, 0.000014, 0.000010 };
        static readonly double[] riY = { 0.000000, 0.000000, 0.000010, 0.000020, 0.000040, 0.000060, 0.000120, 0.000220, 0.000400, 0.000640, 0.001200, 0.002180, 0.004000, 0.007300, 0.011600, 0.016840, 0.023000, 0.029800, 0.038000, 0.048000, 0.060000, 0.073900, 0.090980, 0.112600, 0.139020, 0.169300, 0.208020, 0.258600, 0.323000, 0.407300, 0.503000, 0.608200, 0.710000, 0.793200, 0.862000, 0.914850, 0.954000, 0.980300, 0.994950, 1.000000, 0.995000, 0.978600, 0.952000, 0.915400, 0.870000, 0.816300, 0.757000, 0.694900, 0.631000, 0.566800, 0.503000, 0.441200, 0.381000, 0.321000, 0.265000, 0.217000, 0.175000, 0.138200, 0.107000, 0.081600, 0.061000, 0.044580, 0.032000, 0.023200, 0.017000, 0.011920, 0.008210, 0.005730, 0.004100, 0.002930, 0.002090, 0.001050, 0.001050, 0.000740, 0.000520, 0.000360, 0.000250, 0.000170, 0.000120, 0.000080, 0.000060, 0.000040, 0.000030, 0.000020, 0.000014, 0.000010, 0.000007, 0.000005, 0.000003 };
        static readonly double[] riZ = { 0.000610, 0.001080, 0.001950, 0.003490, 0.006450, 0.010550, 0.020050, 0.036210, 0.067850, 0.110200, 0.207400, 0.371300, 0.645600, 1.039050, 1.385600, 1.622960, 1.747060, 1.782600, 1.772110, 1.744100, 1.669200, 1.528100, 1.287640, 1.041900, 0.812950, 0.616200, 0.465180, 0.353300, 0.272000, 0.212300, 0.158200, 0.111700, 0.078250, 0.057250, 0.042160, 0.029840, 0.020300, 0.013400, 0.008750, 0.005750, 0.003900, 0.002750, 0.002100, 0.001800, 0.001650, 0.001400, 0.001100, 0.001000, 0.000800, 0.000600, 0.000340, 0.000240, 0.000190, 0.000100, 0.000050, 0.000030, 0.000020, 0.000010, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000 };

        static MathNet.Numerics.Interpolation.IInterpolation interpX = Interpolate.LinearSpline(ls, riX);
        static MathNet.Numerics.Interpolation.IInterpolation interpY = Interpolate.LinearSpline(ls, riY);
        static MathNet.Numerics.Interpolation.IInterpolation interpZ = Interpolate.LinearSpline(ls, riZ);

        static Matrix<double> XYZtoRGB = InitXYZtoRGB();

        // R, G, B must be between [0, 1]
        Spectrum(double R, double G, double B, 
            double minLambda, double maxLambda, int numSamples)
        {           
            var A = Init(numSamples, minLambda, maxLambda);

            // find matrix null space http://twitter.com/cdrnet/status/449003828642590720
            var svd = A.Svd(true); 
            var An = svd.VT.EnumerateRows(svd.Rank, A.ColumnCount-svd.Rank).ToArray();

            var rgb = Vector<double>.Build.DenseOfArray(new[] {R, G, B });
            var x = A.Inverse() * rgb;

            // TODO 
            //var spectrum = x + An * constr(ColorFunction, zeros, options);

              
        }

        static Matrix<double> InitXYZtoRGB()
        {
            //double rx = .625, ry = .340;  //sony data
            //double gx = .280, gy = .595;
            //double bx = .155, by = .070;
               
            //double rx = .600, ry = .350;  //less saturated version
            //double gx = .290, gy = .580;
            //double bx = .175, by = .090;

            //double rx = .628, ry = .346;   //CRT from Rogers
            //double gx = .268, gy = .588;
            //double bx = .150, by = .070;

            //double rx = .670, ry = .330;   //NTSC from Rogers
            //double gx = .210, gy = .710;
            //double bx = .140, by = .080;

            double rx = .640, ry = .330;   //data from W3C http://www.w3.org/Graphics/Color/sRGB 
            double gx = .300, gy = .600;
            double bx = .150, by = .060;

            double wx = .3333, wy = .3333; // equal energy white
            //double wx = .2830, wy = .2980;  // sony white

            double wY = 106.8;  //should be the area under the XYZ curves, picked to make white = 1

            double Yovery = wY / wy;
            double D =  (rx * (gy - by) + gx * (by - ry) + bx * (ry - gy));
            double Cr = (Yovery  * (wx * (gy - by) - wy * (gx - bx) + gx * by - bx * gy) / D);
            double Cg = (Yovery  * (wx * (by - ry) - wy * (bx - rx) + bx * ry - rx * by) / D);
            double Cb = (Yovery  * (wx * (ry - gy) - wy * (rx - gx) + rx * gy - gx * ry) / D);

            var toXYZ = Matrix<double>.Build.Dense(3, 3, new[]
            {
                rx * Cr, ry * Cr, (1.0 - (rx + ry)) * Cr,
                gx * Cg, gy * Cg, (1.0 - (gx + gy)) * Cg,
                bx * Cb, by * Cb, (1.0 - (bx + by)) * Cb 
            });

            return toXYZ.Inverse();
        }

        Vector<double> GetXYZ(double lambda)
        {
            return Vector<double>.Build.Dense( new[]
            {
                interpX.Interpolate(lambda), 
                interpY.Interpolate(lambda), 
                interpZ.Interpolate(lambda)
            });
        }

        Vector<double> GetRGB(double lambda)
        {
            return XYZtoRGB * GetXYZ(lambda);
        }

        Matrix<double> Init(int numSamples, double minLambda, double maxLambda)
        {
            var vals = Generate.LinearSpaced(numSamples, minLambda, maxLambda);
            var A = Matrix<double>.Build.Dense(3, numSamples, 0.0);
            var sum = Vector<double>.Build.Dense(3, 0.0);

            for (int i = 0; i < numSamples; ++i )
            {
                for (double j = vals[i]; j < (vals[i+1]-1); ++j)    // don't add the endpoints twice...
                {
                    sum += GetXYZ(j);                               // note: this causes problems for non-integer
                    A.SetColumn(i, A.Column(i) + GetRGB(j));        // endpoints....
                }
            }

            return A;
        }






        void ColorFunction(Vector<double> rgb, 
            Matrix<double> A, Matrix<double> An, Vector<double> x)
        {
            var y = x + An * rgb;

            double mx = 0;
            if (y.Max() > 1.0)                      // penalty for values greater than 1
            {
                mx = 100.0 * (y.Max() - 1.0);
            }

            var f = 10 * y.Diff().L2Norm() + mx;    // function to minimize (objective)
            var gMin = 0.0 - y.Min();               // constrain the spectrum between 0 and 1.2
            var gMax = y.Max() - 1.2;
        }



    }
}
