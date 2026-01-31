using Math.Interfaces;

namespace Math.Numbers
{
    public readonly struct Complex<T> : IField<Complex<T>>
        where T : IComplexBase<T>
    {
        public readonly T Real;
        public readonly T Imaginary;

        public Complex(T real, T imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public Complex(T real) : this(real, T.FromInt(0)) { }

        public Complex(int real, int imaginary) : this(T.FromInt(real), T.FromInt(imaginary)) { }

        public Complex(int real) : this(real, 0) { }

        public static Complex<T> FromInt(int num) => new(num);

        public static Complex<T> operator +(Complex<T> left, Complex<T> right) => new(left.Real + right.Real, left.Imaginary + right.Imaginary);

        public static Complex<T> operator -(Complex<T> left, Complex<T> right) => new(left.Real - right.Real, left.Imaginary - right.Imaginary);

        public static Complex<T> operator *(Complex<T> left, Complex<T> right)
        {
            T real = left.Real * right.Real - left.Imaginary * right.Imaginary;
            T imaginary = left.Real * right.Imaginary + left.Imaginary * right.Real;
            return new(real, imaginary);
        }

        public static Complex<T> operator /(Complex<T> left, Complex<T> right)
        {
            T denom = right.Real * right.Real + right.Imaginary * right.Imaginary;
            T real = left.Real * right.Real + left.Imaginary * right.Imaginary;
            T imaginary = left.Imaginary * right.Real - left.Real * right.Imaginary;
            return new(real / denom, imaginary / denom);
        }

        public static Complex<T> Exp(Complex<T> z)
        {
            T ea = T.Exp(z.Real);
            T cb = T.Cos(z.Imaginary);
            T sb = T.Sin(z.Imaginary);
            return new(ea * cb, ea * sb);
        }

        public static Complex<T> Ln(Complex<T> z)
        {
            T r = T.Sqrt(z.Real * z.Real + z.Imaginary * z.Imaginary);
            T t = T.Atan2(z.Imaginary, z.Real);
            return new(T.Ln(r), t);
        }

        public static readonly Complex<T> E = Exp(new Complex<T>(1));

        public static Complex<T> Inv(Complex<T> x) => new Complex<T>(1) / x;

        public static Complex<T> Pow(Complex<T> x, Complex<T> y) => Exp(y * Ln(x));

        public static Complex<T> Sqrt(Complex<T> x) => Pow(x, Inv(new Complex<T>(2)));

        public static Complex<T> Log(Complex<T> x, Complex<T> b) => Ln(x) / Ln(b);

        public static Complex<T> Sin(Complex<T> x) => (Exp(new Complex<T>(0, 1) * x) - Exp(new Complex<T>(0, -1) * x)) / new Complex<T>(0, 2);

        public static Complex<T> Cos(Complex<T> x) => (Exp(new Complex<T>(0, 1) * x) + Exp(new Complex<T>(0, -1) * x)) / new Complex<T>(2, 0);

        public static Complex<T> Tan(Complex<T> x) => Sin(x) / Cos(x);

        public static Complex<T> Sinh(Complex<T> x) => (Exp(x) - Exp(new Complex<T>(-1) * x)) / new Complex<T>(2);

        public static Complex<T> Cosh(Complex<T> x) => (Exp(x) + Exp(new Complex<T>(-1) * x)) / new Complex<T>(2);

        public static Complex<T> Arcsin(Complex<T> x) => new Complex<T>(0, -1) * Ln(new Complex<T>(0, 1) * x + Sqrt(new Complex<T>(1) - x * x));

        public static Complex<T> Arccos(Complex<T> x) => new Complex<T>(0, -1) * Ln(x + new Complex<T>(0, 1) * Sqrt(new Complex<T>(1) - x * x));

        public static Complex<T> Arctan(Complex<T> x) => new Complex<T>(0, 1) / new Complex<T>(2, 0) * Ln(new Complex<T>(0, 1) + x / new Complex<T>(0, 1) - x);

        public static readonly Complex<T> Pi = new Complex<T>(16) * Arctan(Inv(new Complex<T>(5))) - new Complex<T>(4) * Arctan(Inv(new Complex<T>(239)));

        public static readonly Complex<T> Tau = new Complex<T>(2) * Pi;
    }
}
