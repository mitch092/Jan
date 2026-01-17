using System.Numerics;

namespace Math
{
    public readonly record struct Complex :
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

        public Complex(FDecimal real)
        {
            Real = real;
            Imaginary = 0;
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

        public static Complex Exp(Complex x)
        {
            Complex term = new(1);
            Complex sum = new(1);
            Complex oldSum = new(0);
            Complex i = new(1);

            while (oldSum != sum)
            {
                term /= i;
                term *= x;
                oldSum = sum;
                sum += term;
                i += new Complex(1);
            }

            return sum;
        }

        public static Complex Ln(Complex x)
        {
            Complex yn = x - new Complex(1);
            Complex yn1 = yn;
            Complex two = new(2);

            do
            {
                yn = yn1;
                Complex exp = Exp(yn);
                yn1 = yn + two * (x - exp) / (x + exp);
            } while (yn != yn1);

            return yn1;
        }

        public static readonly Complex E = Exp(new Complex(1));

        public static Complex Pow(Complex x, Complex y) => Exp(y * Ln(x));

        public static Complex Sqrt(Complex x) => Pow(x, new Complex(1) / new Complex(2));

        public static Complex Log(Complex x, Complex b) => Ln(x) / Ln(b);

        public static Complex Sin(Complex x) => (Exp(new Complex(0, 1) * x) - Exp(new Complex(0, -1) * x)) / new Complex(0, 2);

        public static Complex Cos(Complex x) => (Exp(new Complex(0, 1) * x) + Exp(new Complex(0, -1) * x)) / new Complex(2, 0);

        public static Complex Tan(Complex x) => Sin(x) / Cos(x);

        public static Complex Sinh(Complex x) => (Exp(x) - Exp(new Complex(-1) * x)) / new Complex(2);

        public static Complex Cosh(Complex x) => (Exp(x) + Exp(new Complex(-1) * x)) / new Complex(2);

        public static Complex Arcsin(Complex x) => new Complex(0, -1) * Ln(new Complex(0, 1) * x + Sqrt(new Complex(1) - x * x));

        public static Complex Arccos(Complex x) => new Complex(0, -1) * Ln(x + new Complex(0, 1) * Sqrt(new Complex(1) - x * x));

        public static Complex Arctan(Complex x) => new Complex(0, 1) / new Complex(2, 0) * Ln(new Complex(0, 1) + x / new Complex(0, 1) - x);
    }
}
