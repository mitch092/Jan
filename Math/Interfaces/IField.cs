using System.Numerics;

namespace Math.Interfaces
{
    public interface IField<T> : 
        //IComparable<T>,
        //IComparisonOperators<T, T, bool>,
        //IEqualityOperators<T, T, bool>,
        IAdditionOperators<T, T, T>,
        ISubtractionOperators<T, T, T>,
        IMultiplyOperators<T, T, T>,
        IDivisionOperators<T, T, T>
        where T : IField<T>
    {

    }
}
