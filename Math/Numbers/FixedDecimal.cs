using Math.Interfaces;
using System.Collections.Concurrent;
using System.Numerics;

namespace Math.Numbers
{
    /// <summary>
    /// This is a fixed precision decimal (base 10) number, implemented on top of C#'s BigInteger class.
    /// </summary>
    public readonly record struct FixedDecimal : IField<FixedDecimal>
    {
        private static readonly ConcurrentDictionary<int, BigInteger> PowersOf10 = [];
        public static BigInteger Pow10(int number) => PowersOf10.GetOrAdd(number, val => BigInteger.Pow(10, val));

        public readonly BigInteger Number;
        public readonly int Scale;

        private FixedDecimal(BigInteger number, int scale)
        {
            Number = number;
            Scale = scale;
        }

        /// <summary>
        /// This function divides BigIntegers and uses the remainder for banker's rounding.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BigInteger RDiv(BigInteger left, BigInteger right)
        {
            (BigInteger q, BigInteger r) = BigInteger.DivRem(left, right);
            if (r.IsZero) return q;

            BigInteger twiceR = BigInteger.Abs(r) * 2;
            BigInteger den = BigInteger.Abs(right);

            if (twiceR < den) return q;
            if (twiceR > den) return q + q.Sign;

            return q.IsEven ? q : q + q.Sign;
        }

        public FixedDecimal Rescale(int newScale)
        {
            int shift = Scale - newScale;
            if (shift == 0)
            {
                return this;
            }
            else if (shift > 0)
            {
                BigInteger div = Pow10(shift);
                return new(RDiv(Number, div), shift);
            }
            else
            {
                int absShift = -shift;
                BigInteger mul = Pow10(absShift);
                return new(Number * mul, absShift);
            }
        }

        public static (FixedDecimal, FixedDecimal) Align(FixedDecimal left, FixedDecimal right)
        {
            if (left.Scale == right.Scale)
            {
                return (left, right);
            }
            if (left.Scale < right.Scale)
            {
                return (left.Rescale(right.Scale), right);
            }
            else
            {
                return (left, right.Rescale(left.Scale));
            }
        }

        public static FixedDecimal operator +(FixedDecimal left, FixedDecimal right)
        {
            (left, right) = Align(left, right);
            return new(left.Number + right.Number, left.Scale);
        }

        public static FixedDecimal operator -(FixedDecimal left, FixedDecimal right)
        {
            (left, right) = Align(left, right);
            return new(left.Number - right.Number, left.Scale);
        }

        public static FixedDecimal operator *(FixedDecimal left, FixedDecimal right)
        {
            return new(left.Number * right.Number, left.Scale + right.Scale);
        }

        public static FixedDecimal operator /(FixedDecimal left, FixedDecimal right)
        {
            (left, right) = Align(left, right);
            BigInteger numerator = left.Number * Pow10(left.Scale);
            BigInteger quotient = RDiv(numerator, right.Number);
            return new(quotient, left.Scale);
        }

        public int CompareTo(FixedDecimal other)
        {
            (FixedDecimal left, FixedDecimal right) = Align(this, other);
            return left.CompareTo(right);
        }
    }
}
