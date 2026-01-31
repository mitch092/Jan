using System.Collections.Concurrent;
using System.Numerics;

namespace Math
{
    public static class MathUtils
    {
        private static readonly ConcurrentDictionary<int, BigInteger> PowersOf10 = [];
        public static BigInteger Pow10(int number) => PowersOf10.GetOrAdd(number, val => BigInteger.Pow(10, val));

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

        public static T Max<T>(T left, T right) where T : IComparable<T> => left.CompareTo(right) <= 0 ? right : left;

        public static T Min<T>(T left, T right) where T : IComparable<T> => left.CompareTo(right) <= 0 ? left : right;
    }
}
