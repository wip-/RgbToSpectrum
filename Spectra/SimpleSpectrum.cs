﻿
using System.Diagnostics;
using System.Drawing;
using WilCommon;

namespace RgbToSpectrum
{
    public enum Primary : int { R, G, B, C, M, Y, W, Count }

    public class SimpleSpectrum
    {
        // values read from Brian Smits paper curve images pixels - approximative for the least
        public static readonly int BinsCount = 21;
        public static readonly double[] Lambdas =   { 361.856, 383.505, 405.155, 426.804, 448.454, 470.103, 491.753, 513.402, 535.052, 556.701, 578.351, 600.000, 621.649, 643.299, 664.948, 686.598, 708.247, 729.897, 751.546, 773.196, 794.845 };

        static readonly double[] Rspectrum = { 0.09559, 0.09559, 0.08824, 0.07353, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.08824, 0.69853, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000 };
        static readonly double[] Gspectrum = { 0.00000, 0.00000, 0.00000, 0.00000, 0.03676, 0.38971, 0.78676, 1.00000, 1.00000, 0.77941, 0.31618, 0.00735, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000 };
        static readonly double[] Bspectrum = { 0.99265, 1.00000, 1.00000, 1.00000, 0.86765, 0.61029, 0.30882, 0.08088, 0.00000, 0.00000, 0.00000, 0.02941, 0.05147, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618, 0.06618 };

        static readonly double[] Cspectrum = { 0.99265, 0.99265, 0.98529, 0.92647, 0.98529, 1.00000, 1.00000, 1.00000, 1.00000, 0.91912, 0.30147, 0.00000, 0.00000, 0.00000, 0.00000, 0.00000, 0.00735, 0.00735, 0.00735, 0.00735, 0.00735 };
        static readonly double[] Mspectrum = { 1.00000, 1.00000, 1.00000, 1.00000, 0.97794, 0.61765, 0.19853, 0.00000, 0.00000, 0.24265, 0.68382, 0.98529, 1.00000, 1.00000, 1.00000, 0.99265, 0.97794, 0.97794, 0.98529, 0.98529, 0.98529 };
        static readonly double[] Yspectrum = { 0.00735, 0.00735, 0.00000, 0.00000, 0.30147, 0.39706, 0.69853, 0.92647, 1.00000, 1.00000, 1.00000, 0.97059, 0.95588, 0.95588, 0.95588, 0.96324, 0.97059, 0.98529, 0.99265, 1.00000, 1.00000 };

      //static readonly double[] Wspectrum = { 0.98750, 0.99554, 1.05714, 1.07589, 0.89643, 0.99554, 1.11607, 1.07857, 1.00089, 0.84286, 1.01964, 1.04643, 1.05179, 1.06250, 1.06518, 1.06518, 1.06250, 1.06518, 1.06786, 1.06786, 1.06518 };
        static readonly double[] Wspectrum = { 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000, 1.00000 };

        public static double SamplePrimarySpectrum(Primary primary, int index)
        {
            double[][] Spectra = new double[7][] { Rspectrum, Gspectrum, Bspectrum, Cspectrum, Mspectrum, Yspectrum, Wspectrum };
            return Spectra[(int)primary][index];
        }

        public double[] values = new double[BinsCount];

        //public double Rweight { get; private set; }
        //public double Gweight { get; private set; }
        //public double Bweight { get; private set; }
        //public double Cweight { get; private set; }
        //public double Mweight { get; private set; }
        //public double Yweight { get; private set; }
        //public double Wweight { get; private set; }

        public static double LambdaMin  { get { return Lambdas[ 0]; } }
        public static double LambdaMax  { get { return Lambdas[BinsCount-1]; } }
        public static double LambdaStep { get { return Lambdas[1]-Lambdas[0]; } }

        // R,  G,  B must be between [0,  1]
        public SimpleSpectrum(double r,  double g,  double b)
        {
            double Rweight = 0;
            double Gweight = 0;
            double Bweight = 0;
            double Cweight = 0;
            double Mweight = 0;
            double Yweight = 0;
            double Wweight = 0;

            if( r <= g && g <= b )
            {
                Wweight = r;
                Cweight = g - r;
                Bweight = b - g;
            }
            else if( r <= b && b <= g )
            {
                Wweight = r;
                Cweight = b - r;
                Gweight = g - b;
            }
            else if( g <= r && r <= b )
            {
                Wweight = g;
                Mweight = r - g;
                Bweight = b - r;
            }
            else if( g <= b && b <= r )
            {
                Wweight = g;
                Mweight = b - g;
                Rweight = r - b;
            }
            else if( b <= r && r <= g )
            {
                Wweight = b;
                Yweight = r - b;
                Gweight = g - r;
            }
            else if( b <= g && g <= r )
            {
                Wweight = b;
                Yweight = g - b;
                Rweight = r - g;
            }
            else
            {
                Debugger.Break();
            }

            for(int i=0; i<BinsCount; ++i)
            {
                values[i] =
                    Wweight * Wspectrum[i] +
                    Rweight * Rspectrum[i] +
                    Gweight * Gspectrum[i] +
                    Bweight * Bspectrum[i] +
                    Cweight * Cspectrum[i] +
                    Mweight * Mspectrum[i] +
                    Yweight * Yspectrum[i];
            }
        }

        public double Sample(double lambda)
        {
            // find closest smaller lambda
            int i;
            for (i = 0; i < BinsCount - 1; ++i)
            {
                if (Lambdas[i + 1] > lambda)
                    break;
            }
            if (lambda >= Lambdas[BinsCount - 1])
                i = BinsCount - 1;

            return values[i];
        }

        public Bitmap ToBitmap(bool spline)
        {
            if (spline)
                return values.ToCatmullRomSpline(512, new Helpers.WavelengthRange { Start = LambdaMin, End = LambdaMax }).ToBitmap();
            else
                return values.ToSnakeCurve(512, new Helpers.WavelengthRange { Start = LambdaMin, End = LambdaMax }).ToBitmap();
        }
    }
}
