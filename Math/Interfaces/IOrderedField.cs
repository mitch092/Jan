namespace Math.Interfaces
{
    public interface IOrderedField<T> :
        IField<T>,
        IComparable<T>
        where T : IOrderedField<T>
    {
    }
}
