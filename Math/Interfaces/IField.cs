using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Math.Interfaces
{
    public interface IField<T> : 
        IComparable<T>,
        IAdditionOperators<T, T, T>,
        ISubtractionOperators<T, T, T>,
        IMultiplyOperators<T, T, T>,
        IDivisionOperators<T, T, T>
        where T : IField<T>
    {
        public static abstract T FromInt(int num);
    }
}
