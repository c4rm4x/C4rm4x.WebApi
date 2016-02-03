namespace C4rm4x.WebApi.ExceptionShielding.Configuration
{
    /// <summary>
    /// Abstraction of ExceptionHandler
    /// </summary>
    public interface IExceptionHandlerData
    {
        /// <summary>
        /// Creates a new instance of IExceptionHandler with the configuration specified
        /// </summary>
        /// <returns>A new instance of IExceptionHandler as configured</returns>
        IExceptionHandler GetExceptionHandler();
    }
}
