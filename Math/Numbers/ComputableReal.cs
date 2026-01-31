using Math.Interfaces;
using static Math.MathUtils;

namespace Math.Numbers
{
    public readonly struct ComputableReal : IField<ComputableReal>
    {
        public readonly Func<int, FixedDecimal> Approx;

        public ComputableReal(Func<int, FixedDecimal> approx)
        {
            Approx = approx;
        }

        public static ComputableReal FromInt(int num) => new ComputableReal(p => new FixedDecimal(num, p));

        public Interval<FixedDecimal> GetInterval(int g)
        {
            FixedDecimal a = Approx(g);
            FixedDecimal one = new(1, g);
            return new(a - one, a + one);
        }

        public ComputableReal Apply(ComputableReal other, Func<FixedDecimal, FixedDecimal, FixedDecimal> op, bool checkZero = false)
        {
            ComputableReal cr = this;
            return new ComputableReal(p =>
            {
                int g = p;
                while (true)
                {
                    g += 2;
                    Interval<FixedDecimal> b = other.GetInterval(g);
                    if (checkZero == false || b.CompareTo(new Interval<FixedDecimal>(new FixedDecimal(0, g))) != PartialOrdering.Indeterminate)
                    {
                        Interval<FixedDecimal> a = cr.GetInterval(g);
                        Interval<FixedDecimal> val = a.Apply(b, op);
                        if (TryCertify(val, p, out var r))
                        {
                            return r;
                        }
                    }
                }
            });
        }

        public static ComputableReal operator +(ComputableReal left, ComputableReal right) => left.Apply(right, (a, b) => a + b);

        public static ComputableReal operator -(ComputableReal left, ComputableReal right) => left.Apply(right, (a, b) => a - b);

        public static ComputableReal operator *(ComputableReal left, ComputableReal right) => left.Apply(right, (a, b) => a * b);

        public static ComputableReal operator /(ComputableReal left, ComputableReal right) => left.Apply(right, (a, b) => a / b, true);
    }
}
