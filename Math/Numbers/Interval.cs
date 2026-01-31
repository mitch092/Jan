using System;
using System.Collections.Generic;
using System.Text;

namespace Math.Numbers
{
    public readonly struct Interval<T> where T : IComparable<T>
    {
        public readonly T Lower;
        public readonly T Upper;

        public Interval(T left, T right)
        {
            if (left.CompareTo(right) <= 0)
            {
                Lower = left;
                Upper = right;
            }
            else
            {
                Lower = right;
                Upper = left;
            }
        }

        public enum PartialOrdering
        {
            LessThan,
            GreaterThan,
            Equal,
            Indeterminate
        }

        /// <summary>
        /// Returns whether or not this instance is less than,
        /// greater than, or equal to the other instance.
        /// If the intervals overlap, the ordering is indeterminate.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public PartialOrdering CompareTo(Interval<T> other)
        {

        }

        public bool IsPoint => Lower.CompareTo(Upper) == 0;

        public static Interval<T> Apply(Interval<T> interval, Func<T, T> op) => new(op(interval.Lower), op(interval.Upper));

        public static Interval<T> Apply(Interval<T> left, Interval<T> right, Func<T, T, T> op)
        {

        }
    }
}
