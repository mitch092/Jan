using System.Numerics;
using System.Runtime.InteropServices;

namespace Math
{
    /// <summary>
    /// This is a fixed precision decimal (base 10) number, implemented on top of C#'s BigInteger class.
    /// For now, the scaling factor is hardcoded to 4. This means that there are 4 digits after the decimal.
    /// </summary>
    public readonly struct FDecimal :
        IAdditionOperators<FDecimal, FDecimal, FDecimal>,
        ISubtractionOperators<FDecimal, FDecimal, FDecimal>,
        IMultiplyOperators<FDecimal, FDecimal, FDecimal>,
        IDivisionOperators<FDecimal, FDecimal, FDecimal>,
        IEqualityOperators<FDecimal, FDecimal, bool>
    {
        public const int Scale = 4;
        public static readonly BigInteger ScaleFactor = BigInteger.Pow(10, Scale);
        public readonly BigInteger Number;

        private FDecimal(BigInteger number)
        {
            Number = number;
        }

        public static FDecimal FromInt(int num) => new(num * ScaleFactor);

        public static implicit operator FDecimal(int num) => FromInt(num);

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

        public static FDecimal operator +(FDecimal left, FDecimal right) => new(left.Number + right.Number);

        public static FDecimal operator -(FDecimal left, FDecimal right) => new(left.Number - right.Number);

        public static FDecimal operator *(FDecimal left, FDecimal right) => new(RDiv(left.Number * right.Number, ScaleFactor));

        public static FDecimal operator /(FDecimal left, FDecimal right) => new(RDiv(left.Number * ScaleFactor, right.Number));

        public static bool operator ==(FDecimal left, FDecimal right) => left.Number == right.Number;

        public static bool operator !=(FDecimal left, FDecimal right) => left.Number != right.Number;
    }
}
