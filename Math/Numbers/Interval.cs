using static Math.MathUtils;

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

        public Interval(T val) : this(val, val) { }

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
            if (Upper.CompareTo(other.Lower) < 0)
            {
                return PartialOrdering.LessThan;
            }
            else if (Lower.CompareTo(other.Upper) > 0)
            {
                return PartialOrdering.GreaterThan;
            }
            else if (Lower.CompareTo(other.Lower) == 0 && Upper.CompareTo(other.Upper) == 0)
            {
                return PartialOrdering.Equal;
            }
            else
            {
                return PartialOrdering.Indeterminate;
            }
        }

        public bool IsPoint => Lower.CompareTo(Upper) == 0;

        public static Interval<T> Apply(Interval<T> interval, Func<T, T> op) => new(op(interval.Lower), op(interval.Upper));

        public static Interval<T> Apply(Interval<T> left, Interval<T> right, Func<T, T, T> op)
        {
            T a = op(left.Lower, right.Lower);
            T b = op(left.Lower, right.Upper);
            T c = op(left.Upper, right.Lower);
            T d = op(left.Upper, right.Upper);
            T lower = Min(Min(a, b), Min(c, d));
            T upper = Max(Max(a, b), Max(c, d));
            return new(lower, upper);
        }
    }
}
