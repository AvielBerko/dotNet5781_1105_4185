using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Utility functions that can be used by the bl.
    /// </summary>
    static class Utils
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Returns a double number in the range [a, b), means equals or greater from a and less than b.
        /// </summary>
        public static double RandomDouble(double a, double b)
        {
            return random.NextDouble() * (a + b) + a;
        }

        /// <summary>
        /// Claps a value between two given values.
        /// </summary>
        /// <returns>
        /// If value is between a and b returns value. <br />
        /// If a is less than or equals to value, returns a <br />
        /// If b is greater than or equals to value, returns b.
        /// </returns>
        public static T Clap<T>(T value, T a, T b)
            where T : IComparable<T>
        {
            if (value.CompareTo(a) < 0) return a;
            if (value.CompareTo(b) > 0) return b;
            return value;
        }

        /// <summary>
        /// Returns the minimum value between a and b.
        /// </summary>
        /// <returns>If a is less than or equals to b, returns a. Else returns b.</returns>
        public static T Min<T>(T a, T b)
            where T : IComparable<T>
        {
            if (a.CompareTo(b) <= 0) return a;
            return b;
        }

        /// <summary>
        /// Returns the maximum value between a and b.
        /// </summary>
        /// <returns>If a is greater than or equals to b, returns a. Else returns b.</returns>
        public static T Max<T>(T a, T b)
            where T : IComparable<T>
        {
            if (a.CompareTo(b) >= 0) return a;
            return b;
        }

        /// <summary>
        /// Copies all the properties from one existing object to another exising object.
        /// </summary>
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }

        /// <summary>
        /// Copies all the properties from one existing object to another new object.
        /// </summary>
        /// <returns>The new created object with the given type.</returns>
        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }
    }
}
