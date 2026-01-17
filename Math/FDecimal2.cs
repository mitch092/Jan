using System.Collections.Concurrent;
using System.Numerics;

namespace Math
{
    /// <summary>
    /// This is a fixed precision decimal (base 10) number, implemented on top of C#'s BigInteger class.
    /// For now, the scaling factor is hardcoded to 4. This means that there are 4 digits after the decimal.
    /// </summary>
    public readonly record struct FDecimal2 :
        IAdditionOperators<FDecimal2, FDecimal2, FDecimal2>,
        ISubtractionOperators<FDecimal2, FDecimal2, FDecimal2>,
        IMultiplyOperators<FDecimal2, FDecimal2, FDecimal2>,
        IDivisionOperators<FDecimal2, FDecimal2, FDecimal2>
    {
        public const int WorkingScale = 8;
        public const int OutputScale = 4;
        private static readonly ConcurrentDictionary<int, BigInteger> PowersOf10 = [];

        private readonly int Scale;
        public readonly BigInteger Number;

        public static BigInteger Pow10(int number) => PowersOf10.GetOrAdd(number, val => BigInteger.Pow(10, val));

        private FDecimal2(BigInteger number, int scale = WorkingScale)
        {
            Scale = scale;
            Number = number;
        }

        public static FDecimal2 FromInt(int num) => new(num * Pow10(WorkingScale));

        public static implicit operator FDecimal2(int num) => FromInt(num);

        public FDecimal2 WithScale(int newScale)
        {
            if (newScale == Scale) return this;

            int delta = newScale - Scale;

            if (delta > 0)
            {
                return new(Number * Pow10(delta), newScale);
            }

            BigInteger factor = Pow10(System.Math.Abs(delta));

            BigInteger q = BigInteger.DivRem(Number, factor, out BigInteger r);

            BigInteger twiceR = BigInteger.Abs(r) * 2;
            BigInteger absFactor = BigInteger.Abs(factor);

            bool roundUp = twiceR > absFactor || (twiceR == absFactor && !q.IsEven);

            if (roundUp)
            {
                q += Number.Sign;
            }

            return new FDecimal2(q, newScale);
        }

        public static void Align(ref FDecimal2 left, ref FDecimal2 right)
        {
            if (left.Scale == right.Scale) return;
            if (left.Scale > right.Scale)
            {
                right = right.WithScale(left.Scale);
            }
            else
            {
                left = left.WithScale(right.Scale);
            }
        }

        public static FDecimal2 operator +(FDecimal2 left, FDecimal2 right)
        {
            Align(ref left, ref right);
            return new FDecimal2(left.Number + right.Number, left.Scale).WithScale(WorkingScale);
        }

        public static FDecimal2 operator -(FDecimal2 left, FDecimal2 right)
        {
            Align(ref left, ref right);
            return new FDecimal2(left.Number - right.Number, left.Scale).WithScale(WorkingScale);
        }

        public static FDecimal2 operator *(FDecimal2 left, FDecimal2 right)
        {
            return new FDecimal2(left.Number * right.Number, left.Scale + right.Scale).WithScale(WorkingScale);
        }

        public static FDecimal2 operator /(FDecimal2 left, FDecimal2 right)
        {
            int guardDigits = right.ToString()!.Length + 2;
            int baseScale = System.Math.Max(left.Scale, right.Scale);
            int workScale = baseScale + guardDigits;
            int shift = workScale + right.Scale - left.Scale;
            BigInteger num = shift >= 0 ? left.Number * Pow10(shift) : left.Number / Pow10(System.Math.Abs(shift));
            BigInteger q = num / right.Number;
            FDecimal2 tmp = new(q, workScale);
            return tmp.WithScale(WorkingScale);
        }
    }
}
