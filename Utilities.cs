using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using WilCommon;

namespace RgbToSpectrum
{
    public static class Utilities
    {

        /// <param name="fullFilename">full file path including extension</param>
        public static void ConvertImage(String fullFilename, Filter filter)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(fullFilename, FileMode.Open, FileAccess.Read, FileShare.None);
                Bitmap bitmap = new Bitmap(fs);
                BitmapInfo colorsIn = new BitmapInfo(bitmap);
                BitmapInfo colorsOut = new BitmapInfo(bitmap, BitmapInfo.CopyData.False);
                fs.Close();

                // TODO make parallel, cache converted colors
                for (var x = 0; x < colorsIn.Width; ++x)
                    for (var y = 0; y < colorsIn.Height; ++y)
                    {
                        var colorIn = colorsIn.GetPixelColor(x, y);
                        SimpleSpectrum spectrumIn = new SimpleSpectrum(
                            colorIn.RNormalized(),
                            colorIn.GNormalized(),
                            colorIn.BNormalized());
                        FilteredSpectrum spectrumOut = new FilteredSpectrum(spectrumIn, filter);
                        XYZColor xyz = new XYZColor(spectrumOut);
                        var colorOut = xyz.ToRGB();
                        colorsOut.SetPixelColor(x, y, colorOut);
                    }

                String newFileName = Path.GetDirectoryName(fullFilename) + @"\" + Path.GetFileNameWithoutExtension(fullFilename) + "-filtered" + Path.GetExtension(fullFilename);
                colorsOut.ToBitmap().Save(newFileName);

                Process.Start("explorer.exe", @"/select,""" + newFileName + "\"");
            }
            catch (System.Exception ex)
            {
                if (fs != null)
                    fs.Close();
                Helpers.MyCatch(ex);
            }
        }



        enum XYZComponent : int{X, Y, Z, Count }

        static readonly int LutSize   = 16;
        static readonly int BinsCount = 21; // TODO use more precision for precomputed data (in the end bins count does not appear in run-time algorithm)
        static readonly double[] X = { 0.000001, 0.000463, 0.025755, 0.230867, 0.349635, 0.190858, 0.023639, 0.030366, 0.217824, 0.545990, 0.898733, 1.055926, 0.823207, 0.393550, 0.115378, 0.020743, 0.002287, 0.000155, 0.000006, 0.000000, 0.000000 };
        static readonly double[] Y = { 0.000050, 0.000340, 0.001888, 0.008471, 0.030726, 0.090366, 0.229621, 0.570152, 0.917524, 0.996714, 0.887481, 0.634136, 0.354204, 0.151442, 0.048962, 0.011914, 0.002179, 0.000299, 0.000031, 0.000002, 0.000000 };
        static readonly double[] Z = { 0.000625, 0.010014, 0.112000, 1.155540, 1.783960, 1.289414, 0.422734, 0.127835, 0.029638, 0.004794, 0.000540, 0.000042, 0.000002, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000, 0.000000 };
        static double SampleMatchingCurve(XYZComponent component, int index)
        {
            double[][] MatchingCurves = new double[3][] { X, Y, Z};
            return MatchingCurves[(int)component][index];
        }

        // Pre-compute coefficients for a subset of 16x16x16 R,G,B input colors
        public static void PrecomputeCoefficients()
        {
            double[][][][][] M = Helpers.CreateJaggedArray<double[][][][][]>(
                new int[] { LutSize, LutSize, LutSize, (int)XYZComponent.Count, BinsCount });

            for (int rIn = 0; rIn < LutSize; ++rIn)
            for (int gIn = 0; gIn < LutSize; ++gIn)
            for (int bIn = 0; bIn < LutSize; ++bIn)
            {
                // TODO : try to compute complex spectrum for precomputed input colors
                SimpleSpectrum colorInSpectrum = new SimpleSpectrum(
                    (double)rIn / (LutSize - 1),
                    (double)gIn / (LutSize - 1),
                    (double)bIn / (LutSize - 1));

                for (int component = (int)XYZComponent.X; component < (int)XYZComponent.Count; ++component)
                for (int sampleIndex = 0; sampleIndex < BinsCount; ++sampleIndex)
                {
                    double colorInSample = colorInSpectrum.values[sampleIndex];
                    double matchingCurveSample = SampleMatchingCurve((XYZComponent)component, sampleIndex);

                    M[rIn][gIn][bIn][component][sampleIndex] = colorInSample * matchingCurveSample;
                }
            }



            double[][][][][] N = Helpers.CreateJaggedArray<double[][][][][]>(
                new int[] { LutSize, LutSize, LutSize, (int)XYZComponent.Count, (int)Primary.Count });

            for (int rIn = 0; rIn < LutSize; ++rIn)
            for (int gIn = 0; gIn < LutSize; ++gIn)
            for (int bIn = 0; bIn < LutSize; ++bIn)
            for (int component = (int)XYZComponent.X; component < (int)XYZComponent.Count; ++component)
            for (int primary = (int)Primary.R; primary < (int)Primary.Count; ++primary)
            {
                double sum = 0.0;
                for (int sampleIndex = 0; sampleIndex < BinsCount; ++sampleIndex)
                {
                    double m = M[rIn][gIn][bIn][component][sampleIndex];
                    double primarySample = SimpleSpectrum.SamplePrimarySpectrum((Primary)primary, sampleIndex);
                    sum += m * primarySample;
                }

                N[rIn][gIn][bIn][component][primary] = sum * SimpleSpectrum.LambdaStep;
            }

#if true
            // test: find min, max
            double min = double.MaxValue;   //   0.000000
            double max = double.MinValue;   // 106.884100562

            for (int rIn = 0; rIn < LutSize; ++rIn)
            for (int gIn = 0; gIn < LutSize; ++gIn)
            for (int bIn = 0; bIn < LutSize; ++bIn)
            for (int component = (int)XYZComponent.X; component < (int)XYZComponent.Count; ++component)
            for (int primary = (int)Primary.R; primary < (int)Primary.Count; ++primary)
            {
                double val = N[rIn][gIn][bIn][component][primary];
                if( val < min)
                    min = val;
                if (val > max)
                    max = val;
            }
#endif

            String[] ComponentNames = new String[3]{"X", "Y", "Z", };

            String fileName = "M.cpp";

            using (StreamWriter file = File.CreateText(fileName))
            {
                file.Write("float N[16][16][16][3][7] = \n{   ");
                for (int rIn = 0; rIn < LutSize; ++rIn)
                {
                    file.Write("{   ");
                    for (int gIn = 0; gIn < LutSize; ++gIn)
                    {
                        file.Write("{   ");
                        for (int bIn = 0; bIn < LutSize; ++bIn)
                        {
                            file.Write("{   // input rgb(" + (17 * rIn).ToString("000") + "," + (17 * gIn).ToString("000") + "," + (17 * bIn).ToString("000") + ")\n            ");
                            for (int component = (int)XYZComponent.X; component < (int)XYZComponent.Count; ++component)
                            {
                                //file.Write("\n                /*" + ComponentNames[component] + "*/ { ");
                                file.Write("    /*" + ComponentNames[component] + "*/ { ");
                                for (int primary = (int)Primary.R; primary < (int)Primary.Count; ++primary)
                                {
                                    file.Write(String.Format("{0:000.00000}f, ", N[rIn][gIn][bIn][component][primary]));
                                }
                                file.Write("}, ");
                                if (component != (int)XYZComponent.Count - 1) file.Write("\n            ");
                            }
                            file.Write("}, ");
                            if (bIn != LutSize - 1) file.Write("\n            ");
                        }
                        file.Write("}, ");
                        if (gIn != LutSize - 1) file.Write("\n        ");
                    }
                    file.Write("}, ");
                    if (rIn != LutSize - 1) file.Write("\n    ");
                }
                file.Write("}; ");
            }

            Process.Start("explorer.exe", @"/select,""" + AppDomain.CurrentDomain.BaseDirectory + fileName + "\"");


        }

    }
}
