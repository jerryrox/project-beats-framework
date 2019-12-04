namespace PBFramework.Data.Bindables
{
    /// <summary>
    /// IBindable variant for read-only access.
    /// </summary>
    public interface IReadOnlyBindable : IBindable
    {
        /// <summary>
        /// Returns the inner value stored as Object type.
        /// </summary>
        new object RawValue { get; }
    }

    /// <summary>
    /// IBindable generic variant for read-only access.
    /// </summary>
    public interface IReadOnlyBindable<T> : IBindable<T>
    {
        /// <summary>
        /// Returns the inner value stored.
        /// </summary>
        new T Value { get; }
    }
}