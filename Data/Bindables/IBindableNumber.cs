namespace PBFramework.Data.Bindables
{
    public interface IBindableNumber<T> : IBindable<T>
        where T : struct
    {
        /// <summary>
        /// The minimum range of the bound number.
        /// </summary>
        T MinValue { get; set; }

        /// <summary>
        /// The maximum range of the bound number.
        /// </summary>
        T MaxValue { get; set; }
    }
}