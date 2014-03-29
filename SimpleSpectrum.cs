using System;

using System.Diagnostics;
using System.Drawing;
using WilCommon;

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

        public double[] values = new double[21];


        // R,  G,  B must be between [0,  1]
        public SimpleSpectrum(double r,  double g,  double b)
        {
            double Rweigth = 0;
            double Gweigth = 0;
            double Bweigth = 0;
            double Cweigth = 0;
            double Mweigth = 0;
            double Yweigth = 0;
            double Wweigth = 0;

            // TODO implement formula at bottom of http://www.cs.utah.edu/~bes/papers/color/paper-node2.html

            if( r <= g && g <= b )
            {
                Wweigth = r;
                Cweigth = g - r;
                Bweigth = b - g;
            }
            else if( r <= b && b <= g )
            {
                Wweigth = r;
                Cweigth = b - r;
                Gweigth = g - b;
            }
            else if( g <= r && r <= b )
            {
                Wweigth = g;
                Mweigth = r - g;
                Bweigth = b - r;
            }
            else if( g <= b && b <= r )
            {
                Wweigth = g;
                Mweigth = b - g;
                Rweigth = r - b;
            }
            else if( b <= r && r <= g )
            {
                Wweigth = b;
                Yweigth = r - b;
                Gweigth = g - r;
            }
            else if( b <= g && g <= r )
            {
                Wweigth = b;
                Yweigth = g - b;
                Rweigth = r - g;
            }
            else
            {
                Debugger.Break();
            }

            for(int i=0; i<21; ++i)
            {
                values[i] =
                    Wweigth * Wspectrum[i] +
                    Rweigth * Rspectrum[i] +
                    Gweigth * Gspectrum[i] +
                    Bweigth * Bspectrum[i] +
                    Cweigth * Cspectrum[i] +
                    Mweigth * Mspectrum[i] +
                    Yweigth * Yspectrum[i];
            }
        }

        public Bitmap ToBitmap()
        {
            BitmapInfo bitmapInfo = new BitmapInfo(512, 512, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            // render white background
            for(var x=0; x<512; ++x)
            for(var y=0; y<512; ++y)
                bitmapInfo.SetPixelColor(x, y, Color.White);

            for(var i=0; i<21; ++i)
            {
                var s = values[i];
                int yCurrent = (int)Helpers.Lerp(s, 0, 1, 508, 4);

                for(var j=0; j<24; ++j)
                {
                    int x = 4 + 24 * i + j;
                    bitmapInfo.SetPixelColor(x, yCurrent, Color.Black);
                    if(j==23 && i!=20)
                    {
                        var sNext = values[i+1];
                        int yNext = (int)Helpers.Lerp(sNext, 0, 1, 508, 4);
                        int yStart = Math.Min(yCurrent, yNext);
                        int yEnd = Math.Max(yCurrent, yNext);
                        for (var y = yStart; y <= yEnd; ++y)
                        {
                            bitmapInfo.SetPixelColor(x, y, Color.Black);
                        }
                    }
                }
            }




            return bitmapInfo.ToBitmap();
        }

    }
}
