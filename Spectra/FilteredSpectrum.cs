﻿using System;
using System.Collections.Generic;
using System.Drawing;
using WilCommon;

namespace RgbToSpectrum
{
    public class FilteredSpectrum
    {
        public double LambdaMin  { get; private set; }
        public double LambdaMax  { get; private set; }
        public double LambdaStep { get; private set; }

        public int BinsCount { get; private set; }
        List<double> Lambdas = new List<double>();
        List<double> Values = new List<double>();

        public FilteredSpectrum(SimpleSpectrum inputSpectrum, Filter filter)
        {
            LambdaMin = Math.Max(SimpleSpectrum.LambdaMin, filter.LambdaMin);
            LambdaMax = Math.Min(SimpleSpectrum.LambdaMax, filter.LambdaMax);
            LambdaStep = Math.Min(SimpleSpectrum.LambdaStep, filter.LambdaStep);

            for (var l = LambdaMin; l <= LambdaMax; l += LambdaStep)
            {
                Lambdas.Add(l);
                double colorSample  = inputSpectrum.Sample(l);
                double filterSample = filter.Sample(l);
                Values.Add(colorSample * filterSample);
            }

            BinsCount = Lambdas.Count;
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

            return Values[i];
        }

        public Bitmap ToBitmap(bool spline)
        {
            if (spline)
                return Values.ToArray().ToCatmullRomSpline(512, new Helpers.WavelengthRange { Start = LambdaMin, End = LambdaMax }).ToBitmap();
            else
                return Values.ToArray().ToSnakeCurve(512, new Helpers.WavelengthRange { Start = LambdaMin, End = LambdaMax }).ToBitmap();
        }
    }
}
