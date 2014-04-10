using MathNet.Numerics.LinearAlgebra;

namespace MathnetHelper
{
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
