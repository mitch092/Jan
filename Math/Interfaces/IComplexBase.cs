namespace Math.Interfaces
{
    public interface IComplexBase<T> : IField<T> where T : IComplexBase<T>
    {
        public abstract static T Exp(T x);
        public abstract static T Ln(T x);
        public abstract static T Sin(T x);
        public abstract static T Cos(T x);
        public abstract static T Sqrt(T x);
        public abstract static T Atan2(T y, T x);
    }
}
