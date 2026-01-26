using System.Numerics;

namespace Math.Interfaces
{
    public interface IField<T> : 
        IRing<T>,
        IDivisionOperators<T, T, T>
        where T : IField<T>
    {
    }
}
