using System.Numerics;

namespace Math
{
    public readonly struct Complex :
        IAdditionOperators<Complex, Complex, Complex>,
        ISubtractionOperators<Complex, Complex, Complex>,
        IMultiplyOperators<Complex, Complex, Complex>,
        IDivisionOperators<Complex, Complex, Complex>
    {
        public readonly FDecimal Real;
        public readonly FDecimal Imaginary;

        public Complex(FDecimal real, FDecimal imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static Complex operator +(Complex left, Complex right) => new(left.Real + right.Real, left.Imaginary + right.Imaginary);

        public static Complex operator -(Complex left, Complex right) => new(left.Real - right.Real, left.Imaginary - right.Imaginary);

        public static Complex operator *(Complex left, Complex right)
        {
            FDecimal real = left.Real * right.Real - left.Imaginary * right.Imaginary;
            FDecimal imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real;
            return new(real, imaginary);
        }

        public static Complex operator /(Complex left, Complex right)
        {
            FDecimal denom = right.Real * right.Real + right.Imaginary * right.Imaginary;
            FDecimal real = left.Real * right.Real + left.Imaginary * right.Imaginary;
            FDecimal imaginary = left.Imaginary * right.Real - left.Real * right.Imaginary;
            return new(real / denom, imaginary / denom);
        }
    }
}
