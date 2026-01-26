using System.Numerics;

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
        // FromRational()
    }
}
