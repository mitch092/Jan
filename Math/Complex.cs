using System.Numerics;

namespace Math
{
    public readonly struct Complex :
        IAdditionOperators<Complex, Complex, Complex>,
        ISubtractionOperators<Complex, Complex, Complex>,
        IMultiplyOperators<Complex, Complex, Complex>,
        IDivisionOperators<Complex, Complex, Complex>,
        IEqualityOperators<Complex, Complex, bool>
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
            Imaginary = FDecimal.FromInt(0);
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

        public static bool operator ==(Complex left, Complex right) => left.Real == right.Real && left.Imaginary == right.Imaginary;

        public static bool operator !=(Complex left, Complex right) => !(left == right);

        public static Complex Exp(Complex x)
        {
            Complex term = new(FDecimal.FromInt(1));
            Complex sum = new(FDecimal.FromInt(1));
            Complex oldSum = new(FDecimal.FromInt(0));

            int i = 1;
            while (oldSum != sum)
            {
                term /= new Complex(FDecimal.FromInt(i));
                term *= x;
                sum += term;
                ++i;
            }

            return sum;
        }

        public static Complex Ln(Complex x) 
        {
            Complex yn = x - new Complex(FDecimal.FromInt(1));
            Complex yn1 = yn;
            Complex two = new(FDecimal.FromInt(2));

            do
            {
                yn = yn1;
                Complex exp = Exp(yn);
                yn1 = yn + two * (x - exp) / (x + exp);
            } while (yn != yn1);

            return yn1;
        }
    }
}
