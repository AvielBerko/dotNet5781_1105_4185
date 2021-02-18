using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    static class Utils
    {
        private static readonly Random random = new Random();
        public static double RandomDouble(double a, double b)
        {
            return random.NextDouble() * (a + b) + a;
        }
        public static T Clap<T>(T value, T a, T b)
            where T : IComparable<T>
        {
            if (value.CompareTo(a) < 0) return a;
            if (value.CompareTo(b) > 0) return b;
            return value;
        }
        public static T Min<T>(T a, T b)
            where T : IComparable<T>
        {
            if (a.CompareTo(b) <= 0) return a;
            return b;
        }
        public static T Max<T>(T a, T b)
            where T : IComparable<T>
        {
            if (a.CompareTo(b) >= 0) return a;
            return b;
        }
    }
}
