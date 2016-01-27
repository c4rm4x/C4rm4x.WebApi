namespace C4rm4x.WebApi.Framework.Runtime
{
    /// <summary>
    /// Abstraction of an execution context extension
    /// </summary>
    public interface IExecutionContextExtension
    { }

    /// <summary>
    /// Abstraction of an execution context extension which exposes a value
    /// of type TValue
    /// </summary>
    /// <typeparam name="TValue">Type of the value</typeparam>
    public interface IExecutionContextExtension<TValue> :
        IExecutionContextExtension
    {
        /// <summary>
        /// Gets the value
        /// </summary>
        TValue Value { get; }
    }
}
