using MathNet.Numerics.LinearAlgebra;
using System;

namespace Helpers
{
    public static class Extensions
    {
        //http://stackoverflow.com/a/2683487/758666
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }   

        public static byte ToByte(this double val)
        {
            return Convert.ToByte(val.Clamp(0, 255));
        }
    }

    public static class Math
    {
        public static Vector<double> Diff(this Vector<double> x)
        {
            return Vector<double>.Build.Dense(x.Count - 1, i => x[i + 1] - x[i] );
        }









    //    /// <summary>
    //    /// Linearly interpolates over the value x between the points (xMin, yMin) and (xMax, yMax).
    //    /// </summary>
    //    public static T Lerp<T>(
    //        T x,
    //        T xMin, T xMax,
    //        T yMin, T yMax) where T : IComparable<T>
    //    {
    //        double ratio = (x - xMin) / (xMax - xMin);
    //        return yMin + ratio * (yMax - yMin);
    //    }  
    }

}
