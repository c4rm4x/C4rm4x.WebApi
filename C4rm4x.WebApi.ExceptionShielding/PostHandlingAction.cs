namespace C4rm4x.WebApi.ExceptionShielding
{
    /// <summary>
    /// Determines what action should occur after an exception is handled by the 
    /// configured exception handling chain
    /// </summary>
    public enum PostHandlingAction
    {
        /// <summary>
        /// Do nothing after exception has been handled
        /// </summary>
        None = 0,

        /// <summary>
        /// Original exception should be rethrown
        /// </summary>
        ShouldRethrow,

        /// <summary>
        /// New exception should be thrown
        /// </summary>
        ThrowNewException,
    }
}
