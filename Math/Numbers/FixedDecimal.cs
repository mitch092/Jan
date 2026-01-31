using Math.Interfaces;
using System.Numerics;
using static Math.MathUtils;

namespace Math.Numbers
{
    /// <summary>
    /// This is a fixed precision decimal (base 10) number, implemented on top of C#'s BigInteger class.
    /// </summary>
    public readonly struct FixedDecimal : IOrderedField<FixedDecimal>
    {
        public readonly BigInteger Number;
        public readonly int Scale;

        public FixedDecimal(BigInteger number, int scale)
        {
            Number = number * Pow10(scale);
            Scale = scale;
        }

        public static FixedDecimal FromInt(int num) => new(num, 10);

        public BigRational ToRational() => new(Number, Pow10(Scale));

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
                return new(RDiv(Number, div), newScale);
            }
            else
            {
                int absShift = -shift;
                BigInteger mul = Pow10(absShift);
                return new(Number * mul, newScale);
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

        // TODO: Implement TryParse and ToString for decimal strings.
        // use regex for parsing inputs.
    }
}
