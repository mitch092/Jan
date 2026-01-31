using Math.Interfaces;
using System.Numerics;

namespace Math.Numbers
{
    public readonly struct BigRational : IOrderedField<BigRational>
    {
        public readonly BigInteger Num;
        public readonly BigInteger Den;

        public BigRational(BigInteger num, BigInteger den)
        {
            if (den.IsZero)
            {
                throw new DivideByZeroException();
            }

            if (den.Sign < 0)
            {
                num = BigInteger.Negate(num);
                den = BigInteger.Negate(den);
            }

            if (num.IsZero)
            {
                Num = 0;
                Den = 1;
            }
            else
            {
                var g = BigInteger.GreatestCommonDivisor(BigInteger.Abs(num), den);
                Num = num / g;
                Den = den / g;
            }
        }

        public static BigRational FromInt(int num) => new(num, 1);

        public static BigRational operator +(BigRational left, BigRational right) => new(left.Num * right.Den + right.Num * left.Den, left.Den * right.Den);

        public static BigRational operator -(BigRational left, BigRational right) => new(left.Num * right.Den - right.Num * left.Den, left.Den * right.Den);

        public static BigRational operator *(BigRational left, BigRational right) => new(left.Num * right.Num, left.Den * right.Den);

        public static BigRational operator /(BigRational left, BigRational right) => new(left.Num * right.Den, left.Den * right.Num);

        public int CompareTo(BigRational other)
        {
            BigInteger a = Num * other.Den;
            BigInteger b = Den * other.Num;
            return a.CompareTo(b);
        }

        /// <summary>
        /// Eventually, I would like to make a ToString() that displays the rational as a decimal string,
        /// does repeating decimal detection, and uses UTF-8 codepoints for the overbar for repeating decimals.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Den == 1)
            {
                return Num.ToString();
            }
            else
            {
                (BigInteger quotient, BigInteger remainder) = BigInteger.DivRem(Num, Den);
                return $"{quotient} {remainder}/{Den}";
            }
        }

        // TODO: Add ToParse method that reads decimal strings into a BigRational.
        //ToParse
    }
}
