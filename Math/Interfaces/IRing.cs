using System.Numerics;

namespace Math.Interfaces
{
    public interface IRing<T> :
        IAdditionOperators<T, T, T>,
        ISubtractionOperators<T, T, T>,
        IMultiplyOperators<T, T, T>
        where T : IRing<T>
    {
        public static abstract T FromInt(int num);
    }
}
